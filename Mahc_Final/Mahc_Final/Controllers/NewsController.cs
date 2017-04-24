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
    public class NewsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: News
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

            var news = from s in db.News
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                news = news.Where(s => s.Title.Contains(searchString)
                                       || s.Content.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    news = news.OrderByDescending(s => s.Title);
                    break;
                case "Published":
                    news = news.OrderByDescending(s => s.Published);
                    break;
                case "not_published":
                    news = news.OrderBy(s => s.Published);
                    break;
                case "Created":
                    news = news.OrderBy(s => s.Date_created);
                    break;
                case "created_desc":
                    news = news.OrderByDescending(s => s.Date_created);
                    break;
                case "Date":
                    news = news.OrderBy(s => s.Date_last_modified);
                    break;
                case "date_desc":
                    news = news.OrderByDescending(s => s.Date_last_modified);
                    break;
                default:
                    news = news.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View("Admin/Index", news.ToPagedList(pageNumber, pageSize));
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
            return View("Admin/Details", news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View("Admin/Create");
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
            return View("Admin/Create", news);
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
            return View("Admin/Edit", news);
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
            return View("Admin/Edit", news);
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
            return View("Admin/Delete", news);
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


        //Public
        public ActionResult PublicIndex()
        {
            var news = db.News.OrderBy(s=> s.Date_last_modified).Where(j => j.Published == true);
            return View("Public/Index", news.ToList());
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
                News article = db.News.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View("Public/Details", article);
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
