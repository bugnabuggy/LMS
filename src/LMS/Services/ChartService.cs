using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Core.Models;
using LMS.ViewModels.Charts;

namespace LMS.Services
{
    public class ChartService : IChartService
    {
        private IRepository<CalendarTask> _taskRepository;

        private IRepository<UserArea> _areaRepository;

        private ITimeConverter _timeConverter;

        private IAppContext _appContext;

        public ChartService(IRepository<CalendarTask> taskRepository,
            IRepository<UserArea> areaRepository,
            ITimeConverter timeConverter,
            IAppContext appContext)
        {
            _taskRepository = taskRepository;
            _areaRepository = areaRepository;
            _timeConverter = timeConverter;
            _appContext = appContext;
        }

        public OverviewChartVM OverviewChart()
        {
            var tasks = _taskRepository.Items
                //.Where(t => t.Goal.Area.UserId == _appContext.UserId)
                .Select(t => new
                {
                    Task = t,
                    AreaId = t.Goal.AreaId,
                })
                .ToList();
            tasks.ForEach(t => t.Task.Timestamp = _timeConverter.ToLocal(t.Task.Timestamp));


            var tasksByDate = tasks.GroupBy(g => g.Task.Timestamp.Date).ToList();
            var dateCount = tasksByDate.Count();

            var areaIds = tasks.Select(t => t.AreaId).Distinct().ToList();

            var areasDict = new Dictionary<string, List<float>>(areaIds.Count);
            areaIds.ForEach(a => areasDict.Add(a, new List<float>(dateCount)));

            var dates = new List<DateTime>(dateCount);

            foreach (var dateTasks in tasksByDate)
            {
                var values = dateTasks.GroupBy(t => t.AreaId)
                    .Select(t => new
                    {
                        AreaId = t.Key,
                        Value = t.Any() ? t.Sum(v => v.Task.TimeSpentMin) : 0
                    });

                foreach (var dateValue in values)
                {
                    areasDict[dateValue.AreaId].Add(dateValue.Value);
                }
                dates.Add(dateTasks.Key);
            }

            var areas = _areaRepository.Items
                .Where(a => areaIds.Contains(a.Id))
                .ToList();

            var datasets = areasDict.Select(a =>
                {
                    var userArea = areas.First(area => area.Id == a.Key);
                    return new ChartSeriesVM
                    {
                        data = a.Value,
                        strokeColor = userArea.Color,
                        label = userArea.Title
                    };
                })
                .ToList();

            var labels = dates
                .Select(d => d.DayOfWeek == DayOfWeek.Monday ? d.ToString("M") : string.Empty)
                .ToList();

            return new OverviewChartVM
            {
                labels = labels,
                datasets = datasets
            };
        }
    }
}
