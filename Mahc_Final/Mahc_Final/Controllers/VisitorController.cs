using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Stripe;
using System.Threading.Tasks;

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
        }/*
        I dont need it now because instead of posting on some other page 
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


        }*/
        public PartialViewResult _display1(int id)
        {



            List<Staff1> stf = db.Staff1.Where(d => d.dept_id == id).ToList();

            Mahc_Final.DBContext.HospitalStaffM staffm = new Mahc_Final.DBContext.HospitalStaffM();
            staffm.staff = stf;


            return PartialView("_display1", staffm);


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









       /*If visitor is a staff member he has to create an account using the admin panel and add information about him and then admin will assign staff
        * role to this specific staff member to staff directory

        public ActionResult Create()
        {
            ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name");

            //ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name");
            return View();
        }

        // POST: Staff1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dept_id,fname,lname,image,description,phone,email")]Staff1 staff1, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName).ToLower();
                    staff1.image = fileName.Replace(fileName.Substring(0, fileName.IndexOf(".")), staff1.fname);
                    var path = Path.Combine(Server.MapPath("/photos/"), staff1.image);
                    file.SaveAs(path);

                }
                db.Staff1.Add(staff1);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name", staff1.dept_id);
            //ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name");

            return View(staff1);
        }*/
           







    }
}