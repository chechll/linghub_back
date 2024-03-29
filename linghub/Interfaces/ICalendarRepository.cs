﻿namespace linghub.Interfaces
{
    public interface ICalendarRepository
    {
        Calendar GetCalendar(int id);
        ICollection<Calendar> GetCalendars();
        int GetVisitsStreak(int id);
        bool isCalendarExist(int id);
        bool Save();
        bool CreateCalendar(Calendar calendar);
        bool DeleteCalendar(Calendar calendar);
        bool DeleteCalendars(List<Calendar> calendars);
        bool UpdateCalendar(Calendar calendar);
        List<int> GetAppointmentsCountByDay(int idUser);
        ICollection<Calendar> GetCalendarsToDeleteByUserId(int idUser);

    }
}
