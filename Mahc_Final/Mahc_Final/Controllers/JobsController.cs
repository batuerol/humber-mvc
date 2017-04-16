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

namespace Mahc_Final.Controllers
{
    public class JobsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.Job_types);
            return View(jobs.ToList());
        }
        public ActionResult PublicIndex()
        {
            var jobs = db.Jobs.Include(j => j.Job_types);
            return View(jobs.ToList());
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
                return View(jobapp);
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
                    if (file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        ja.application.CV = fileName;
                    }
                    ja.application.Date = DateTime.Now;
                    db.Job_applications.Add(ja.application);
                    db.SaveChanges();
                    if (file.ContentLength > 0)
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


            // ViewBag.Type = new SelectList(db.Job_types, "Id", "Title", jobs.Type);
            return View(ja);
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
                return View(job);
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
            return View();
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
            return View(job);
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
            return View(job);
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
            return View(job);
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
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
