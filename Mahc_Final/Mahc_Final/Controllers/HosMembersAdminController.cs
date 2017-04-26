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

namespace Mahc_Final.Controllers
{
    public class HosMembersAdminController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: HosMembersAdmin
        public ActionResult Index()
        {
            var hosMembers = db.HosMembers.Include(h => h.Role);
            return View(hosMembers.ToList());
        }

        // GET: HosMembersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            return View(hosMember);
        }

        // GET: HosMembersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename");
            return View();
        }

        // POST: HosMembersAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,first_name,last_name,username,password,fname,phone,email,role_id,pic")] HosMember hosMember)
        {
            if (ModelState.IsValid)
            {
                db.HosMembers.Add(hosMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // GET: HosMembersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // POST: HosMembersAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,first_name,last_name,username,password,fname,phone,email,role_id,pic")] HosMember hosMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hosMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // GET: HosMembersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            return View(hosMember);
        }

        // POST: HosMembersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HosMember hosMember = db.HosMembers.Find(id);
            db.HosMembers.Remove(hosMember);
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
