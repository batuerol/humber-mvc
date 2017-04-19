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
    public class EventsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(a => a.HosMember).Include(a => a.HosMember1);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name");
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Type,Location,Time_start,Time_end,Status,Featured,Volunteers,Desc,Date_created,Created_by,Date_last_modified,Modified_by")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Modified_by);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Modified_by);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Type,Location,Time_start,Time_end,Status,Featured,Volunteers,Desc,Date_created,Created_by,Date_last_modified,Modified_by")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", @event.Modified_by);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
