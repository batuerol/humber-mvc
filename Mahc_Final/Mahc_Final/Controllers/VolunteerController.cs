using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;

namespace Mahc_Final.Controllers
{
    public class VolunteerController : Controller
    {
        private HospitalContext db = new HospitalContext();
        // GET: Volunteer
        public ActionResult Index()
        {
            Volunteers TskVol = new Volunteers();

            TskVol.tasks= (from o in db.Tasks
                                    orderby o.Modified_date descending
                                    select o).Take(5).ToList();
            TskVol.volunteers = db.Volunteers.OrderByDescending(a => a.Date).Take(5).ToList();
            return View("Admin/Index",TskVol);
        }
    }
}