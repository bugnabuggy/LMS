using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Services;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

namespace LMS.Controllers
{
    public class ChartsController : Controller
    {
        private IChartService _chartService;

        public ChartsController(IChartService chartService)
        {
            _chartService = chartService;
        }

        public ActionResult Overview()
        {
            var data = _chartService.OverviewChart();
            return Json(data);
        }
    }
}
