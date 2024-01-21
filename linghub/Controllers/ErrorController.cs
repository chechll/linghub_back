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
    public class ErrorController : Controller
    {
        private readonly IErrorRepository _errorRepository;
        private readonly IMapper _mapper;
        private readonly ICheckDataRepository _checkDataRepository;

        public ErrorController( IErrorRepository errorRepository,
            IMapper mapper,
            ICheckDataRepository checkDataRepository)
        {
            _errorRepository = errorRepository;
            _mapper = mapper;
            _checkDataRepository = checkDataRepository;
        }

        [HttpGet("GetAllErrors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Word>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllErrors()
        {
            try
            {
                var allErrors = _errorRepository.GetError();

                return Ok(allErrors);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddError")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUText([FromBody] ErrorDto errorCreate)
        {
            if (errorCreate == null)
                return BadRequest();

            var error = _errorRepository.GetError().Where(c => c.Id == errorCreate.Id).FirstOrDefault();

            if (error != null)
            {
                ModelState.AddModelError("", "error already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_checkDataRepository.IsValidEmail(errorCreate.Email))
            {
                ModelState.AddModelError("", "your mail is invalid");
                return StatusCode(422, ModelState);
            }

            if (!_checkDataRepository.CheckStringLengs(errorCreate.Email, 30))
            {
                ModelState.AddModelError("", "your mail length is more then 30");
                return StatusCode(422, ModelState);
            }

            if (!_checkDataRepository.IsEnglishSentence(errorCreate.Description) && !_checkDataRepository.IsUkranianSentence(errorCreate.Description))
            {
                ModelState.AddModelError("", "please write in english or ukranian and use only ,.!? symbols");
                return StatusCode(422, ModelState);
            }

            var errorMap = _mapper.Map<Error>(errorCreate);

            if (!_errorRepository.CreateError(errorMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpDelete("ErrorDelete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteError(int errorId)
        {
            if (!_errorRepository.isErrorExist(errorId))
            {
                return NotFound();
            }

            var errorToDelete = _errorRepository.GetError(errorId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_errorRepository.DeleteError(errorToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
