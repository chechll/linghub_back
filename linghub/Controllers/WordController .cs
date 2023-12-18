using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using Microsoft.EntityFrameworkCore;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : Controller
    {
        private readonly IWordRepository _wordRepository;
        private readonly IU_wordRepository _u_wordRepository;
        private readonly IMapper _mapper;

        public WordController(IWordRepository wordRepository,
            IU_wordRepository u_wordRepository,
            IMapper mapper)
        {
            _wordRepository = wordRepository;
            _u_wordRepository = u_wordRepository;
            _mapper = mapper;
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Word>))]
        [ProducesResponseType(400)]
        public IActionResult GetWords(int id)
        {
            if (!_wordRepository.isWordExist(id))
                return NotFound();

            var word = _mapper.Map<WordDto>(_wordRepository.GetWord(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(word);
        }

        [HttpPost]
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

        [HttpPut("{wordId}")]
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
    }
}
