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
    public class AlertsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Alerts
        public ActionResult Index()
        {
            var alerts = db.Alerts.Include(a => a.HosMember).Include(a => a.HosMember1);
            return View(alerts.ToList());
        }

        // GET: Alerts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = db.Alerts.Find(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
            return View(alert);
        }

        // GET: Alerts/Create
        public ActionResult Create()
        {
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name");
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name");
            return View();
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Due_time,Desc,Status,Date_created,Created_by,Date_last_modified,Modified_by")] Alert alert)
        {
            if (ModelState.IsValid)
            {
                db.Alerts.Add(alert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Modified_by);
            return View(alert);
        }

        // GET: Alerts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = db.Alerts.Find(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Modified_by);
            return View(alert);
        }

        // POST: Alerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Due_time,Desc,Status,Date_created,Created_by,Date_last_modified,Modified_by")] Alert alert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", alert.Modified_by);
            return View(alert);
        }

        // GET: Alerts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = db.Alerts.Find(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alert alert = db.Alerts.Find(id);
            db.Alerts.Remove(alert);
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
