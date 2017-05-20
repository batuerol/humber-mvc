using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;

namespace Mahc_Final.Controllers
{
    public class contactUsVisitorController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: contactUsVisitor
        public ActionResult Index()
        {
            return View(db.contactInfoes.ToList());
        }
    }
}
