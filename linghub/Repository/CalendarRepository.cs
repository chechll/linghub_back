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

        public bool DeleteCalendars(List<Calendar> calendars)
        {
            _context.RemoveRange(calendars);

            return Save();
        }

        public List<int> GetAppointmentsCountByDay(int idUser)
        {

            try 
            {
                    var today = DateTime.Today;
                    var startOfWeek = today.AddDays(1 - (int)today.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(6);
                   

                var appointmentsCountByDay = _context.Calendars
                 .Where(c => c.IdUser == idUser && c.Datum >= startOfWeek && c.Datum <= endOfWeek)
                 .AsEnumerable()
                 .GroupBy(c => c.Datum.DayOfWeek)
                 .OrderBy(g => g.Key)
                 .Select(g => g.Count())
                 .ToList();

                return appointmentsCountByDay;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public Calendar GetCalendar(int id)
        {
            return _context.Calendars.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Calendar> GetCalendars()
        {
            return _context.Calendars.OrderBy(p => p.Id).ToList();
        }

        public ICollection<Calendar> GetCalendarsToDeleteByUserId(int idUser)
        {
            return _context.Calendars.Where(p => p.IdUser == idUser).ToList();
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
