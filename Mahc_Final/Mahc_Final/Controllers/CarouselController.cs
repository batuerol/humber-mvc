using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.Helpers;

namespace Mahc_Final.Controllers
{
    public class CarouselController: Controller
    {
        private readonly HospitalContext _dbEntities = new HospitalContext();

        // GET: Carousel
        [Route("Admin/Carousel")]
        public ActionResult Index()
        {
            return View("Admin/Index", _dbEntities.CarouselImages.ToList());
        }

        // GET: Carousel/Details/5
        [Route("Admin/Images/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarouselImage carouselImage = _dbEntities.CarouselImages.Find(id);
            if (carouselImage == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Details", carouselImage);
        }

        // GET: Carousel/Create
        [Route("Admin/NewImage")]
        public ActionResult Create()
        {
            return View("Admin/Create");
        }

        // POST: Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/NewImage")]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "Id,Image,Show")] CarouselImage carouselImage)
        {
            if (file == null)
            {
                ModelState.AddModelError("FileUpload", "Please upload an image.");
            }

            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/CarouselImages/");
                string fileName = FileHelper.GetEpochMs() + "_" + file.FileName;
                file.SaveAs(Path.Combine(path, fileName));
                carouselImage.Image = fileName;

                _dbEntities.CarouselImages.Add(carouselImage);
                _dbEntities.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Admin/Create", carouselImage);
        }

        // GET: Carousel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarouselImage carouselImage = _dbEntities.CarouselImages.Find(id);
            if (carouselImage == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Edit", carouselImage);
        }

        // POST: Carousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "Id,Image,Show")] CarouselImage carouselImage)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string path = Server.MapPath("~/CarouselImages/");
                    if (carouselImage.Image != null && carouselImage.Image != "Placeholder.png")
                        System.IO.File.Delete(Path.Combine(path, carouselImage.Image));

                    string fileName = FileHelper.GetEpochMs() + "_" + file.FileName;
                    file.SaveAs(Path.Combine(path, fileName));
                    carouselImage.Image = fileName;
                }

                _dbEntities.Entry(carouselImage).State = EntityState.Modified;
                _dbEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Admin/Edit", carouselImage);
        }

        // GET: Carousel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarouselImage carouselImage = _dbEntities.CarouselImages.Find(id);
            if (carouselImage == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", carouselImage);
        }

        // POST: Carousel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarouselImage carouselImage = _dbEntities.CarouselImages.Find(id);
            _dbEntities.CarouselImages.Remove(carouselImage);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbEntities.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetImages()
        {
            var images = _dbEntities.CarouselImages.Where(c => c.Show);

            return PartialView("Partials/_Carousel", images);
        }
    }
}
