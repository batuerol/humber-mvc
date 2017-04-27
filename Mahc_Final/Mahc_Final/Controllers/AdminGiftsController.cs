using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Mahc_Final.Models;
using Mahc_Final.DBContext;

namespace Mahc_Final.Controllers
{
    public class AdminGiftsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: AdminGifts
        public ActionResult Index()
        {
            var gifts = db.Gifts.Include(g => g.GiftCat);
            return View(gifts.ToList());
        }

        // GET: AdminGifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            return View(gift);
        }

        // GET: AdminGifts/Create
        public ActionResult Create()
        {
            ViewBag.cat_id = new SelectList(db.GiftCats, "Id", "cat_name");
            return View();
        }

        // POST: AdminGifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,cat_id,name,desc,image,price")] Gift gift,HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName).ToLower();
                    gift.image = fileName.Replace(fileName.Substring(0, fileName.IndexOf(".")), gift.name);
                    var path = Path.Combine(Server.MapPath("/gifts/"), gift.image);
                    file.SaveAs(path);

                }
                db.Gifts.Add(gift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cat_id = new SelectList(db.GiftCats, "Id", "cat_name", gift.cat_id);
            return View(gift);
        }

        // GET: AdminGifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            ViewBag.cat_id = new SelectList(db.GiftCats, "Id", "cat_name", gift.cat_id);
            return View(gift);
        }

        // POST: AdminGifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,cat_id,name,desc,image,price")] Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cat_id = new SelectList(db.GiftCats, "Id", "cat_name", gift.cat_id);
            return View(gift);
        }

        // GET: AdminGifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return HttpNotFound();
            }
            return View(gift);
        }

        // POST: AdminGifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gift gift = db.Gifts.Find(id);
            db.Gifts.Remove(gift);
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
