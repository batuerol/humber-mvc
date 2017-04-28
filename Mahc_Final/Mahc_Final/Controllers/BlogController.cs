using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.Helpers;
using Mahc_Final.ViewModels;

namespace Mahc_Final.Controllers
{
    public class BlogController: Controller
    {
        private readonly HospitalContext _dbEntities = new HospitalContext();

        // GET: BlogPosts        
        //[Authorize(Roles = "Admin, Superuser")]        
        [Route("Admin/Posts")]
        public ActionResult Index()
        {
            var blogPosts = _dbEntities.BlogPosts.Include(b => b.HosMember);
            return View("Admin/Index", blogPosts.ToList());
        }

        // GET: BlogPosts/Details/5
        //[Authorize(Roles = "Admin, Superuser")]        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", "Home");
            }
            BlogPost blogPost = _dbEntities.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Details", blogPost);
        }

        // GET: BlogPosts/Create
        //[Authorize(Roles = "Admin, Superuser")]        
        [Route("Admin/NewPost")]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "Username");
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft" }, "Publish");
            return View("Admin/Create");
        }

        // POST: BlogPosts/Create
        //[Authorize(Roles = "Admin, Superuser")]        
        [HttpPost]
        [Route("Admin/NewPost")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,
            [Bind(Include = "Id,Title,Content,Excerpt,PostDate,UpdatedAt,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft" }, "Publish");
                ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
                return View("Admin/Create", blogPost);
            }

            //blogPost.Excerpt = HtmlDescriptionHelper.GetShortDescFromHtml(blogPost.Content);
            var temp = HtmlDescriptionHelper.StripHTML(blogPost.Content);
            blogPost.Excerpt = temp.Length > 50 ? temp.Substring(0, 50) : temp;
            blogPost.UpdatedAt = DateTime.Now;
            blogPost.PostDate = DateTime.Now;

            if (file == null)
            {
                blogPost.Slug = "Placeholder.png";
            }
            else
            {
                imageUploadHandler(file, blogPost);
            }

            _dbEntities.BlogPosts.Add(blogPost);
            _dbEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: BlogPosts/Edit/5
        //[Authorize(Roles = "Admin, Superuser")]        
        [Route("Admin/Edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            BlogPost blogPost = _dbEntities.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "Username", blogPost.AuthorId);
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft" }, "Publish");
            return View("Admin/Edit", blogPost);
        }

        // POST: BlogPosts/Edit/5
        //[Authorize(Roles = "Admin, Superuser")]        
        [HttpPost]
        [Route("Admin/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file,
            [Bind(Include = "Id,Title,Content,Slug,Excerpt,PostDate,UpdatedAt,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    imageUploadHandler(file, blogPost);
                }

                if (blogPost.PostStatus == "Publish")
                {
                    BlogPost oldValues = _dbEntities.BlogPosts.AsNoTracking().Single(b => b.Id == blogPost.Id);

                    // Nothing has changed.
                    if (oldValues.Equals(blogPost))
                    {
                        return RedirectToAction("Index");
                    }

                    BlogPost revisionPost = new BlogPost
                    {
                        Title = oldValues.Title,
                        Content = oldValues.Content,
                        Slug = oldValues.Slug,
                        HosMember = oldValues.HosMember,
                        PostDate = oldValues.PostDate,
                        Excerpt = oldValues.Excerpt,
                        ParentPostId = oldValues.Id,
                        UpdatedAt = oldValues.UpdatedAt,
                        PostStatus = "Revision",
                    };
                    // WHAT THE FUCK??
                    var user = new HosMember { Id = oldValues.AuthorId };
                    _dbEntities.HosMembers.Attach(user);
                    revisionPost.HosMember = user;

                    _dbEntities.BlogPosts.Add(revisionPost);
                }

                blogPost.PostDate = blogPost.PostDate;
                blogPost.UpdatedAt = DateTime.Now;
                _dbEntities.Entry(blogPost).State = EntityState.Modified;
                _dbEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft" }, "Publish");
            return View("Admin/Edit", blogPost);
        }

        // GET: BlogPosts/Delete/5
        //[Authorize(Roles = "Admin, Superuser")]        
        [Route("Admin/DeletePost/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = _dbEntities.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View("Admin/Delete", blogPost);
        }

        // POST: BlogPosts/Delete/5
        //[Authorize(Roles = "Admin, Superuser")]        
        [HttpPost, ActionName("Delete")]
        [Route("Admin/DeletePost/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = _dbEntities.BlogPosts.Find(id);
            //blogPost.PostStatus = "Archived";
            //blogPost.UpdatedAt = DateTime.Now;
            //_dbEntities.Entry(blogPost).State = EntityState.Modified;
            if (blogPost.Slug != "Placeholder.png")
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/BlogImages/"), blogPost.Slug));
            }
            _dbEntities.BlogPosts.Remove(blogPost);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Posts")]
        public ActionResult PublicBlog()
        {
            var blogPosts = _dbEntities.BlogPosts
                .Where(b => b.PostStatus == "Publish")
                .OrderByDescending(b => b.UpdatedAt);
            return View("Public/Blog", blogPosts.ToList());
        }

        [HttpGet]
        [Route("Post/{id}")]
        public ActionResult PublicPost(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("PublicBlog");

            var post = _dbEntities.BlogPosts.Single(b => b.Id == id);
            if (post == null)
                return RedirectToAction("PublicBlog");

            return View("Public/Post", post);
        }

        // TODO(batuhan): GetPostsFrom(int startIndex, int finishIndex)
        public PartialViewResult GetLatestPosts(int count)
        {
            var posts = _dbEntities.BlogPosts.Where(b => b.PostStatus == "Publish")
                .OrderByDescending(b => b.UpdatedAt).Include(b => b.HosMember)
                .Take(count);

            return PartialView("Partials/_BlogPosts", posts);
        }

        public PartialViewResult GetPost(int id)
        {
            BlogPost post = _dbEntities.BlogPosts.Single(p => p.Id == id);
            return PartialView("Partials/_BlogPost", post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbEntities.Dispose();
            }
            base.Dispose(disposing);
        }

        //A\\ 
        /*
         *  Placeholder.png renamed to {postdate}_placeholder.png
         *  Slug is used Image path for historical reasons.
         */
        private void imageUploadHandler(HttpPostedFileBase file, BlogPost blogPost)
        {
            string fileName = file.FileName;

            if (fileName == "Placeholder.png")
                fileName = blogPost.PostDate.
                            ToString(CultureInfo.InvariantCulture) + "_"
                           + fileName;

            if (blogPost.Slug != file.FileName)
            {
                string path = Server.MapPath("~/BlogImages/");
                if (blogPost.Slug != null && blogPost.Slug != "Placeholder.png")
                    System.IO.File.Delete(Path.Combine(path, blogPost.Slug));

                file.SaveAs(Path.Combine(path, fileName));
                blogPost.Slug = fileName;
            }
        }
    }
}