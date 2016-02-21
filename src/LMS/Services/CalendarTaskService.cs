using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Areas.Api.ViewModels;
using LMS.Core;
using LMS.Core.Models;

namespace LMS.Services
{
    public class CalendarTaskService : ICalendarTaskService
    {
        private IRepository<CalendarTask> _taskRepository;

        private IRepository<Goal> _goalRepository;

        private IUnitOfWorkFactory _unitOfWorkFactory;

        private ITimeConverter _timeConverter;

        private IAppContext _appContext;

        public CalendarTaskService(IRepository<CalendarTask> taskRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITimeConverter timeConverter,
            IAppContext appContext,
            IRepository<Goal> goalRepository)
        {
            _taskRepository = taskRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _timeConverter = timeConverter;
            _appContext = appContext;
            _goalRepository = goalRepository;
        }

        public CalendarTaskVM Add(CalendarTaskVM task, string goalId)
        {
            CheckGoal(goalId);

            var item = Mapper.Map(task);
            using (var uof = _unitOfWorkFactory.Create())
            {
                item.GoalId = goalId;
                _taskRepository.Add(item);
            }

            return Mapper.Map(item, _timeConverter);
        }

        public void Delete(string taskId)
        {
            var task = _taskRepository.Items
                .FirstOrDefault(t => t.Goal.Area.UserId == _appContext.UserId && t.Id == taskId);

            if (task != null)
            {
                using (var uof = _unitOfWorkFactory.Create())
                {
                    _taskRepository.Delete(task);
                    uof.SaveChanges();
                }
            }
        }

        private void CheckGoal(string goalId)
        {
            var goalUserId = _goalRepository.Items
                .Where(g => g.Id == goalId)
                .Select(g => g.Area.UserId)
                .FirstOrDefault();

            if (_appContext.UserId != goalUserId)
            {
                throw new ItemNotFountException($"Goal with id = {goalId} is not exists");
            }
        }
    }
}
