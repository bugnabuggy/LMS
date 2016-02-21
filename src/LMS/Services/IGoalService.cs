using System.Collections.Generic;
using LMS.Areas.Api.ViewModels;

namespace LMS.Services
{
    public interface IGoalService
    {
        List<GoalVM> List(GoalListOptions options);
        GoalVM Get(string goalId);
        GoalVM Add(GoalVM goal, string areaId);
        GoalVM Update(GoalVM goal);
        void Delete(string goalId);
    }
}