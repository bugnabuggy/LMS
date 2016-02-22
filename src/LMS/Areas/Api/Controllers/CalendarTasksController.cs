using System.Collections.Generic;
using LMS.Areas.Api.ViewModels;
using LMS.Services;
using Microsoft.AspNet.Mvc;

namespace LMS.Areas.Api.Controllers
{
    [Route("api")]
    public class CalendarTasksController : AController
    {
        private ICalendarTaskService _calendarTasksService;

        public CalendarTasksController(IAppContext appContext,
            ICalendarTaskService calendarTasksService)
            : base(appContext)
        {
            _calendarTasksService = calendarTasksService;
        }

        [HttpPost("areas/{areaid}/goals/{goalid}/calendartasks")]
        [HttpPost("goals/{goalid}/calendartasks")]
        public CalendarTaskVM Post([FromBody]CalendarTaskVM task, string goalid)
        {
            return _calendarTasksService.Add(task, goalid);
        }

        [HttpDelete("areas/{areaid}/goals/{goalid}/calendartasks/{id}")]
        [HttpDelete("calendartasks/{id}")]
        public string Delete(string id)
        {
            _calendarTasksService.Delete(id);
            return id;
        }
    }
}
