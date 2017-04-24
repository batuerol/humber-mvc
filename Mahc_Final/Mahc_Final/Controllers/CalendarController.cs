using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mahc_Final.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View("Admin/Index");
        }

        public ActionResult PublicIndex()
        {
            //RedirectToAction("PublicIndex","News");
            return View("Public/Index");// why??? 
        }
    }
}