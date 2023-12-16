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

        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Calendar>))]
        [ProducesResponseType(400)]
        public IActionResult GetCalendar(int id)
        {
            if (!_calendarRepository.isCalendarExist(id))
                return NotFound();

            var calendar = _mapper.Map<CalendarDto>(_calendarRepository.GetCalendar(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(calendar);
        }
    }
}
