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
    public class UWordController : Controller
    {
        private readonly IWordRepository _wordRepository;
        private readonly IU_wordRepository _u_wordRepository;
        private readonly IMapper _mapper;

        public UWordController(IU_wordRepository u_wordRepository,
            IWordRepository wordRepository,
            IMapper mapper)
        {
            _wordRepository = wordRepository;
            _u_wordRepository = u_wordRepository;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUWord([FromBody] UwordDto uwordCreate)
        {
            if (uwordCreate == null)
                return BadRequest();

            var word = _u_wordRepository.GetUWords().Where(c => c.Id == uwordCreate.Id).FirstOrDefault();

            if (word != null)
            {
                ModelState.AddModelError("", "word already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uwordMap = _mapper.Map<UWord>(uwordCreate);

            if (!_u_wordRepository.CreateUword(uwordMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{calendarId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUword(int uwordId,
            [FromBody] UwordDto updatedUword)
        {
            if (updatedUword == null)
                return BadRequest(ModelState);

            if (uwordId != updatedUword.Id)
                return BadRequest(ModelState);

            if (!_u_wordRepository.isUwordExist(uwordId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var uwordMap = _mapper.Map<UWord>(updatedUword);

            if (!_u_wordRepository.UpdateUword(uwordMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{uwordId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUword(int uwordId)
        {
            if (!_u_wordRepository.isUwordExist(uwordId))
            {
                return NotFound();
            }

            var uwordToDelete = _u_wordRepository.GetUWord(uwordId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_u_wordRepository.DeleteUword(uwordToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }

}