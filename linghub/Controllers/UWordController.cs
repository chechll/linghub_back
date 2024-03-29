﻿using AutoMapper;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UWordController(IU_wordRepository u_wordRepository,
            IWordRepository wordRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _u_wordRepository = u_wordRepository;
            _mapper = mapper;
        }

        [HttpPost("AddSolvedWord")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUWord([FromBody] UwordDto uwordCreate)
        {
            if (uwordCreate == null)
                return BadRequest();

            var word = _u_wordRepository.GetUWords().Where(c => c.Id == uwordCreate.Id).FirstOrDefault();

            if (word != null)
            {
                ModelState.AddModelError("", "uword already exists");
                return StatusCode(422, ModelState);
            }

            if (!_userRepository.isUserExist(uwordCreate.IdUser) || !_wordRepository.isWordExist(uwordCreate.IdWord))
            {
                ModelState.AddModelError("", "wrong data");
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
    }

}