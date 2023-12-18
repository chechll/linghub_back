using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
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
        private readonly IMapper _mapper;

        public TextController(ITextRepository textRepository,
            IU_textRepository u_textRepository,            
            IMapper mapper)
        {
            _textRepository = textRepository;
            _u_textRepository = u_textRepository;
            _mapper = mapper;
        }

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Text>))]
        [ProducesResponseType(400)]
        public IActionResult GetText(int id)
        {
            if (!_textRepository.isTextExist(id))
                return NotFound();

            var text = _mapper.Map<TextDto>(_textRepository.GetText(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(text);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateText([FromBody] TextDto textCreate)
        {
            if (textCreate == null)
                return BadRequest();

            var text = _textRepository.GetAllText().Where(c => c.IdText == textCreate.IdText).FirstOrDefault();

            if (text != null)
            {
                ModelState.AddModelError("", "word already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var textMap = _mapper.Map<Text>(textCreate);

            if (!_textRepository.CreateText(textMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{textId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateText(int textId,
            [FromBody] TextDto updatedText)
        {
            if (updatedText == null)
                return BadRequest(ModelState);

            if (textId != updatedText.IdText)
                return BadRequest(ModelState);

            if (!_textRepository.isTextExist(textId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var textMap = _mapper.Map<Text>(updatedText);

            if (!_textRepository.UpdateText(textMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
    }
}
