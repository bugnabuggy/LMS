using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models;
using Xunit;

namespace LMS.Tests
{
    public class GoalServiceTests
    {
        public GoalServiceTests()
        {
            var goalRepository = new FakeRepository<Goal>();
            goalRepository.Source = new List<Goal>
            {
                //TODO: fill test data
            };
        }


        [Fact]
        public void GetListOfAllGoalsByArea()
        {

        }

        [Fact]
        public void GetListOfNotCompletedGoalsByArea()
        {

        }

        [Fact]
        public void GetListOfLastGoals()
        {

        }

        [Fact]
        public void AddGoal()
        {

        }

        [Fact]
        public void UpdateGoal()
        {

        }

        [Fact]
        public void RemoveGoal()
        {

        }

        [Fact]
        public void GetGoalByIdReturnsCorrectGoal()
        {

        }

        [Fact]
        public void GetNotExistsGoalByIdThrowsException()
        {

        }
    }
}
