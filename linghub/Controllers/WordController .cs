using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using Microsoft.EntityFrameworkCore;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;
using linghub.Models;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : Controller
    {
        private readonly IWordRepository _wordRepository;
        private readonly IU_wordRepository _u_wordRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public WordController(IWordRepository wordRepository,
            IU_wordRepository u_wordRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _wordRepository = wordRepository;
            _u_wordRepository = u_wordRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public class WordsAmountResponse
        {
            public int SolvedWords { get; set; }
            public int AllWords { get; set; }
        }

        [HttpGet("GetCount")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Word>))]
        [ProducesResponseType(400)]
        public IActionResult GetAmount(int idUser)
        {
            if (!_userRepository.isUserExist(idUser))
                return BadRequest();

            int solvedWords = _wordRepository.GetSolvedWordCount(idUser);

            int allWords = _wordRepository.GetWordCount();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = new WordsAmountResponse
            {
                SolvedWords = solvedWords,
                AllWords = allWords
            };

            return Ok(response);
        }

        [HttpGet("GetWord")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Word>))]
        [ProducesResponseType(400)]
        public IActionResult GetWords(int idUser)
        {
            if (!_userRepository.isUserExist(idUser))
                return BadRequest();

            var id = _wordRepository.GetUnsolvedWordId(idUser);

            if (!_wordRepository.isWordExist(id))
                return NotFound();

            var word = _mapper.Map<WordDto>(_wordRepository.GetWord(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(word);
        }

        [HttpPost("CreateWord")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateWord([FromBody] WordDto wordCreate)
        {
            if (wordCreate == null)
                return BadRequest();

            var word = _wordRepository.GetWords().Where(c => c.IdWord == wordCreate.IdWord).FirstOrDefault();

            if (word != null)
            {
                ModelState.AddModelError("", "word already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var wordMap = _mapper.Map<Word>(wordCreate);

            if (!_wordRepository.CreateWord(wordMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("UpdateWord")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWord(int wordId,
            [FromBody] WordDto updatedWord)
        {
            if (updatedWord == null)
                return BadRequest(ModelState);

            if (wordId != updatedWord.IdWord)
                return BadRequest(ModelState);

            if (!_wordRepository.isWordExist(wordId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var wordMap = _mapper.Map<Word>(updatedWord);

            if (!_wordRepository.UpdateWord(wordMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("DeleteWord")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWord(int wordId)
        {
            if (!_wordRepository.isWordExist(wordId))
            {
                return NotFound();
            }

            var wordToDelete = _wordRepository.GetWord(wordId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_wordRepository.DeleteWord(wordToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
