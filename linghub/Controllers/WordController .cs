using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
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
        public IActionResult GetWord(int id)
        {
            if (!_wordRepository.isWordExist(id))
                return NotFound();

            var word = _mapper.Map<WordDto>(_wordRepository.GetWord(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(word);
        }
    }
}
