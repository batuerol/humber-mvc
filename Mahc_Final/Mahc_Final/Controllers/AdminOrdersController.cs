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
    public class AdminOrdersController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: AdminOrders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Gift);
            return View(orders.ToList());
        }

        // GET: AdminOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        
        // GET: AdminOrders/Create
        public ActionResult CreateOrder(int ?id)
        {
            
            //ViewBag.gift_id = new SelectList(db.Gifts, "Id", "name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gift gift = db.Gifts.Find(id);
            Order or = new Order();
            or.gift_id = gift.Id;
            //ViewBag.gift_id = gift.Id;

            return View(or);
        }

        // POST: AdminOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "Id,sender_first_name,sender_last_name,sender_phone,sender_email,patient_first_name,patient_last_name,patient_phn_num,gift_id,message")] Order order)
        {
            Gift gift = db.Gifts.Find(order.gift_id);
            if (ModelState.IsValid)
            {
                

                //ViewBag.gift_id = gift.Id;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Pay", "ViewGifts",new { price= gift.price});
            }

            
           
            //var price = gift.price;
           // TempData["price"] = price;
            
            return RedirectToAction("Pay","ViewGifts");
            //return View(order);
        }

        // GET: AdminOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.gift_id = new SelectList(db.Gifts, "Id", "name", order.gift_id);
            return View(order);
        }

        // POST: AdminOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,sender_first_name,sender_last_name,sender_phone,sender_email,patient_first_name,patient_last_name,patient_phn_num,gift_id,message")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gift_id = new SelectList(db.Gifts, "Id", "name", order.gift_id);
            return View(order);
        }

        // GET: AdminOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
