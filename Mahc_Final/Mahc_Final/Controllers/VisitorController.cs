using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.Models;
using Mahc_Final.DBContext;
using MAhc_Final.Models;

namespace Mahc_Final.Controllers
{
    public class VisitorController : Controller
    {
        HospitalContext db = new HospitalContext();
        // GET: Visitor
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult ListDepart()
        {
            ViewBag.Id = new SelectList(db.Departments.ToList(), "Id", "dept_name");

            //var dept_list = db.Departments.ToList();
            //SelectList dlist = new SelectList(dept_list, "Id", "dept_name");
            //ViewBag.dept_list = dlist;
            return View();
        }

        [HttpPost]
        public ActionResult ListDepart(Department objSelectedDept)
        {
            int selectedDepartmentId = objSelectedDept.Id;
            return RedirectToAction("display", new { id = selectedDepartmentId });
        }

        public ActionResult display(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Staff1> stf = db.Staff1.Where(d => d.dept_id == id).ToList();

            HospitalStaffM staffm = new HospitalStaffM();
            staffm.staff = stf;

            if (staffm == null)
            {
                return HttpNotFound();
            }
            return View(staffm);


        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff1 staff1 = db.Staff1.Find(id);
            if (staff1 == null)
            {
                return HttpNotFound();
            }
            return View(staff1);
        }
    }
}