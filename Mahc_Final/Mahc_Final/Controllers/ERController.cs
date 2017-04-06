using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mahc_Final.Controllers
{
    public class ERController: Controller
    {
        // GET: ER
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GetCurrentWaitTime()
        {
            return null;
        }
    }
}