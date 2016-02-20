using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var shellPath = PlatformServices.Default.Application.ApplicationBasePath + @"\shell.html";
            //var content = System.IO.File.ReadAllText(shellPath);

            //return Content(content,"text/html");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
