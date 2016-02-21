using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Core.Models;
using LMS.Services;
using Xunit;
using AppContext = LMS.Services.AppContext;

namespace LMS.Tests
{
    public class GoalServiceTests : AUnitTest
    {
        private GoalService CreateGoalService()
        {
            //Unfortunately, DI libraries (like Ninject) don't support coreclr yet. 
            //So we forced to instantiate the service manually
            return new GoalService(_context,
                new TimeConverter(_context),
                _goalRepository,
                new UnitOfWorkFactoryFake(),
                _userAreaRepository);
        }

        [Fact]
        public void GetListOfAllGoalsByArea()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            var incompleteState = new GoalState { Id = GoalStateType.InProgress, Title = "Incomplete" };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress, Description = "Goal 1", Priority = 2},
                new Goal {AreaId = area1.Id, Area = area1,  StateId = GoalStateType.InProgress, State = incompleteState, Description = "Goal 2", Priority = 1},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.Completed, Description = "Goal 3", Priority = 3},
                new Goal {AreaId = area2.Id, Area = area1, StateId = GoalStateType.Completed, Description = "Goal 4", Priority = 1},
            };

            var service = CreateGoalService();
            var list = service.List(new GoalListOptions { AreaId = area1.Id, IncludeCompletedGoals = true, OnlyLastGoals = false });

            Assert.Equal(3, list.Count);
            Assert.Equal(_goalRepository.Source[1].Description, list[0].Description);
            Assert.Equal(_goalRepository.Source[1].StateId, list[0].StateId);
            Assert.Equal(_goalRepository.Source[1].Id, list[0].Id);
            Assert.Equal(_goalRepository.Source[0].Id, list[1].Id);
            Assert.Equal(_goalRepository.Source[2].Id, list[2].Id);
        }

        [Fact]
        public void RequestAnotherUserAreaThrowsException()
        {
            var user1 = new User();
            var user2 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user2, UserId = user2.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1},
                new Goal {AreaId = area2.Id, Area = area2},
                new Goal {AreaId = area2.Id, Area = area2},
            };

            var service = CreateGoalService();
            Assert.Throws<ItemNotFountException>(() => service.List(new GoalListOptions { AreaId = area2.Id }));
        }

        [Fact]
        public void RequestNotExistsAreaThrowsException()
        {
            var service = CreateGoalService();
            Assert.Throws<ItemNotFountException>(() => service.List(new GoalListOptions { AreaId = "area id" }));
        }

        [Fact]
        public void GetListOfNotCompletedGoalsByArea()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1,  StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.Completed},
            };

            var service = CreateGoalService();
            var list = service.List(new GoalListOptions { AreaId = area1.Id, IncludeCompletedGoals = false, OnlyLastGoals = false });

            Assert.Equal(2, list.Count);
            Assert.True(list.All(a => a.StateId == GoalStateType.InProgress));
        }

        [Fact]
        public void GetListOfLastGoals()
        {
            //We takes the last task. And returns all tasks that checked at the same date.
            var user1 = new User();
            _context.UserId = user1.Id;
            _context.TimeZoneId = "Bangladesh Standard Time";

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1,  StateId = GoalStateType.InProgress},
                new Goal
                {
                    AreaId = area1.Id,
                    Area = area1,
                    StateId = GoalStateType.InProgress,
                    Priority = 1,
                    Tasks = new List<CalendarTask> { new CalendarTask {Timestamp = new DateTime(2016, 02, 21)} }
                },
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.Completed},
                new Goal
                {
                    AreaId = area1.Id,
                    Area = area1,
                    StateId = GoalStateType.InProgress,
                    Priority = 2,
                    Tasks = new List<CalendarTask> { new CalendarTask {Timestamp = new DateTime(2016, 02, 20, 20, 0 ,0)} }
                },
                new Goal
                {
                    AreaId = area1.Id,
                    Area = area1,
                    StateId = GoalStateType.InProgress,
                    Tasks = new List<CalendarTask> { new CalendarTask {Timestamp = new DateTime(2016, 02, 20)} }
                },
            };

            var service = CreateGoalService();
            var list = service.List(new GoalListOptions { AreaId = area1.Id, OnlyLastGoals = true });

            Assert.Equal(2, list.Count);
            Assert.Equal(_goalRepository.Source[2].Id, list[0].Id);
            Assert.Equal(_goalRepository.Source[4].Id, list[1].Id);
        }

        [Fact]
        public void AddGoal()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress}
            };
            var goal = new GoalVM
            {
                Description = "My new goal",
            };

            var service = CreateGoalService();
            var addedGoalModel = service.Add(goal, area1.Id);
            var addedGoal = _goalRepository.Source.Last();

            Assert.Equal(3, _goalRepository.Source.Count);
            Assert.NotNull(addedGoalModel.Id);
            Assert.Equal(GoalStateType.InProgress, addedGoalModel.StateId);
            Assert.Equal(area1.Id, addedGoal.AreaId);
        }

        [Fact]
        public void UpdateGoal()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress}
            };
            var goal = new GoalVM
            {
                Description = "My new goal",
                Id = _goalRepository.Source[0].Id
            };

            var service = CreateGoalService();
            var updatedGoalModel = service.Update(goal);
            var updatedGoal = _goalRepository.Source[0];

            Assert.Equal(goal.Description, updatedGoalModel.Description);
            Assert.Equal(goal.Description, updatedGoal.Description);
        }

        [Fact]
        public void RemoveExistsGoal()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress}
            };

            var service = CreateGoalService();
            var goalId = _goalRepository.Source[0].Id;
            service.Delete(goalId);

            Assert.Equal(1, _goalRepository.Source.Count);
            Assert.NotEqual(goalId, _goalRepository.Source[0].Id);
        }

        [Fact]
        public void RemoveNotExistsGoalDontThrowException()
        {
            var service = CreateGoalService();
            service.Delete("Some goal id");
        }

        [Fact]
        public void RemoveAnotherUserGoalDontRemoveTheGoal()
        {
            var user1 = new User();
            var user2 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user2, UserId = user2.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress}
            };

            var service = CreateGoalService();
            var goalId = _goalRepository.Source[0].Id;
            service.Delete(goalId);

            Assert.Equal(2, _goalRepository.Source.Count);
        }

        [Fact]
        public void GetGoalByIdReturnsCorrectGoal()
        {
            var user1 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress}
            };
            var item = _goalRepository.Source[1];

            var service = CreateGoalService();
            var model = service.Get(item.Id);

            Assert.Equal(item.Id, model.Id);
            Assert.Equal(item.Description, model.Description);
        }

        [Fact]
        public void GetAnotherUserGoalThrowException()
        {
            var user1 = new User();
            var user2 = new User();
            _context.UserId = user1.Id;

            var area1 = new UserArea { User = user1, UserId = user1.Id };
            var area2 = new UserArea { User = user2, UserId = user2.Id };
            _userAreaRepository.Source = new List<UserArea>
            {
                area1,
                area2
            };

            _goalRepository.Source = new List<Goal>
            {
                new Goal {AreaId = area1.Id, Area = area1, StateId = GoalStateType.InProgress},
                new Goal {AreaId = area2.Id, Area = area2, StateId = GoalStateType.InProgress}
            };

            var service = CreateGoalService();
            Assert.Throws<ItemNotFountException>(() => service.Get(_goalRepository.Source[1].Id));
        }

        [Fact]
        public void GetNotExistsGoalByIdThrowsException()
        {
            var service = CreateGoalService();
            Assert.Throws<ItemNotFountException>(() => service.Get("Goal id"));
        }
    }
}
