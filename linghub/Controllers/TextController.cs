using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Models;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : Controller
    {
        private readonly ITextRepository _textRepository;
        private readonly IU_textRepository _u_textRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICheckDataRepository _checkDataRepository;

        public TextController(ITextRepository textRepository,
            IU_textRepository u_textRepository,            
            IMapper mapper,
            IUserRepository userRepository,
            ICheckDataRepository checkDataRepository)
        {
            _textRepository = textRepository;
            _u_textRepository = u_textRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _checkDataRepository = checkDataRepository;
        }

        public class TextsAmountResponse
        {
            public int SolvedTexts { get; set; }
            public int AllTexts { get; set; }
        }

        [HttpGet("GetAllTexts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Text>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllTexts()
        {
            try
            {
                var alltext = _textRepository.GetAllText();

                return Ok(alltext);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetCount")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Word>))]
        [ProducesResponseType(400)]
        public IActionResult GetAmount(int idUser)
        {
            if (!_userRepository.isUserExist(idUser))
                return BadRequest();

            int solvedTexts = _textRepository.GetSolvedTextCount(idUser);

            int allTexts = _textRepository.GetTextCount();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = new TextsAmountResponse
            {
                SolvedTexts = solvedTexts,
                AllTexts = allTexts
            };

            return Ok(response);
        }

        [HttpGet("GetText")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Text>))]
        [ProducesResponseType(400)]
        public IActionResult GetText(int idUser)
        {

            if (!_userRepository.isUserExist(idUser))
                return BadRequest();

            var id = _textRepository.GetUnsolvedTextId(idUser);

            if (!_textRepository.isTextExist(id))
                return NotFound();

            var text = _mapper.Map<TextDto>(_textRepository.GetText(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(text);
        }

        [HttpPost("CreateText")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateText([FromBody] TextDto textCreate)
        {
            if (textCreate == null)
                return BadRequest();

            var text = _textRepository.GetAllText().Where(c => c.IdText == textCreate.IdText).FirstOrDefault();

            if (text != null)
            {
                ModelState.AddModelError("", "text already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string resp = _checkDataRepository.checkText(textCreate);

            if (resp != "Ok")
            {
                ModelState.AddModelError("", resp);
                return StatusCode(422, ModelState);
            }

            var textMap = _mapper.Map<Text>(textCreate);

            if (!_textRepository.CreateText(textMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("UpdateText")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateText([FromForm] TextDto updatedText)
        {
            if (updatedText == null)
                return BadRequest(ModelState);

            if (!_textRepository.isTextExist(updatedText.IdText))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            string resp = _checkDataRepository.checkText(updatedText);

            if (resp != "Ok")
            {
                ModelState.AddModelError("", resp);
                return StatusCode(422, ModelState);
            }

            var textMap = _mapper.Map<Text>(updatedText);

            if (!_textRepository.UpdateText(textMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("DeleteText")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteText(int textId)
        {
            if (!_textRepository.isTextExist(textId))
            {
                return NotFound();
            }

            var textToDelete = _textRepository.GetText(textId);

            var uTextsToDelete = _u_textRepository.GetUTextsToDeleteByTextId(textId).ToList();

            if (uTextsToDelete != null && uTextsToDelete.Any())
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_u_textRepository.DeleteUTexts(uTextsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return StatusCode(500, ModelState);
                }
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_textRepository.DeleteText(textToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
