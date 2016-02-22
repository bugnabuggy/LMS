using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Areas.Api.ViewModels;
using LMS.Core;
using LMS.Core.Models;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace LMS.Services
{
    public class GoalService : IGoalService
    {
        private IAppContext _appContext;

        private IRepository<Goal> _goalRepository;

        private IRepository<UserArea> _userAreaRepository;

        private ITimeConverter _timeConverter;

        private IUnitOfWorkFactory _unitOfWorkFactory;

        private IRepository<CalendarTask> _taskRepository;

        public GoalService(IAppContext appContext,
            ITimeConverter timeConverter,
            IRepository<Goal> goalRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<UserArea> userAreaRepository,
            IRepository<CalendarTask> taskRepository)
        {
            _appContext = appContext;
            _goalRepository = goalRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _userAreaRepository = userAreaRepository;
            _taskRepository = taskRepository;
            _timeConverter = timeConverter;
        }

        public List<GoalVM> List(GoalListOptions options)
        {
            CheckUserArea(options.AreaId);

            var query = _goalRepository.Items
                .Where(goal => goal.Area.UserId == _appContext.UserId
                    && goal.AreaId == options.AreaId);

            var goalFilterOptions = new GoalFilterOptions
            {
                IncludeCompletedGoals = options.IncludeCompletedGoals,
                OnlyLastGoals = options.OnlyLastGoals
            };

            var list = GoalFilter.Filter(query, goalFilterOptions, _timeConverter, _taskRepository);
            if (list.Count > 0)
            {
                return list[0].Goals
                    .Select(g => Mapper.Map(g, _timeConverter))
                    .ToList();
            }
            return new List<GoalVM>();
        }

        public GoalVM Get(string goalId)
        {
            var item = _goalRepository.Items
                .FirstOrDefault(goal => goal.Area.UserId == _appContext.UserId
                    && goal.Id == goalId);

            if (item == null)
            {
                throw new ItemNotFountException($"Goal with id = {goalId} is not exists");
            }

            var lastTask = _taskRepository.Items
                .FirstOrDefault(t => t.GoalId == goalId);

            if (lastTask != null)
            {
                item.Tasks.Add(lastTask);
            }

            return Mapper.Map(item, _timeConverter);
        }

        public GoalVM Add(GoalVM goal, string areaId)
        {
            CheckUserArea(areaId);

            var item = Mapper.Map(goal);
            item.AreaId = areaId;
            item.StateId = GoalStateType.InProgress;

            using (var uof = _unitOfWorkFactory.Create())
            {
                _goalRepository.Add(item);
                uof.SaveChanges();
            }

            return Mapper.Map(item, _timeConverter);
        }

        private void CheckUserArea(string areaId)
        {
            var userArea = _userAreaRepository.Items
                .FirstOrDefault(a => a.UserId == _appContext.UserId && a.Id == areaId);

            if (userArea == null)
            {
                throw new ItemNotFountException($"User area with id = {areaId} is not exists");
            }
        }

        public GoalVM Update(GoalVM goal)
        {
            var item = _goalRepository.Items
                .FirstOrDefault(g => g.Area.UserId == _appContext.UserId && g.Id == goal.Id);

            if (item == null)
            {
                throw new ItemNotFountException($"Goal with id = {goal.Id} is not exists");
            }

            Mapper.Map(goal, item);

            using (var uof = _unitOfWorkFactory.Create())
            {
                _goalRepository.Update(item);
                uof.SaveChanges();
            }

            return Mapper.Map(item, _timeConverter);
        }

        public void Delete(string goalId)
        {
            var goal = _goalRepository.Items
                .FirstOrDefault(g => g.Area.UserId == _appContext.UserId && g.Id == goalId);

            if (goal != null)
            {
                using (var uof = _unitOfWorkFactory.Create())
                {
                    _goalRepository.Delete(goal);
                    uof.SaveChanges();
                }
            }
        }
    }
}
