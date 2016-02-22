﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.ViewModels.Charts
{
    public class ChartSeriesVM
    {
        public string label { get; set; }

        public string strokeColor { get; set; }

        public string fillColor { get; set; }

        public string pointColor { get; set; }

        public string pointStrokeColor { get; set; }

        public string pointHighlightFill { get; set; }

        public string pointHighlightStroke { get; set; }

        public List<float> data { get; set; }

        public ChartSeriesVM()
        {
            data = new List<float>();
        }
    }
}
