using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : Controller
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CalendarController(ICalendarRepository calendarRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _calendarRepository = calendarRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("Appointments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Calendar>))]
        [ProducesResponseType(400)]
        public IActionResult GetCalendar(int idUser)
        {
            try
            {
                
                if (idUser <= 0)
                    return BadRequest();
                
                var appointmentsCountByDay = _calendarRepository.GetAppointmentsCountByDay(idUser);
                
                return Ok(new { AppointmentsCountByDay = appointmentsCountByDay });
            } catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddDate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCalendar([FromQuery] int idUser)
        {
            if (idUser <= 0)
                return BadRequest();

            var calendar = new Calendar {Datum =  DateTime.Today, IdUser = idUser };
                
                
            if(_calendarRepository.GetCalendars().Any(c => c.IdUser == calendar.IdUser && c.Datum == calendar.Datum))
                return Ok();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var calendarMap = _mapper.Map<Calendar>(calendar);

            if (!_calendarRepository.CreateCalendar(calendarMap))
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
        public IActionResult UpdateCalendar(int calendarId,
            [FromBody] CalendarDto updatedCalendar)
        {
            if (updatedCalendar == null)
                return BadRequest(ModelState);

            if (calendarId != updatedCalendar.Id)
                return BadRequest(ModelState);

            if (!_calendarRepository.isCalendarExist(calendarId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var calendarMap = _mapper.Map<Calendar>(updatedCalendar);

            if (!_calendarRepository.UpdateCalendar(calendarMap))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{calendarId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCalendar(int calendarId)
        {
            if (!_calendarRepository.isCalendarExist(calendarId))
            {
                return NotFound();
            }

            var calendarToDelete = _calendarRepository.GetCalendar(calendarId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_calendarRepository.DeleteCalendar(calendarToDelete))
            {
                ModelState.AddModelError("", "Something went wrong ");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted successfully");
        }
    }
}
