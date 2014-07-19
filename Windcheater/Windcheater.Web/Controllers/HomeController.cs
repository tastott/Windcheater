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

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStravaSegmentWithWeather(int id)
        {
            var segment = _service.GetStravaSegmentWithWeather(id);

            return Json(new { success = true, data = segment }, JsonRequestBehavior.AllowGet);
        }
    }
}
