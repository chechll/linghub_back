using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Models;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("AddHash")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult AddHash(int id)
        {
            
            if (!_userRepository.isUserExist(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.UserPassword, 13);

            //var result = BCrypt.Net.BCrypt.EnhancedVerify(user.UserPassword, passwordHash);

            user.UserPassword = passwordHash;

            var userMap = _mapper.Map<User>(user);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

        [HttpGet("LogIn")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult LogIn(string email, string user_password)
        {


            int id = _userRepository.GetId(email);
            Console.WriteLine("Log1");
            if (!_userRepository.isUserExist(id))
                return NotFound();
            Console.WriteLine("Log2");
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));
            // var worker = _workerRepository.GetWorker(id);
            Console.WriteLine("Log3");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Console.WriteLine("Log4");
            if (BCrypt.Net.BCrypt.EnhancedVerify(user_password, user.UserPassword)) {
                return Ok(user.IdUser);
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

            //userCreate.Admin = 0;

            var user = _userRepository.GetUsers().Where(c => c.Email == userCreate.Email).FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "user already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userCreate.UserPassword, 13);
            userCreate.UserPassword = passwordHash;

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            var userId = _userRepository.GetId(userCreate.Email );

            return Ok(userId);

        }

        [HttpPut("Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(
            [FromForm] UserDto updatedUser)
        {
            Console.WriteLine("1");
            bool isUpdateNeeded = false;
            if (updatedUser == null)
                return BadRequest(ModelState);
            Console.WriteLine("2");
            if (!_userRepository.isUserExist(updatedUser.IdUser))
                return NotFound();
            Console.WriteLine("3");
            if (updatedUser.IdUser != _userRepository.GetId(updatedUser.Email) && _userRepository.isUserExist(_userRepository.GetId(updatedUser.Email)))
                return BadRequest(ModelState);
            Console.WriteLine("4");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Model state is not valid");
                return BadRequest(ModelState);
            }
            Console.WriteLine("5");
            var user = _userRepository.GetUser(updatedUser.IdUser);

            Console.WriteLine("Updated user mail is ",updatedUser.Email);
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
            Console.WriteLine("6");

            if (updatedUser.Surname != user.Surname)
            {
                user.Surname = updatedUser.Surname;
                isUpdateNeeded = true;
            }

            Console.WriteLine("7");

            Console.WriteLine("User password is " + updatedUser.UserPassword);
            var changeMail = BCrypt.Net.BCrypt.EnhancedVerify(updatedUser.UserPassword, user.UserPassword);
            Console.WriteLine("ChangeMail is " + changeMail);
            if (!changeMail)
            {
                Console.WriteLine("8");
                string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(updatedUser.UserPassword, 13);
                user.UserPassword = passwordHash;
                isUpdateNeeded = true;
            }

            Console.WriteLine("9");
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


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
