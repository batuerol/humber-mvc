using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        //[Authorize(Roles = "Admin, Superuser")]
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
            [Bind(Include = "Id,Title,Content,Excerpt,PostDate,UpdatedAt,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft", "Revision" }, "Publish");
                ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
                return View("Admin/Create", blogPost);
            }

            //blogPost.Excerpt = blogPost.Content.Length > 50 ? blogPost.Content.Substring(0, 50) : blogPost.Content;
            blogPost.Excerpt = ProcessContent(blogPost.Content);
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

        /**
         * Takes content, escapes html and shortens it to max 50 characters.         
         **/
        private static string ProcessContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            string escaped = StripHTML(content);
            string excerpt = escaped.Length > 50 ? escaped.Substring(0, 50) : escaped;
            return excerpt;
        }

        private static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
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
        public ActionResult Edit(HttpPostedFileBase file,
            [Bind(Include = "Id,Title,Content,Slug,PostDate,UpdatedAt,PostStatus,AuthorId")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.UpdatedAt = DateTime.Now;
                // NOTE(batuhan): Retarded EF
                blogPost.PostDate = blogPost.PostDate;
                if (blogPost.Excerpt != _dbEntities.BlogPosts.Single(b => b.Id == blogPost.Id).Excerpt)
                    blogPost.Excerpt = ProcessContent(blogPost.Content);
                    

                if (file != null)
                    imageUploadHandler(file, blogPost);

                _dbEntities.Entry(blogPost).State = EntityState.Modified;
                _dbEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(_dbEntities.HosMembers, "Id", "username", blogPost.AuthorId);
            ViewBag.PostStatus = new SelectList(new List<string> { "Publish", "Draft", "Revision" }, "Publish");
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

        //A\\ 
        /*
         *  Placeholder.png renamed to {postdate}_placeholder.png
         *  
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