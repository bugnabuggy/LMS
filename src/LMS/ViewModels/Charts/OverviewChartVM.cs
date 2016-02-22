using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.ViewModels.Charts
{
    public class OverviewChartVM
    {
        public List<string> labels { get; set; }

        public List<ChartSeriesVM> datasets { get; set; }

        public OverviewChartVM()
        {
            labels = new List<string>();
            datasets = new List<ChartSeriesVM>();
        }
    }
}
