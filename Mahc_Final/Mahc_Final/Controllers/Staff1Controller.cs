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
using System.IO;

namespace Mahc_Final.Controllers
{
    public class Staff1Controller : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Staff1
        public ActionResult Index()
        {
            var staff1 = db.Staff1.Include(s => s.Department);
            return View(staff1.ToList());
        }

        // GET: Staff1/Details/5
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

        // GET: Staff1/Create
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
        }

        // GET: Staff1/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name", staff1.dept_id);
            return View(staff1);
        }

        // POST: Staff1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dept_id,fname,lname,image,description,phone,email")] Staff1 staff1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dept_id = new SelectList(db.Departments, "Id", "dept_name", staff1.dept_id);
            return View(staff1);
        }

        // GET: Staff1/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Staff1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff1 staff1 = db.Staff1.Find(id);
            db.Staff1.Remove(staff1);
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
