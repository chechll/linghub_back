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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public UserController(IUserRepository userRepository,
            IMapper mapper, TokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet("LogIn")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult LogIn(string email, string user_password)
        {
            int id = _userRepository.GetId(email, user_password);

            if (!_userRepository.isUserExist(id))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(id));
            // var worker = _workerRepository.GetWorker(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user.IdUser);
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

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            var userId = _userRepository.GetUsers().Where(c => c.Email == userCreate.Email).FirstOrDefault();

            return Ok(userId.IdUser);

        }

        [HttpPut("Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId,
            [FromBody] UserDto updatedUser)
        {

            Console.WriteLine("1");
            if (updatedUser == null)
                return BadRequest(ModelState);
            Console.WriteLine("2");
            Console.WriteLine(userId + " " + updatedUser.IdUser);
            if (userId != updatedUser.IdUser)
                return BadRequest(ModelState);
            Console.WriteLine("3");
            if (!_userRepository.isUserExist(userId))
                return NotFound();
            Console.WriteLine("4");
            if (!ModelState.IsValid)
                return BadRequest();
            Console.WriteLine("5");
            var userMap = _mapper.Map<User>(updatedUser);
            Console.WriteLine("6");
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }
            Console.WriteLine("7");
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
