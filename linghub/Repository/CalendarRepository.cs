using linghub.Data;
using linghub.Interfaces;

namespace linghub.Repository
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly LinghubContext _context;

        public CalendarRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateCalendar(Calendar calendar)
        {
            _context.Add(calendar);

            return Save();
        }

        public bool DeleteCalendar(Calendar calendar)
        {
            _context.Remove(calendar);

            return Save();
        }

        public Calendar GetCalendar(int id)
        {
            return _context.Calendars.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Calendar> GetCalendars()
        {
            return _context.Calendars.OrderBy(p => p.Id).ToList();
        }

        public int GetVisitsStreak(int id)
        {
            throw new NotImplementedException();
        }

        public bool isCalendarExist(int id)
        {
            return _context.Calendars.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCalendar(Calendar calendar)
        {
            _context.Update(calendar);
            return Save();
        }
    }
}
