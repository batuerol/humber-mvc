using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;
using PagedList;
using Mahc_Final.Helpers;

namespace Mahc_Final.Controllers
{
    public class JobsController : Controller
    {
        private HospitalContext db = new HospitalContext();
        // GET: Jobs
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

            var jobs = from s in db.Jobs
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(s => s.Title.Contains(searchString)
                                       || s.Desc.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    jobs = jobs.OrderByDescending(s => s.Title);
                    break;
                case "Published":
                    jobs = jobs.OrderByDescending(s => s.Status);
                    break;
                case "not_published":
                    jobs = jobs.OrderBy(s => s.Status);
                    break;
                case "Created":
                    jobs = jobs.OrderBy(s => s.Date_created);
                    break;
                case "created_desc":
                    jobs = jobs.OrderByDescending(s => s.Date_created);
                    break;
                case "Date":
                    jobs = jobs.OrderBy(s => s.Date_last_modified);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(s => s.Date_last_modified);
                    break;
                default:
                    jobs = jobs.OrderBy(s => s.Title);
                    break;
            }
            /*foreach (Job job in jobs)
            {
                job.Desc=job.Desc.Substring(0, 100)+"...";
            }*/
            //var jobs = db.Jobs.Include(j => j.Job_types);
            foreach (Job a in jobs)
            {
                a.Desc = Helpers.HtmlDescriptionHelper.GetShortDescFromHtml(a.Desc);
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", jobs.ToPagedList(pageNumber,pageSize));
        }
       

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                //a request will be sent to the database here. 
                Job job = db.Jobs.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                return View("Admin/Details", job);
            }
            catch (Exception dex) //this catch is finding a server error. 
            {
                ViewBag.Message = "Something went wrong: " + dex.Message;
            }
            return RedirectToAction("Index"); //if the try was successful, then the return above would execute.
            //this return would execute if catch was needed
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.Job_types, "Id", "Title");
            return View("Admin/Create");
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Type,Desc,Status")] Job job)
        {
            try //if you auto build the controllers, visual studio will NOT include a try/catch
            { //a try/catch will try what you want to do, then "catch" what goes wrong. Try/catch can even catch server errors such as if the database server is down
                if (ModelState.IsValid)
                {
                    job.Created_by = 1;
                    job.Modified_by = 1;
                    job.Date_created = DateTime.Now;
                    job.Date_last_modified = DateTime.Now;
                    db.Jobs.Add(job);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }


            ViewBag.Type = new SelectList(db.Job_types, "Id", "Title", job.Type);
            return View("Admin/Create", job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.Job_types, "Id", "Title", job.Type);
            return View("Admin/Edit", job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Type,Desc,Status")] Job job)
        {
            Job currentJob = db.Jobs.FirstOrDefault(j => j.Id == job.Id);
            if (ModelState.IsValid)
            {
                //db.Entry(jobs).State = EntityState.Modified;
                currentJob.Title = job.Title;
                currentJob.Type = job.Type;
                currentJob.Desc = job.Desc;
                currentJob.Status = job.Status;
                currentJob.Modified_by = 2;
                currentJob.Date_last_modified = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.Job_types, "Id", "Title", job.Type);
            return View("Admin/Edit", job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            try
            {
                db.Jobs.Remove(job);
                db.SaveChanges();
            }
            catch(Exception dex)
            {
                ViewBag.Message = "Something went wrong..." ;
                Job_applications ja = db.Job_applications.Where(a => a.Job_id == id).First();
                if (ja!=null)
                {
                    ViewBag.Message += " You were trying to delete job which have active applications";
                }
                return View("Admin/Delete", job);
            }
            
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


        //Public
        public ActionResult PublicIndex(int? Job_type)
        {
            var jobs = db.Jobs.Include(j => j.Job_types).Where(j => j.Status == true);
            if (Job_type != null)
            {
                jobs = jobs.Where(j => j.Status == true && j.Type == Job_type);
            }
            ViewBag.Job_Type = new SelectList(db.Job_types, "Id", "Title");
            return View("Public/Index", jobs.ToList());
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
                Job jobs = db.Jobs.Find(id);
                if (jobs == null)
                {
                    return HttpNotFound();
                }
                JobApply jobapp = new JobApply();
                Job_applications apn = new Job_applications();
                apn.Job_id = (int)id;
                jobapp.job = jobs;
                jobapp.application = apn;
                // ViewBag.Job_id = db.Jobs.Where(j => j.Id==id && j.Status == true);
                return View("Public/Details", jobapp);
            }
            catch (Exception dex) //this catch is finding a server error. 
            {
                ViewBag.Message = "Something went wrong: " + dex.Message;
            }
            return RedirectToAction("PublicIndex"); //if the try was successful, then the return above would execute.
                                                    //this return would execute if catch was needed
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Job_id,Name,Email,Phone,CV,Text")]
        public ActionResult PublicDetails(HttpPostedFileBase file, JobApply ja)
        {
            try //if you auto build the controllers, visual studio will NOT include a try/catch
            { //a try/catch will try what you want to do, then "catch" what goes wrong. Try/catch can even catch server errors such as if the database server is down
                if (ModelState.IsValid)
                {
                    var fileName = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        ja.application.CV = fileName;
                    }
                    ja.application.Date = DateTime.Now;
                    db.Job_applications.Add(ja.application);
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/CVs/"), fileName);
                        file.SaveAs(path);
                    }
                    return RedirectToAction("PublicIndex");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }

            ja.job = db.Jobs.Find(ja.application.Job_id);
            // ViewBag.Type = new SelectList(db.Job_types, "Id", "Title", jobs.Type);
            return View("Public/Details", ja);
        }
    }
}
