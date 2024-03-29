﻿using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Models;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System;
using Azure;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IU_textRepository _u_textRepository;
        private readonly IU_wordRepository _u_wordRepository;
        private readonly ICheckDataRepository _checkDataRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository,
            ICheckDataRepository checkDataRepository,
            ICalendarRepository calendarRepository,
            IU_textRepository u_textRepository,
            IU_wordRepository u_wordRepository,
            IMapper mapper)
        {
            _checkDataRepository = checkDataRepository;
            _u_textRepository = u_textRepository;
            _u_wordRepository = u_wordRepository;
            _calendarRepository = calendarRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public class OperatingData
        {
            public int idUser { get; set; }
            public int isAdmin { get; set; }
        }

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllErrors()
        {
            try
            {
                var alluser = _userRepository.GetUsers();

                return Ok(alluser);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("LogIn")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult LogIn(string email, string user_password)
        {


            int id = _userRepository.GetId(email);
            if (!_userRepository.isUserExist(id))
                return NotFound();
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_checkDataRepository.IsValidEmail(email))
            {
                ModelState.AddModelError("", "your mail is invalid");
                return StatusCode(422, ModelState);
            }

            if (!_checkDataRepository.CheckStringLengs(email, 30))
            {
                ModelState.AddModelError("", "your mail length is more then 30");
                return StatusCode(422, ModelState);
            }

            if (!_checkDataRepository.CheckStringLengs(user_password, 30))
            {
                ModelState.AddModelError("", "your password length is more then 30");
                return StatusCode(422, ModelState);
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(user_password, user.UserPassword)) {
                var response = new OperatingData
                {
                    idUser = id,
                    isAdmin = user.Admin
                };

                return Ok(response);
            } else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id)
        {
            if (!_userRepository.isUserExist(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost("SignUp")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest();

            var user = _userRepository.GetUsers().Where(c => c.Email == userCreate.Email).FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "user already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string resp = _checkDataRepository.checkUser(userCreate);

            if (resp != "Ok")
            {
                ModelState.AddModelError("", resp);
                return StatusCode(422, ModelState);
            }

            if (userCreate.Admin != 0 && userCreate.Admin != null)
            {
                ModelState.AddModelError("", "You have no right to do it");
                return StatusCode(422, ModelState);
            }

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userCreate.UserPassword, 13);
            userCreate.UserPassword = passwordHash;

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            var userId = _userRepository.GetId(userCreate.Email );

            user = _userRepository.GetUser(userId);
            
            var response = new OperatingData
            {
                idUser = userId,
                isAdmin = user.Admin
            };

            return Ok(response);

        }

        [HttpPut("Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(
            [FromForm] UserDto updatedUser)
        {
            bool isUpdateNeeded = false;
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (!_userRepository.isUserExist(updatedUser.IdUser))
                return NotFound();

            if (updatedUser.IdUser != _userRepository.GetId(updatedUser.Email) && _userRepository.isUserExist(_userRepository.GetId(updatedUser.Email)))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model state is not valid");
                return BadRequest(ModelState);
            }

            string resp = _checkDataRepository.checkUser(updatedUser);

            if (resp != "Ok")
            {
                ModelState.AddModelError("", resp);
                return StatusCode(422, ModelState);
            }

            if (updatedUser.Admin > 2 && updatedUser.Admin < 0)
            {
                ModelState.AddModelError("", "there is no such isAdmin");
                return StatusCode(422, ModelState);
            }

            var user = _userRepository.GetUser(updatedUser.IdUser);

            
            if (updatedUser.Name != user.Name)
            {
                user.Name = updatedUser.Name;
                isUpdateNeeded = true;
            }

            if (updatedUser.Photo != null)
            {
                user.Photo = updatedUser.Photo;
                isUpdateNeeded = true;
            }

            if (updatedUser.Email != user.Email)
            {
                user.Email = updatedUser.Email;
                isUpdateNeeded = true;
            }

            if (updatedUser.Surname != user.Surname)
            {
                user.Surname = updatedUser.Surname;
                isUpdateNeeded = true;
            }


            if (updatedUser.Admin != user.Admin)
            {
                user.Admin = updatedUser.Admin;
                isUpdateNeeded = true;
            }

            if (updatedUser.UserPassword != user.UserPassword) 
            {
                var changeMail = BCrypt.Net.BCrypt.EnhancedVerify(updatedUser.UserPassword, user.UserPassword);
                if (!changeMail)
                {
                    string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(updatedUser.UserPassword, 13);
                    user.UserPassword = passwordHash;
                    isUpdateNeeded = true;
                }
            }


            if (isUpdateNeeded)
            {
                var userMap = _mapper.Map<User>(user);
                if (!_userRepository.UpdateUser(userMap))
                {
                    ModelState.AddModelError("", "Something went wrong ");
                    return StatusCode(500, ModelState);
                }
            }
            return Ok("Successfully updated");
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.isUserExist(userId))
            {
                return NotFound();
            }
            var userToDelete = _userRepository.GetUser(userId);

            var calendarsToDelete = _calendarRepository.GetCalendarsToDeleteByUserId(userId).ToList();

            if (calendarsToDelete != null && calendarsToDelete.Any())
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_calendarRepository.DeleteCalendars(calendarsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return StatusCode(500, ModelState);
                }
            }
            var uWordsToDelete = _u_wordRepository.GetUwordsToDeleteByUserId(userId).ToList();

            if (uWordsToDelete != null && uWordsToDelete.Any())
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!_u_wordRepository.DeleteUWords(uWordsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return StatusCode(500, ModelState);
                }
            }

            var uTextsToDelete = _u_textRepository.GetUTextsToDeleteByUserId(userId).ToList();

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

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok(0);
        }
    }
}
