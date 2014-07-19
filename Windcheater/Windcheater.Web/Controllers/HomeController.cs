using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Windcheater.Web.Services;

namespace Windcheater.Web.Controllers
{
    public class HomeController : Controller
    {
        private StravaService stravaService;

        public HomeController()
        {
            stravaService = new StravaService(ConfigurationManager.AppSettings["StravaApiKey"]);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var segment = stravaService.GetSegment(3752866);
            return View();
        }
    }
}
