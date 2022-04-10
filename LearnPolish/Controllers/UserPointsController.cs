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
    public class UserPointsController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: UserPoints
        public ActionResult Index()
        {
            var userPoints = db.UserPoints.Include(u => u.Lesson).Include(u => u.Profile);
            return View(userPoints.ToList());
        }

        // GET: UserPoints/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPoints userPoints = db.UserPoints.Find(id);
            if (userPoints == null)
            {
                return HttpNotFound();
            }
            return View(userPoints);
        }

        // GET: UserPoints/Create
        public ActionResult Create()
        {
            ViewBag.LessonID = new SelectList(db.Lessons, "ID", "LessonName");
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login");
            return View();
        }

        // POST: UserPoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ForLetters,ForListen,ForSee,ProfileID,LessonID")] UserPoints userPoints)
        {
            if (ModelState.IsValid)
            {
                db.UserPoints.Add(userPoints);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonID = new SelectList(db.Lessons, "ID", "LessonName", userPoints.LessonID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", userPoints.ProfileID);
            return View(userPoints);
        }

        // GET: UserPoints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPoints userPoints = db.UserPoints.Find(id);
            if (userPoints == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "ID", "LessonName", userPoints.LessonID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", userPoints.ProfileID);
            return View(userPoints);
        }

        // POST: UserPoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ForLetters,ForListen,ForSee,ProfileID,LessonID")] UserPoints userPoints)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPoints).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "ID", "LessonName", userPoints.LessonID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", userPoints.ProfileID);
            return View(userPoints);
        }

        // GET: UserPoints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPoints userPoints = db.UserPoints.Find(id);
            if (userPoints == null)
            {
                return HttpNotFound();
            }
            return View(userPoints);
        }

        // POST: UserPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPoints userPoints = db.UserPoints.Find(id);
            db.UserPoints.Remove(userPoints);
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
