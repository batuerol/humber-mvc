using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using PagedList;

namespace Mahc_Final.Controllers
{
    public class Job_applicationsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Job_applications
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var job_applications = from s in db.Job_applications
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                job_applications = job_applications.Where(s => s.Name.Contains(searchString)
                                       || s.Phone.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    job_applications = job_applications.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    job_applications = job_applications.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    job_applications = job_applications.OrderByDescending(s => s.Date);
                    break;
                default:
                    job_applications = job_applications.OrderBy(s => s.Name);
                    break;
            }
            //var job_application = db.Job_applications.Include(j => j.Job);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", job_applications.ToPagedList(pageNumber, pageSize));
        }

        // GET: Job_applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_applications job_application = db.Job_applications.Find(id);
            if (job_application == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Details", job_application);
        }

        // GET: Job_applications/Create
        public ActionResult Create()
        {
            ViewBag.Job_id = new SelectList(db.Jobs, "Id", "Title");
            return View("Admin/Create");
        }

        // POST: Job_applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Job_id,Name,Email,Phone,CV,Text,Date")] Job_applications job_application)
        {
            if (ModelState.IsValid)
            {
                job_application.Date = DateTime.Now;
                db.Job_applications.Add(job_application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Job_id = new SelectList(db.Jobs, "Id", "Title", job_application.Job_id);
            return View("Admin/Create", job_application);
        }

        // GET: Job_applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_applications job_application = db.Job_applications.Find(id);
            if (job_application == null)
            {
                return HttpNotFound();
            }
            ViewBag.Job_id = new SelectList(db.Jobs, "Id", "Title", job_application.Job_id);
            return View("Admin/Edit", job_application);
        }

        // POST: Job_applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Job_id,Name,Email,Phone,CV,Text,Date")] Job_applications job_application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job_application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Job_id = new SelectList(db.Jobs, "Id", "Title", job_application.Job_id);
            return View("Admin/Edit", job_application);
        }

        // GET: Job_applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_applications job_application = db.Job_applications.Find(id);
            if (job_application == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", job_application);
        }

        // POST: Job_applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job_applications job_application = db.Job_applications.Find(id);
            db.Job_applications.Remove(job_application);

            string fullPath = Request.MapPath("~/CVs/" + job_application.CV);
            System.IO.File.Delete(fullPath);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
