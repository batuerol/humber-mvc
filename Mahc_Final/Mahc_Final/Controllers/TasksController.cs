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
    public class TasksController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Tasks
        public ActionResult Index()
        {
            return View("Admin/Index", db.Tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Details", task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View("Admin/Create");
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Type,Time,Regularity,Desc,Status,Contact_person,Contact_phone")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.Created_by = 1;
                task.Modified_by = 1;
                task.Created_date = DateTime.Now;
                task.Modified_date = DateTime.Now;
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Admin/Create", task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Edit", task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Type,Time,Regularity,Desc,Status,Contact_person,Contact_phone")] Task task)
        {
            Task currentTask = db.Tasks.FirstOrDefault(j => j.Id == task.Id);
            if (ModelState.IsValid)
            {
                //db.Entry(task).State = EntityState.Modified;
                currentTask.Title = task.Title;
                currentTask.Type = task.Type;
                currentTask.Time = task.Time;
                currentTask.Regularity = task.Regularity;
                currentTask.Desc = task.Desc;
                currentTask.Status = task.Status;
                currentTask.Contact_person = task.Contact_person;
                currentTask.Contact_phone = task.Contact_phone;
                currentTask.Modified_by = 2;
                currentTask.Modified_date = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Admin/Edit", task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
