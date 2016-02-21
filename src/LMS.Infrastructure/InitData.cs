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

        private IUnitOfWorkFactory _unitOfWorkFactory;

        public InitData(IRepository<GoalState> goalStateRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _goalStateRepository = goalStateRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            try
            {
                UpdateGoalStates();
            }
            catch
            {
                //Do nothing
            }
        }

        private void UpdateGoalStates()
        {
            var states = _goalStateRepository.Items.ToList();
            var stateList = GoalStates();
            var newStates = stateList
                .Where(s => states.All(st => st.Id != s.Id))
                .ToList();
            if (newStates.Count > 0)
            {
                using (var uof = _unitOfWorkFactory.Create())
                {
                    newStates.ForEach(s => _goalStateRepository.Add(s));
                    uof.SaveChanges();
                }
            }
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
