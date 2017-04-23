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
    public class ERWaitTimesController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: ERWaitTimes
        public ActionResult Index()
        {
            return View(db.ERWaitTimes.ToList());
        }

        // GET: ERWaitTimes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ERWaitTime eRWaitTime = db.ERWaitTimes.Find(id);
            if (eRWaitTime == null)
            {
                return HttpNotFound();
            }
            return View(eRWaitTime);
        }

        // GET: ERWaitTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ERWaitTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lock,CurrentWaitTime,UpdatedAt,WaitingPatients")] ERWaitTime eRWaitTime)
        {
            if (ModelState.IsValid)
            {
                db.ERWaitTimes.Add(eRWaitTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eRWaitTime);
        }

        // GET: ERWaitTimes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ERWaitTime eRWaitTime = db.ERWaitTimes.Find(id);
            if (eRWaitTime == null)
            {
                return HttpNotFound();
            }
            return View(eRWaitTime);
        }

        // POST: ERWaitTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lock,CurrentWaitTime,UpdatedAt,WaitingPatients")] ERWaitTime eRWaitTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eRWaitTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eRWaitTime);
        }

        // GET: ERWaitTimes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ERWaitTime eRWaitTime = db.ERWaitTimes.Find(id);
            if (eRWaitTime == null)
            {
                return HttpNotFound();
            }
            return View(eRWaitTime);
        }

        // POST: ERWaitTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ERWaitTime eRWaitTime = db.ERWaitTimes.Find(id);
            db.ERWaitTimes.Remove(eRWaitTime);
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
