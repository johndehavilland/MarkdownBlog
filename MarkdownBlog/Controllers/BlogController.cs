using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarkdownBlog.Models;
using PagedList;
using System.Configuration;

namespace MarkdownBlog.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private BlogPostContext db = new BlogPostContext();

        //
        // GET: /Blog/
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            
            var posts = db.BlogPosts.OrderByDescending(x=>x.CreatedOn).ToList();
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfPosts = posts.ToPagedList(pageNumber, 20);
            GetUserInfo();
            return View(onePageOfPosts);

        }

        private void GetUserInfo()
        {
            string owner = ConfigurationManager.AppSettings["Owner"];
            var userInfo = db.UserProfiles.FirstOrDefault(x => x.UserName == owner);
            ViewBag.UserEmail = userInfo.Email;
            ViewBag.UserTwitter = userInfo.Twitter;
            ViewBag.About = userInfo.Biography;
            ViewBag.OwnerDisplayName= ConfigurationManager.AppSettings["OwnerDisplayName"];
            ViewBag.BlogTitle = ConfigurationManager.AppSettings["BlogTitle"];
        }

        //
        // GET: /Blog/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            GetUserInfo();
            var posts = db.BlogPosts.OrderBy(x => x.CreatedOn).ToList();
            var nextPost = posts.FirstOrDefault(y=>y.BlogPostId > id);

            if (nextPost != null)
            {
                ViewBag.NewerId = nextPost.BlogPostId;
            }

            var oldPosts = db.BlogPosts.OrderByDescending(x => x.CreatedOn).ToList();
            var nextOldPost = oldPosts.FirstOrDefault(y => y.BlogPostId < id);

            if (nextOldPost != null)
            {
                ViewBag.OlderId = nextOldPost.BlogPostId;
            }

            BlogPost blogpost = db.BlogPosts.Find(id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        //
        // GET: /Blog/Create
        [Authorize]
        public ActionResult Create()
        {
            GetUserInfo();
            return View();
        }

        //
        // POST: /Blog/Create

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost blogpost)
        {
            blogpost.CreatedOn = DateTime.Now;
            

            if (ModelState.IsValid)
            {

                db.BlogPosts.Add(blogpost);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=blogpost.BlogPostId });
            }

            return View(blogpost);
        }

        //
        // GET: /Blog/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            GetUserInfo();
            BlogPost blogpost = db.BlogPosts.Find(id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        //
        // POST: /Blog/Edit/5

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogpost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = blogpost.BlogPostId });
            }
            return View(blogpost);
        }

        //
        // GET: /Blog/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            GetUserInfo();
            BlogPost blogpost = db.BlogPosts.Find(id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        //
        // POST: /Blog/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogpost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogpost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult RSSFeed()
        {
            string blogName = ConfigurationManager.AppSettings["BlogName"];
            if(string.IsNullOrWhiteSpace(blogName) == true)
            {
                blogName = "Blog";
            }
            var posts = db.BlogPosts.ToList();
            ViewBag.BlogName = blogName;
            return View("Feed", posts);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            GetUserInfo();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

      
    }
}