using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Core.Models;

namespace LMS.Services
{
    public static class GoalFilter
    {
        private const int _lastGoalLimit = 5;

        public static List<GoalsByArea> Filter(IQueryable<Goal> query, 
            GoalFilterOptions options, 
            ITimeConverter timeConverter,
            IRepository<CalendarTask> taskRepository)
        {
            var list = new List<GoalsByArea>();

            if (options.OnlyLastGoals)
            {
                //Group goals by area and take last 5 goals for each area
                var lastGoals = query
                    .Where(goal => goal.StateId == GoalStateType.InProgress)
                    .Select(goal => new
                    {
                        Goal = goal,
                        LastUpdate = (DateTime?)goal.Tasks.Select(t => t.Timestamp).OrderByDescending(t => t).FirstOrDefault()
                    })
                    .ToList()
                    .GroupBy(g => g.Goal.AreaId)
                    .Select(group => new
                    {
                        AreaId = group.Key,
                        Goals = group
                            .OrderByDescending(goal => goal.LastUpdate)
                            .Take(_lastGoalLimit),
                    })
                    .ToList();

                //Leave only goals inside last day
                foreach (var goalsByArea in lastGoals)
                {
                    if (goalsByArea.Goals.Any())
                    {
                        var lastGoalDate = goalsByArea.Goals.First().LastUpdate;
                        lastGoalDate = timeConverter.ToLocal(lastGoalDate.Value);
                        var fromDate = timeConverter.ToUtc(lastGoalDate.Value.Date);
                        var toDate = lastGoalDate;

                        list.Add(new GoalsByArea
                        {
                            Goals = goalsByArea.Goals
                                .Where(goal => goal.LastUpdate >= fromDate && goal.LastUpdate <= toDate)
                                .Select(g => g.Goal)
                                .OrderBy(goal => goal.Priority)
                                .ToList(),
                            AreaId = goalsByArea.AreaId,
                        });
                    }
                }
            }
            else
            {
                if (!options.IncludeCompletedGoals)
                {
                    query = query.Where(goal => goal.StateId == GoalStateType.InProgress);
                }
                list = query
                    .ToList()
                    .GroupBy(g => g.AreaId)
                    .Select(g => new GoalsByArea
                    {
                        AreaId = g.Key,
                        Goals = g.OrderBy(goal => goal.Priority).ToList(),
                    })
                    .ToList();
            }

            FillLastTasks(list, taskRepository, timeConverter);
            return list;
        }

        private static void FillLastTasks(List<GoalsByArea> goals, 
            IRepository<CalendarTask> taskRepository, 
            ITimeConverter timeConverter)
        {
            var goalsIds = goals
                .SelectMany(g => g.Goals)
                .Select(g => g.Id)
                .ToList();

            var tasks = taskRepository.Items
                .Where(t => goalsIds.Contains(t.GoalId))
                .GroupBy(t => t.GoalId)
                .Select(t => t.OrderByDescending(ts => ts.Timestamp).FirstOrDefault())
                .Where(t => t != null)
                .ToList();

            var today = timeConverter.ToLocal(DateTime.UtcNow).Date;

            foreach (var goal in goals.SelectMany(g => g.Goals))
            {
                var task = tasks.FirstOrDefault(t => t.GoalId == goal.Id);
                if (task != null && timeConverter.ToLocal(task.Timestamp) > today)
                {
                    goal.Tasks.Add(task);
                }
            }
        }
    }

    public class GoalsByArea
    {
        public string AreaId { get; set; }

        public List<Goal> Goals { get; set; }
    }

    public class GoalFilterOptions
    {
        public bool OnlyLastGoals { get; set; }

        public bool IncludeCompletedGoals { get; set; }
    }
}
