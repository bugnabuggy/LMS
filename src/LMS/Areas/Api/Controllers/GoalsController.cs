using System;
using System.Collections.Generic;
using LMS.Areas.Api.ViewModels;
using LMS.Services;
using Microsoft.AspNet.Mvc;

namespace LMS.Areas.Api.Controllers
{
    [Route("api")]
    public class GoalsController : AController
    {
        private IGoalService _goalService;

        public GoalsController(IAppContext appContext, IGoalService goalService)
            : base(appContext)
        {
            _goalService = goalService;
        }

        [HttpGet("areas/{areaid}/goals")]
        public IEnumerable<GoalVM> GetList(string areaid, bool includeCompletedGoals = true, bool onlyLastGoals = true)
        {
            if (string.IsNullOrEmpty(areaid))
            {
                throw new ArgumentException("Areaid is not set");
            }
            var options = new GoalListOptions
            {
                AreaId = areaid,
                OnlyLastGoals = onlyLastGoals,
                IncludeCompletedGoals = includeCompletedGoals
            };
            return _goalService.List(options);
        }

        [HttpGet("areas/{areaid}/goals/{id}")]
        [HttpGet("goals/{id}")]
        public GoalVM Get(string id, bool includeGoals = true, bool onlyLastGoals = true)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Goal id is not set");
            }
            return _goalService.Get(id);
        }

        [HttpPost("areas/{areaid}/goals")]
        public GoalVM Post([FromBody]GoalVM goal, string areaid)
        {
            if (string.IsNullOrEmpty(areaid))
            {
                throw new ArgumentException("Areaid id is not set");
            }
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            return _goalService.Add(goal, areaid);
        }

        [HttpPut("areas/{areaid}/goals/{id}")]
        [HttpPut("goals/{id}")]
        public GoalVM Put(string id, [FromBody]GoalVM userArea)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Goal id is not set");
            }
            if (!ModelState.IsValid)
            {
                throw new ArgumentException();
            }
            userArea.Id = id;
            return _goalService.Update(userArea);
        }

        [HttpDelete("areas/{areaid}/goals/{id}")]
        [HttpDelete("goals/{id}")]
        public string Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Goal id is not set");
            }
            _goalService.Delete(id);
            return id;
        }
    }
}

