using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Core;
using LMS.Core.Models;

namespace LMS.Services
{
    public class UserAreaService : IUserAreaService
    {
        private IRepository<UserArea> _userAreaRepository;

        private IRepository<Goal> _goalRepository;

        private ITimeConverter _timeConverter;

        private IUnitOfWorkFactory _unitOfWorkFactory;

        private IAppContext _appContext;

        private IRepository<CalendarTask> _taskRepository;

        public UserAreaService(IRepository<UserArea> userAreaRepository,
            IRepository<Goal> goalRepository,
            ITimeConverter timeConverter,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAppContext appContext,
            IRepository<CalendarTask> taskRepository)
        {
            _userAreaRepository = userAreaRepository;
            _goalRepository = goalRepository;
            _timeConverter = timeConverter;
            _unitOfWorkFactory = unitOfWorkFactory;
            _appContext = appContext;
            _taskRepository = taskRepository;
        }

        public List<UserAreaVM> List(AreaListOptions options)
        {
            var areas = _userAreaRepository.Items
                .Where(a => a.UserId == _appContext.UserId)
                .ToList();

            if (options.IncludeGoals)
            {
                var areasIds = areas.Select(a => a.Id).ToList();
                var goalsQuery = _goalRepository.Items
                    .Where(g => g.Area.UserId == _appContext.UserId
                                && areasIds.Contains(g.AreaId));
                var goalFilterOptions = new GoalFilterOptions
                {
                    IncludeCompletedGoals = options.IncludeCompletedGoals,
                    OnlyLastGoals = options.OnlyLastGoals
                };
                var goals = GoalFilter.Filter(goalsQuery, goalFilterOptions, _timeConverter, _taskRepository);

                areas.ForEach(area => area.Goals = goals.FirstOrDefault(g => g.AreaId == area.Id)?.Goals ?? new List<Goal>());
            }

            return areas
                .OrderBy(a => a.Priority)
                .Select(g => Mapper.Map(g, _timeConverter))
                .ToList();
        }

        public UserAreaVM Get(string areaId, AreaListOptions options)
        {
            var item = _userAreaRepository.Items
                .FirstOrDefault(a => a.UserId == _appContext.UserId && a.Id == areaId);

            if (item == null)
            {
                throw new ItemNotFountException($"Area with id = {areaId} is not exists");
            }

            if (options.IncludeGoals)
            {
                var goalsQuery = _goalRepository.Items
                    .Where(g => g.Area.UserId == _appContext.UserId
                                && g.AreaId == areaId);
                var goalFilterOptions = new GoalFilterOptions
                {
                    IncludeCompletedGoals = options.IncludeCompletedGoals,
                    OnlyLastGoals = options.OnlyLastGoals
                };
                var goals = GoalFilter.Filter(goalsQuery, goalFilterOptions, _timeConverter, _taskRepository);

                if (goals.Count > 0)
                {
                    item.Goals = goals[0].Goals;
                }
            }

            return Mapper.Map(item, _timeConverter);
        }

        public UserAreaVM Add(UserAreaVM userArea)
        {
            var item = Mapper.Map(userArea);
            item.UserId = _appContext.UserId;

            using (var uof = _unitOfWorkFactory.Create())
            {
                _userAreaRepository.Add(item);
                uof.SaveChanges();
            }

            return Mapper.Map(item, _timeConverter);
        }

        public UserAreaVM Update(UserAreaVM userArea)
        {
            var item = _userAreaRepository.Items
                .FirstOrDefault(a => a.UserId == _appContext.UserId && a.Id == userArea.Id);

            if (item == null)
            {
                throw new ItemNotFountException($"Goal with id = {userArea.Id} is not exists");
            }

            Mapper.Map(userArea, item);

            using (var uof = _unitOfWorkFactory.Create())
            {
                _userAreaRepository.Update(item);
                uof.SaveChanges();
            }

            return Mapper.Map(item, _timeConverter);
        }

        public void Delete(string areaId)
        {
            var goal = _userAreaRepository.Items
                .FirstOrDefault(g => g.UserId == _appContext.UserId && g.Id == areaId);

            if (goal != null)
            {
                using (var uof = _unitOfWorkFactory.Create())
                {
                    _userAreaRepository.Delete(goal);
                    uof.SaveChanges();
                }
            }
        }
    }
}
