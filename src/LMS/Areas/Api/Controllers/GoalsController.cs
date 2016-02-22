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
            return _goalService.Get(id);
        }

        [HttpPost("areas/{areaid}/goals")]
        public GoalVM Post([FromBody]GoalVM goal, string areaid)
        {
            return _goalService.Add(goal, areaid);
        }

        [HttpPut("areas/{areaid}/goals/{id}")]
        [HttpPut("goals/{id}")]
        public GoalVM Put(string id, [FromBody]GoalVM userArea)
        {
            userArea.Id = id;
            return _goalService.Update(userArea);
        }

        [HttpDelete("areas/{areaid}/goals/{id}")]
        [HttpDelete("goals/{id}")]
        public string Delete(string id)
        {
            _goalService.Delete(id);
            return id;
        }
    }
}
