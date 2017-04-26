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
    public class DoctorAvailabilitiesController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: DoctorAvailabilities
        public ActionResult Index()
        {
            var doctorAvailabilities = db.DoctorAvailabilities.Include(d => d.doctor);
            return View(doctorAvailabilities.ToList());
        }

        // GET: DoctorAvailabilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(id);
            if (doctorAvailability == null)
            {
                return HttpNotFound();
            }
            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Create
        public ActionResult Create()
        {
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name");
            return View();
        }

        // POST: DoctorAvailabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dr_id,avail_date,time,status")] DoctorAvailability doctorAvailability)
        {
            if (ModelState.IsValid)
            {
                db.DoctorAvailabilities.Add(doctorAvailability);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", doctorAvailability.dr_id);
            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(id);
            if (doctorAvailability == null)
            {
                return HttpNotFound();
            }
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", doctorAvailability.dr_id);
            return View(doctorAvailability);
        }

        // POST: DoctorAvailabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dr_id,avail_date,time,status")] DoctorAvailability doctorAvailability)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorAvailability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", doctorAvailability.dr_id);
            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(id);
            if (doctorAvailability == null)
            {
                return HttpNotFound();
            }
            return View(doctorAvailability);
        }

        // POST: DoctorAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(id);
            db.DoctorAvailabilities.Remove(doctorAvailability);
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
