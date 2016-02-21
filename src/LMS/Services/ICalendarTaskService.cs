using LMS.Areas.Api.ViewModels;

namespace LMS.Services
{
    public interface ICalendarTaskService
    {
        CalendarTaskVM Add(CalendarTaskVM task, string goalId);

        void Delete(string taskId);
    }
}