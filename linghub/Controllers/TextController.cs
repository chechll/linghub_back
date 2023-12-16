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
    }
}
