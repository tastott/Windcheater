using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Windcheater.Web.Controllers
{
    using Services;

    public class HomeController : Controller
    {
        private WindcheaterService _service;

        public HomeController(WindcheaterService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var segment = _service.GetStravaSegmentWithWeather(3752866);
            return View();
        }
    }
}
