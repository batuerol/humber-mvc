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
    public class EventsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Events
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.StatusSortParm = sortOrder == "Published" ? "not_published" : "Published";
            ViewBag.CreatedSortParm = sortOrder == "Created" ? "created_desc" : "Created";
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

            var events = from s in db.Events
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Title.Contains(searchString)
                                       || s.Desc.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    events = events.OrderByDescending(s => s.Title);
                    break;
                case "Published":
                    events = events.OrderByDescending(s => s.Status);
                    break;
                case "not_published":
                    events = events.OrderBy(s => s.Status);
                    break;
                case "Created":
                    events = events.OrderBy(s => s.Date_created);
                    break;
                case "created_desc":
                    events = events.OrderByDescending(s => s.Date_created);
                    break;
                case "Date":
                    events = events.OrderBy(s => s.Date_last_modified);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.Date_last_modified);
                    break;
                default:
                    events = events.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", events.ToPagedList(pageNumber, pageSize));
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
            return View("Admin/Details", @event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View("Admin/Create");
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Type,Location,Time_start,Time_end,Status,Featured,Volunteers,Desc")] Event @event)
        {
            try //if you auto build the controllers, visual studio will NOT include a try/catch
            { //a try/catch will try what you want to do, then "catch" what goes wrong. Try/catch can even catch server errors such as if the database server is down
                if (ModelState.IsValid)
                {
                    @event.Created_by = 1;
                    @event.Modified_by = 1;
                    @event.Date_created = DateTime.Now;
                    @event.Date_last_modified = DateTime.Now;
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View("Admin/Create", @event);
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
            return View("Admin/Edit", @event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Type,Location,Time_start,Time_end,Status,Featured,Volunteers,Desc")] Event @event)
        {
            try
            {
                Event currentEvent = db.Events.FirstOrDefault(j => j.Id == @event.Id);
                if (ModelState.IsValid)
                {
                    //db.Entry(alert).State = EntityState.Modified;
                    currentEvent.Title = @event.Title;
                    currentEvent.Type = @event.Type;
                    currentEvent.Time_start = @event.Time_start;
                    currentEvent.Time_end = @event.Time_end;
                    currentEvent.Location = @event.Location;
                    currentEvent.Status = @event.Status;
                    currentEvent.Featured = @event.Featured;
                    currentEvent.Volunteers = @event.Volunteers;
                    currentEvent.Desc = @event.Desc;

                    currentEvent.Modified_by = 2;
                    currentEvent.Date_last_modified = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View("Admin/Edit", @event);
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
            return View("Admin/Delete", @event);
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

        // PUBLIC

        public ActionResult PublicIndex()
        {
            var events = db.Events.OrderBy(s => s.Date_last_modified).Where(j => j.Status == true);
            return View("Public/Index", events.ToList());
        }
        public ActionResult PublicDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                //a request will be sent to the database here. 
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return HttpNotFound();
                }
                return View("Public/Details", @event);
            }
            catch (Exception dex) //this catch is finding a server error. 
            {
                ViewBag.Message = "Something went wrong: " + dex.Message;
            }
            return RedirectToAction("PublicIndex"); //if the try was successful, then the return above would execute.
                                                    //this return would execute if catch was needed
        }

    }
}
