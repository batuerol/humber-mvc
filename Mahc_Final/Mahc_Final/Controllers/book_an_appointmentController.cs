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
    public class book_an_appointmentController : Controller
    {
        private HospitalContext db = new HospitalContext();

        public JsonResult Availabilities(int id)
        {
            var availabilitis = db.DoctorAvailabilities.Where(a => a.dr_id == id && a.status != "Book").Select(a => new { a.Id, a.avail_date, a.time }).ToList();
            return new JsonResult { Data = availabilitis, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Availabilities2(int id)
        {
            DateTime date = Convert.ToDateTime(Request.QueryString["date"]);
            var availabilitisd = db.DoctorAvailabilities.Where(a => a.dr_id == id && a.avail_date == date && a.status != "Book").Select(a => new { a.Id, a.time }).ToList();
            return new JsonResult { Data = availabilitisd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FindDoctor(int id)
        {
            var availabilitis = db.DoctorAvailabilities.Where(a => a.dr_id == id && a.status != "Book").Select(a => new { a.Id, a.avail_date, a.time }).ToList();
            return new JsonResult { Data = availabilitis, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FindDoctor2(int id)
        {
            DateTime date = Convert.ToDateTime(Request.QueryString["date"]);
            var availabilitisd = db.DoctorAvailabilities.Where(a => a.dr_id == id && a.avail_date == date && a.status != "Book").Select(a => new { a.Id, a.time }).ToList();
            return new JsonResult { Data = availabilitisd, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        // GET: book_an_appointment
        public ActionResult Index()
        {
            var book_an_appointment = db.book_an_appointment.Include(b => b.doctor).Include(b => b.DoctorAvailability);
            return View(book_an_appointment.ToList());
        }

        // GET: book_an_appointment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_an_appointment book_an_appointment = db.book_an_appointment.Find(id);
            if (book_an_appointment == null)
            {
                return HttpNotFound();
            }
            return View(book_an_appointment);
        }

        // GET: book_an_appointment/Create
        public ActionResult Create()
        {
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name");
            ViewBag.avail_id = new SelectList(db.DoctorAvailabilities, "Id", "time");
            return View();
        }

        // POST: book_an_appointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dr_id,First_Name,Last_Name,Email_id,contact_no,avail_id,Date ")] book_an_appointment book_an_appointment)
        {
            if (ModelState.IsValid)
            {
                db.book_an_appointment.Add(book_an_appointment);
                db.SaveChanges();

                DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(book_an_appointment.avail_id);
                doctorAvailability.status = "Book";
                db.Entry(doctorAvailability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SuccessMessage");
            }

            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", book_an_appointment.dr_id);
            ViewBag.avail_id = new SelectList(db.DoctorAvailabilities, "Id", "time", book_an_appointment.avail_id);
            return View(book_an_appointment);
        }
        public ActionResult FindDoctorDetails([Bind(Include = "Id,dr_id,First_Name,Last_Name,Email_id,contact_no,avail_id,Date ")] book_an_appointment book_an_appointment)
        {
            if (ModelState.IsValid)
            {
                db.book_an_appointment.Add(book_an_appointment);
                db.SaveChanges();

                DoctorAvailability doctorAvailability = db.DoctorAvailabilities.Find(book_an_appointment.avail_id);
                doctorAvailability.status = "Book";
                db.Entry(doctorAvailability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SuccessMessage");
            }

            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", book_an_appointment.dr_id);
            ViewBag.avail_id = new SelectList(db.DoctorAvailabilities, "Id", "time", book_an_appointment.avail_id);
            return View(book_an_appointment);
        }
        public ActionResult SuccessMessage()
        {
            var book_an_appointment = db.book_an_appointment.Include(b => b.doctor).Include(b => b.DoctorAvailability);
            return View(book_an_appointment.ToList());
        }

        // GET: book_an_appointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_an_appointment book_an_appointment = db.book_an_appointment.Find(id);
            if (book_an_appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", book_an_appointment.dr_id);
            ViewBag.avail_id = new SelectList(db.DoctorAvailabilities, "Id", "time", book_an_appointment.avail_id);
            return View(book_an_appointment);
        }

        // POST: book_an_appointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dr_id,First_Name,Last_Name,Email_id,contact_no,avail_id,Date")] book_an_appointment book_an_appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book_an_appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dr_id = new SelectList(db.doctors, "Id", "Name", book_an_appointment.dr_id);
            ViewBag.avail_id = new SelectList(db.DoctorAvailabilities, "Id", "time", book_an_appointment.avail_id);
            return View(book_an_appointment);
        }

        // GET: book_an_appointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book_an_appointment book_an_appointment = db.book_an_appointment.Find(id);
            if (book_an_appointment == null)
            {
                return HttpNotFound();
            }
            return View(book_an_appointment);
        }

        // POST: book_an_appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            book_an_appointment book_an_appointment = db.book_an_appointment.Find(id);
            db.book_an_appointment.Remove(book_an_appointment);
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
