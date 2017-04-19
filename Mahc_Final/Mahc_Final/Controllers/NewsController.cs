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
    public class NewsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: News
        public ActionResult Index()
        {
            var news = db.News;
            return View(news.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Published,Featured")] News news)
        {            
            try //if you auto build the controllers, visual studio will NOT include a try/catch
            { //a try/catch will try what you want to do, then "catch" what goes wrong. Try/catch can even catch server errors such as if the database server is down
                if (ModelState.IsValid)
                {
                    news.Created_by = 1;
                    news.Modified_by = 1;
                    news.Date_created = DateTime.Now;
                    news.Date_last_modified = DateTime.Now;
                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.Created_by = new SelectList(db.HosMembers, "Id", "first_name", news.Created_by);
            ViewBag.Modified_by = new SelectList(db.HosMembers, "Id", "first_name", news.Modified_by);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Published,Featured")] News news)
        {
            try
            {
                News currentNews = db.News.FirstOrDefault(j => j.Id == news.Id);
                if (ModelState.IsValid)
                {
                    //db.Entry(alert).State = EntityState.Modified;
                    currentNews.Title = news.Title;
                    currentNews.Content = news.Content;
                    currentNews.Published = news.Published;
                    currentNews.Featured = news.Featured;
                    currentNews.Modified_by = 2;
                    currentNews.Date_last_modified = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) //you can create an Exception/DataException object here and set it to a variable. I've called it dex here. 
            {
                ViewBag.Message = "Whoops! Something went wrong. Here's what went wrong: " + dex.Message; //One of the properties of these objects is Message which is a string of what went wrong. 
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
