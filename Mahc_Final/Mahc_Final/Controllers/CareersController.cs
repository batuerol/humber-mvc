using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;

namespace Mahc_Final.Controllers
{
    public class CareersController : Controller
    {
        private HospitalContext db = new HospitalContext();
        // GET: Careers
        public ActionResult Index()
        {
            Careers OppApp= new Careers() ;

            OppApp.opportunities = (from o in db.Jobs
                                    orderby o.Date_last_modified descending
                                    select o).Take(5).ToList();
            OppApp.applications = db.Job_applications.OrderByDescending(a => a.Date).Take(5).ToList();
            return View("Admin/Index",OppApp);
        }
    }
}