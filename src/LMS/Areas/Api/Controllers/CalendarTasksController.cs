using System;
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
            if (string.IsNullOrEmpty(goalid))
            {
                throw new ArgumentException("Goalid is not set");
            }
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            return _calendarTasksService.Add(task, goalid);
        }

        [HttpDelete("areas/{areaid}/goals/{goalid}/calendartasks/{id}")]
        [HttpDelete("calendartasks/{id}")]
        public string Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Calendar task id is not set");
            }
            _calendarTasksService.Delete(id);
            return id;
        }
    }
}
