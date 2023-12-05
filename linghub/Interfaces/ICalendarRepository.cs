namespace linghub.Interfaces
{
    public interface ICalendarRepository
    {
        Calendar GetCalendar(int id);
        bool isCalendarExist(int id);
        bool Save();
        bool CreateCalendar(Calendar calendar);
        bool DeleteCalendar(Calendar calendar);
        bool UpdateCalendar(Calendar calendar);
    }
}
