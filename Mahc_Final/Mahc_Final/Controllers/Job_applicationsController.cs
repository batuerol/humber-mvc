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
    public class Job_applicationsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Job_applications
        public ActionResult Index()
        {
            var job_application = db.Job_applications.Include(j => j.Job);
            return View(job_application.ToList());
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
            return View(job_application);
        }

        // GET: Job_applications/Create
        public ActionResult Create()
        {
            ViewBag.Job_id = new SelectList(db.Jobs, "Id", "Title");
            return View();
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
            return View(job_application);
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
            return View(job_application);
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
            return View(job_application);
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
            return View(job_application);
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
