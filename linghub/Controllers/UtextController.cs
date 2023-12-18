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
    public class UtextController : Controller
    {
        private readonly ITextRepository _textRepository;
        private readonly IU_textRepository _u_textRepository;
        private readonly IMapper _mapper;

        public UtextController(IU_textRepository u_textRepository,
            ITextRepository textRepository,
            IMapper mapper)
        {
            _textRepository = textRepository;
            _u_textRepository = u_textRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUText([FromBody] UtextDto utextCreate)
        {
            if (utextCreate == null)
                return BadRequest();

            var utext = _u_textRepository.GetUTexts().Where(c => c.Id == utextCreate.Id).FirstOrDefault();

            if (utext != null)
            {
                ModelState.AddModelError("", "word already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var utextMap = _mapper.Map<UText>(utextCreate);

            if (!_u_textRepository.CreateUText(utextMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");

        }

    }
}
