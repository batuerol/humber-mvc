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
    public class ERWaitTimesController: Controller
    {
        private readonly HospitalContext _db = new HospitalContext();

        // GET: ERWaitTimes
        public ActionResult Index()
        {
            return View(_db.ERWaitTimes.ToList());
        }

        // GET: ERWaitTimes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ERWaitTime eRWaitTime = _db.ERWaitTimes.Find(id);
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
        public ActionResult Create([Bind(Include = "ArrivalTime, TreatmentTime")] ERParam newEntry)
        {
            if (ModelState.IsValid)
            {
                _db.ERParams.Add(newEntry);
                _db.SaveChanges();
                CalculateNewERTime();
                return RedirectToAction("Create");
            }

            return View(newEntry);
        }

        public PartialViewResult GetCurrentTime()
        {
            CalculateNewERTime();
            return PartialView("_ERWaitTimes", _db.ERWaitTimes.First(x => x.Lock == "X"));
        }

        private void CalculateNewERTime()
        {
            var lastOneHour = DateTime.Now.AddHours(-1);
            var records = (from p in _db.ERParams where (p.ArrivalTime >= lastOneHour) select p);
            int recordCount = records.Count();
            if (recordCount == 0)
                return;

            int minutes = 0;
            foreach (var erParam in records)
            {
                minutes += (int)((erParam.TreatmentTime - erParam.ArrivalTime).TotalMinutes);
            }
            minutes = minutes / recordCount;

            var erEntity = _db.ERWaitTimes.Single(x => x.Lock == "X");
            erEntity.UpdatedAt = DateTime.Now;
            erEntity.CurrentWaitTime = minutes;
            erEntity.WaitingPatients = records.Count();

            _db.Entry(erEntity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
