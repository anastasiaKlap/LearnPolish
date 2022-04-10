using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearnPolish.DAL;
using LearnPolish.Models;

namespace LearnPolish.Controllers
{
    public class ImagesController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Images
        public ActionResult Index()
        {
            var images = db.Images.Include(i => i.Lesson);
            return View(images.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        [Authorize(Roles = "Admin")]
        // GET: Images/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.Lessons, "ID", "LessonName");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Card,LessonId")] Image image)
        {
            if (ModelState.IsValid)
            { 
                HttpPostedFileBase file = Request.Files["fileOfImage"];

                if (db.Images.Any(i => i.Card == file.FileName))
                {
                    return RedirectToAction("Details", "Lessons", new { id = image.LessonID });
                }
                if (file != null && file.ContentLength > 0)
                {
                    image.Card = file.FileName;
                    string s = HttpContext.Server.MapPath("~/Images/") + image.Card;

                    file.SaveAs(s);
                }
                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Details", "Lessons", new { id = image.LessonID });
            }

            ViewBag.LessonId = new SelectList(db.Lessons, "ID", "LessonName", image.LessonID);
            return View(image);
        }

        public ActionResult List()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return View(db.Images.ToList()); 
        }

        [Authorize(Roles = "Admin")]
        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "ID", "LessonName", image.LessonID);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Card,LessonId")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                HttpPostedFileBase file = Request.Files["fileOfImage"];

                if (file != null && file.ContentLength > 0)
                {
                    image.Card = file.FileName;
                    string s = HttpContext.Server.MapPath("~/Images/") + image.Card;
                }
                else
                {
                    image.Card = db.Images.AsNoTracking().Single(a => a.ID == image.ID).Card;
                }
                db.SaveChanges();
                return RedirectToAction("Details", "Lessons", new { id = image.LessonID });
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "ID", "LessonName", image.LessonID);
            return View(image);
        }

        [Authorize(Roles = "Admin")]
        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Details", "Lessons", new { id = image.LessonID });
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
