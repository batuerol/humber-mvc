﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Newtonsoft.Json.Linq;

namespace Mahc_Final.Controllers
{
    public class contactusVisitorController : Controller
    {
        private HospitalContext db = new HospitalContext();
       
        // GET: contactusVisitor/Create
        public ActionResult Create(string thankmessage)
        {
            if (thankmessage != null)
                ViewBag.ThankMessage = thankmessage;
            return View();
        }

        // POST: contactusVisitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VisitorEmail,VisitorName,Date,Question,Reply")] contactu contactu)
        {
            //VALIDATE RECAPTCHA
            var response = Request["g-recaptcha-response"];
            string secretkey = "6LekzBwUAAAAAH0XnUAoqDySSfez6NSjD5shdiAm";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretkey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Successful" : "Please Confirm, You are not a Robot.";

            if (ModelState.IsValid && status)
            {
                contactu.Date = DateTime.Now;
                db.contactus.Add(contactu);
                db.SaveChanges();
                return RedirectToAction("Create", new { thankmessage = "Thank you for your question! We will get back to you as soon as possible." });
            }

            return View(contactu);
        }
    }
}
