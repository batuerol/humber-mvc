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
    public class contactusAdminController : Controller
    {
        private HospitalContext db = new HospitalContext();


        // GET: contactusAdmin
        public ActionResult Index()
        {
            return View(db.contactus.ToList());
        }

        // GET: contactusAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contactu contactu = db.contactus.Find(id);
            if (contactu == null)
            {
                return HttpNotFound();
            }
            return View(contactu);
        }

        // GET: contactusAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contactu contactu = db.contactus.Find(id);
            if (contactu == null)
            {
                return HttpNotFound();
            }
            return View(contactu);
        }

        // POST: contactusAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VisitorEmail,VisitorName,Date,Question,Reply")] contactu contactu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactu).State = EntityState.Modified;
                db.SaveChanges();

                //if SMTP was setup, it will send a mail 
                //MailMessage mail = new MailMessage();
                // mail.To.Add(contactu.VisitorEmail);
                //mail.From = new MailAddress("muskokahospital@muskokahospital.ca");
                //mail.Subject = "Muskoka Hospital Reply to your Question";
                //string Body = contactu.Reply;
                //mail.Body = Body;
                //mail.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential
                //("username", "password");// Sender's usrname and password goes here
                //smtp.EnableSsl = true;
                //smtp.Send(mail);

                return RedirectToAction("Index");
            }
            return View(contactu);
        }

        // GET: contactusAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contactu contactu = db.contactus.Find(id);
            if (contactu == null)
            {
                return HttpNotFound();
            }
            return View(contactu);
        }

        // POST: contactusAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contactu contactu = db.contactus.Find(id);
            db.contactus.Remove(contactu);
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
