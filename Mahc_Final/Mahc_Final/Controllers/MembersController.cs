using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.Models;
using System.IO;
using System.Web.Security;
using Mahc_Final.DBContext;

namespace Mahc_Final.Controllers
{
    [AllowAnonymous]
    public class MembersController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Members
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var members = db.HosMembers.Include(m => m.Role).Where(m => m.role_id> 0).OrderByDescending(m => m.role_id);
            return View(members.ToList());
        }

        // GET: Members/Details/
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            return View(hosMember);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(HosMember user)
        {
            // compared user input with db
            // check if username and password exist
            int count = db.HosMembers.Where(u => u.username == user.username && u.password == user.password).Count();
            if (count == 0)
            {
                ViewBag.Message = "Invalid login";
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(user.username, false);
                return RedirectToAction("Index", "Members"); // go to the index of the Users Controller when logging successfully
            }
        }






        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,first_name,last_name,username,password,phone,email,role_id,pic")] HosMember hosMember,HttpPostedFileBase pic)
        {

            if (ModelState.IsValid)
            {
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName).ToLower();
                    hosMember.pic = fileName.Replace(fileName.Substring(0, fileName.IndexOf(".")), hosMember.first_name);
                    var path = Path.Combine(Server.MapPath("/members/"), hosMember.pic);
                    pic.SaveAs(path);

                }
                db.HosMembers.Add(hosMember);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
           

            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,first_name,last_name,username,password,fname,phone,email,role_id,pic")] HosMember hosMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hosMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.Roles, "id", "rolename", hosMember.role_id);
            return View(hosMember);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosMember hosMember = db.HosMembers.Find(id);
            if (hosMember == null)
            {
                return HttpNotFound();
            }
            return View(hosMember);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HosMember hosMember = db.HosMembers.Find(id);
            db.HosMembers.Remove(hosMember);
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
