using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using PagedList;

namespace Mahc_Final.Controllers
{
    public class VolunteersController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Volunteers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var volunteers = from s in db.Volunteers
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                volunteers = volunteers.Where(s => s.Name.Contains(searchString)
                                       || s.Phone.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Name);
                    break;               
                case "Date":
                    volunteers = volunteers.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    volunteers = volunteers.OrderByDescending(s => s.Date);
                    break;
                default:
                    volunteers = volunteers.OrderBy(s => s.Name);
                    break;
            }
            //var volunteers = db.Volunteers.Include(j => j.Job_types);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", volunteers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Details", volunteer);
        }

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            ViewBag.Task_id = new SelectList(db.Tasks, "Id", "Title");
            return View("Admin/Create");
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Task_id,Name,Email,Phone,Pref_time,Pref_work")] Volunteer volunteer)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    volunteer.Date = DateTime.Now;
                    db.Volunteers.Add(volunteer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }

            ViewBag.Task_id = new SelectList(db.Tasks, "Id", "Title", volunteer.Task_id);
            return View("Admin/Create", volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Task_id = new SelectList(db.Tasks, "Id", "Title", volunteer.Task_id);
            return View("Admin/Edit", volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Task_id,Name,Email,Phone,Pref_time,Pref_work")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.Date = DateTime.Now;
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Task_id = new SelectList(db.Tasks, "Id", "Title", volunteer.Task_id);
            return View("Admin/Edit", volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
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
