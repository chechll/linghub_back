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
                var startOfWeek = today.AddDays(1 - (int)today.DayOfWeek );

               List<int> appointmentsCountByDay = new List<int>();
                for (int i = 0; i <=6; i++)
                {
                   if( _context.Calendars.Any(p => p.IdUser == idUser && p.Datum == startOfWeek.AddDays(i)))
                    {
                        appointmentsCountByDay.Add(1);
                    } else
                    {
                        appointmentsCountByDay.Add(0);
                    }
                }

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
