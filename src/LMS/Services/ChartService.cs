using System;
using System.Collections.Generic;
using System.Globalization;
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


            var tasksByDate = tasks.GroupBy(g => g.Task.Timestamp.Date).OrderBy(g => g.Key).ToList();
            var fromDate = tasks.Min(t => t.Task.Timestamp.Date);
            var toDate = tasks.Max(t => t.Task.Timestamp.Date);
            var dateCount = (int)(toDate - fromDate).TotalDays + 1;

            var areaIds = tasks.Select(t => t.AreaId).Distinct().ToList();

            var areasDict = new Dictionary<string, List<float>>(areaIds.Count);
            areaIds.ForEach(a => areasDict.Add(a, new List<float>(dateCount)));

            var sumDict = new Dictionary<string, float>(areaIds.Count);
            areaIds.ForEach(a => sumDict.Add(a, 0f));

            var dates = new List<DateTime>(dateCount);

            for (int i = 0; i < dateCount; i++)
            {
                var date = fromDate.AddDays(i);
                var dateTasks = tasksByDate.FirstOrDefault(t => t.Key == date);

                var values = dateTasks?.GroupBy(t => t.AreaId)
                    .Select(t => new
                    {
                        AreaId = t.Key,
                        Value = t.Any() ? t.Sum(v => v.Task.TimeSpentMin) : 0,
                    })
                    .ToList();

                foreach (var areaId in areaIds)
                {
                    var dateValue = values?.FirstOrDefault(t => t.AreaId == areaId);
                    sumDict[areaId] += dateValue?.Value ?? 0;
                    areasDict[areaId].Add(sumDict[areaId]);
                }
                dates.Add(date);
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
                        fillColor = SetColorOpacity(userArea.Color, 0.2f),
                        pointColor = userArea.Color,
                        pointStrokeColor = "#fff",
                        pointHighlightFill = "#fff",
                        pointHighlightStroke = userArea.Color,
                        label = userArea.Title
                    };
                })
                .ToList();

            var labels = dates
                .OrderBy(d => d)
                .Select(d => d.DayOfWeek == DayOfWeek.Monday ? d.ToString("M") : string.Empty)
                .ToList();

            return new OverviewChartVM
            {
                labels = labels,
                datasets = datasets
            };
        }

        private string SetColorOpacity(string color, float opacity)
        {
            var colorSt = color.TrimStart('#');
            int red = int.Parse(colorSt.Substring(0, 2), NumberStyles.HexNumber);
            int green = int.Parse(colorSt.Substring(2, 2), NumberStyles.HexNumber);
            int blue = int.Parse(colorSt.Substring(4, 2), NumberStyles.HexNumber);

            return $"rgba({red}, {green}, {blue}, {opacity}";
        }
    }
}
