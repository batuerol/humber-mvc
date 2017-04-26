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
    public class GiftCatsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: GiftCats
        public ActionResult Index()
        {
            return View(db.GiftCats.ToList());
        }

        // GET: GiftCats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiftCat giftCat = db.GiftCats.Find(id);
            if (giftCat == null)
            {
                return HttpNotFound();
            }
            return View(giftCat);
        }

        // GET: GiftCats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GiftCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,cat_name")] GiftCat giftCat)
        {
            if (ModelState.IsValid)
            {
                db.GiftCats.Add(giftCat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(giftCat);
        }

        // GET: GiftCats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiftCat giftCat = db.GiftCats.Find(id);
            if (giftCat == null)
            {
                return HttpNotFound();
            }
            return View(giftCat);
        }

        // POST: GiftCats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,cat_name")] GiftCat giftCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giftCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(giftCat);
        }

        // GET: GiftCats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiftCat giftCat = db.GiftCats.Find(id);
            if (giftCat == null)
            {
                return HttpNotFound();
            }
            return View(giftCat);
        }

        // POST: GiftCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GiftCat giftCat = db.GiftCats.Find(id);
            db.GiftCats.Remove(giftCat);
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
