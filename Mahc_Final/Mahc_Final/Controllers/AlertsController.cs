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
using Mahc_Final.Helpers;

namespace Mahc_Final.Controllers
{
    public class AlertsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Alerts
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

            var alerts = from s in db.Alerts
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                alerts = alerts.Where(s => s.Title.Contains(searchString)
                                       || s.Desc.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    alerts = alerts.OrderByDescending(s => s.Title);
                    break;
                case "Published":
                    alerts = alerts.OrderByDescending(s => s.Status);
                    break;
                case "not_published":
                    alerts = alerts.OrderBy(s => s.Status);
                    break;
                case "Created":
                    alerts = alerts.OrderBy(s => s.Date_created);
                    break;
                case "created_desc":
                    alerts = alerts.OrderByDescending(s => s.Date_created);
                    break;
                case "Date":
                    alerts = alerts.OrderBy(s => s.Date_last_modified);
                    break;
                case "date_desc":
                    alerts = alerts.OrderByDescending(s => s.Date_last_modified);
                    break;
                default:
                    alerts = alerts.OrderBy(s => s.Title);
                    break;
            }
            /*
            foreach (Alert alert in alerts)
            {
                alert.Desc = alert.Desc.Substring(0, 100) + "...";
            }*/
            foreach (Alert a in alerts)
            {
                a.Desc = Helpers.HtmlDescriptionHelper.GetShortDescFromHtml(a.Desc);
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", alerts.ToPagedList(pageNumber, pageSize));
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
            return View("Admin/Details", alert);
        }

        // GET: Alerts/Create
        public ActionResult Create()
        {
            return View("Admin/Create");
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Due_time,Desc,Status")] Alert alert)
        {
            try //if you auto build the controllers, visual studio will NOT include a try/catch
            { //a try/catch will try what you want to do, then "catch" what goes wrong. Try/catch can even catch server errors such as if the database server is down
                if (ModelState.IsValid)
                {
                    alert.Created_by = 1;
                    alert.Modified_by = 1;
                    alert.Date_created = DateTime.Now;
                    alert.Date_last_modified = DateTime.Now;
                    db.Alerts.Add(alert);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View("Admin/Create", alert);
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
            return View("Admin/Edit", alert);
        }

        // POST: Alerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Due_time,Desc,Status")] Alert alert)
        {
            try
            {
                Alert currentAlert = db.Alerts.FirstOrDefault(j => j.Id == alert.Id);
                if (ModelState.IsValid)
                {
                    //db.Entry(alert).State = EntityState.Modified;
                    currentAlert.Title = alert.Title;
                    currentAlert.Due_time = alert.Due_time;
                    currentAlert.Desc = alert.Desc;
                    currentAlert.Status = alert.Status;
                    currentAlert.Modified_by = 2;
                    currentAlert.Date_last_modified = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View("Admin/Edit", alert);
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
            return View("Admin/Delete", alert);
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

        //PUBLIC
        public ActionResult PublicIndex()
        {
            var alerts = db.Alerts.OrderBy(s => s.Date_last_modified).Where(j => j.Status == true);
            return View("Public/Index", alerts.ToList());
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
                Alert alert = db.Alerts.Find(id);
                if (alert == null)
                {
                    return HttpNotFound();
                }
                return View("Public/Details", alert);
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
