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
        private const int _lastGoalLimit = 5;

        private IAppContext _appContext;

        private IRepository<Goal> _goalRepository;

        private IRepository<UserArea> _userAreaRepository;

        private ITimeConverter _timeConverter;

        private IUnitOfWorkFactory _unitOfWorkFactory;

        public GoalService(IAppContext appContext,
            ITimeConverter timeConverter,
            IRepository<Goal> goalRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<UserArea> userAreaRepository)
        {
            _appContext = appContext;
            _goalRepository = goalRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _userAreaRepository = userAreaRepository;
            _timeConverter = timeConverter;
        }

        public List<GoalVM> List(GoalListOptions options)
        {
            CheckUserArea(options.AreaId);

            List<Goal> list = null;

            var query = _goalRepository.Items
                .Where(goal => goal.Area.UserId == _appContext.UserId
                    && goal.AreaId == options.AreaId);

            if (options.OnlyLastGoals)
            {
                var lastGoals = query
                    .Where(goal => goal.StateId == GoalStateType.InProgress)
                    .Select(goal => new
                    {
                        Goal = goal,
                        LastUpdate = (DateTime?)goal.Tasks.Select(t => t.Timestamp).OrderByDescending(t => t).FirstOrDefault()
                    })
                    .Where(g => g.LastUpdate != null)
                    .OrderByDescending(goal => goal.LastUpdate)
                    .Take(_lastGoalLimit)
                    .ToList();

                if (lastGoals.Count > 0)
                {
                    var lastGoalDate = lastGoals.First().LastUpdate;
                    lastGoalDate = _timeConverter.ToLocal(lastGoalDate.Value);
                    var fromDate = _timeConverter.ToUtc(lastGoalDate.Value.Date);
                    var toDate = lastGoalDate;

                    list = lastGoals
                        .Where(goal => goal.LastUpdate >= fromDate && goal.LastUpdate <= toDate)
                        .Select(g => g.Goal)
                        .ToList();
                }
            }
            else
            {
                if (!options.IncludeCompletedGoals)
                {
                    query = query.Where(goal => goal.StateId == GoalStateType.InProgress);
                }
                list = query.ToList();
            }

            return list
                .OrderBy(goal => goal.Priority)
                .Select(Mapper.Map)
                .ToList();
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

            return Mapper.Map(item);
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

            return Mapper.Map(item);
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
                throw new ArgumentException($"Goal with id = {goal.Id} is not exists");
            }

            Mapper.Map(goal, item);

            using (var uof = _unitOfWorkFactory.Create())
            {
                _goalRepository.Update(item);
                uof.SaveChanges();
            }

            return Mapper.Map(item);
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
