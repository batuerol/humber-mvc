using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mahc_Final.DBContext;
using Mahc_Final.ViewModels;

namespace Mahc_Final.Controllers
{
    public class BlogController: Controller
    {
        private readonly HospitalContext _dbEntities = new HospitalContext();

        // GET: BlogPosts
        //[Authorize]
        [Route("Admin/Posts")]
        public ActionResult Index()
        {
            var blogPosts = _dbEntities.BlogPosts.Include(b => b.HosMember);
            return View("Admin/Index", blogPosts.ToList());
        }

        // GET: BlogPosts/Details/5
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
        [Route("Admin/NewPost")]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "Username");
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft", "Revision" }, "Publish");
            return View("Admin/Create");
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Admin/NewPost")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,
            [Bind(Include = "Id,Title,Content,Excerpt,Slug,PostDate,UpdatedAt,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Excerpt = blogPost.Content.Length > 50 ? blogPost.Content.Substring(0, 50) : blogPost.Content;
                blogPost.UpdatedAt = DateTime.Now;
                blogPost.PostDate = DateTime.Now;
                blogPost.Slug = file.FileName;

                _dbEntities.BlogPosts.Add(blogPost);
                _dbEntities.SaveChanges();

                var path = Path.Combine(Server.MapPath("~/BlogImages/"), file.FileName);
                file.SaveAs(path);

                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
            return View("Admin/Create", blogPost);
        }

        // GET: BlogPosts/Edit/5
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
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft", "Revision" }, "Publish");
            return View("Admin/Edit", blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Admin/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Excerpt,Slug,UpdatedAt,PostDate,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.UpdatedAt = DateTime.Now;
                _dbEntities.Entry(blogPost).State = EntityState.Modified;
                _dbEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
            return View("Admin/Edit", blogPost);
        }

        // GET: BlogPosts/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [Route("Admin/DeletePost/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = _dbEntities.BlogPosts.Find(id);
            blogPost.PostStatus = "Archived";
            blogPost.UpdatedAt = DateTime.Now;
            _dbEntities.Entry(blogPost).State = EntityState.Modified;
            //_dbEntities.BlogPosts.Remove(blogPost);
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

            List<BlogPostViewModel> result = new List<BlogPostViewModel>();
            foreach (var item in posts)
            {
                result.Add(new BlogPostViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    Image = item.Slug,
                    Preview = item.Excerpt,
                    UpdatedAt = item.UpdatedAt,
                    Username = item.HosMember.username
                });
            }

            return PartialView("Partials/_BlogPosts", result);
        }

        public PartialViewResult GetPost(int id)
        {
            BlogPost post = _dbEntities.BlogPosts.Single(p => p.Id == id);
            BlogPostViewModel model = new BlogPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Preview = post.Content.Length > 50 ? post.Content.Substring(0, 50) : post.Content,
                Image = post.Slug
            };
            return PartialView("Partials/_BlogPost", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbEntities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}