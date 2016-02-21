using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Core.Models;

namespace LMS.Infrastructure
{
    public class InitData
    {
        private IRepository<GoalState> _goalStateRepository;
         
        public InitData(IRepository<GoalState> goalStateRepository)
        {
            _goalStateRepository = goalStateRepository;
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            UpdateGoalStates();
        }

        private void UpdateGoalStates()
        {
            throw new NotImplementedException();
        }

        private List<GoalState> GoalStates()
        {
            return new List<GoalState>
            {
                new GoalState {Id = GoalStateType.InProgress, Title = "InProgress"},
                new GoalState {Id = GoalStateType.Completed, Title = "Completed"}
            };
        }
    }
}
