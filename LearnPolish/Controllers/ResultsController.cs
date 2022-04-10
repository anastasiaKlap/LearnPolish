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
    public class ResultsController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Results
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Profile).Include(r => r.Exam);
            return View(results.ToList());
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Level level = new Level();
            level.ProfileID = profile.ID;
            Exam exam = db.Exams.Where(e => e.IsActive == true).First();
            var allQuestion = exam.Questions.Count();
            int score = (int)Session["correctAns"];
            int proc = (score * 100) / allQuestion;
            if (proc > 75)
            {
                ViewBag.grade = "B2"; 
                ViewBag.meaning = "Супэр! Маш добры узровень валоднання моваю. Цяпер мусишь тольки закрапиць яго.";
                level.LevelName = "B2";
            }
            else if (proc >= 50)
            {
                ViewBag.grade = "B1"; 
                ViewBag.meaning = "Маш добры узровень валоднання моваю. Але трошки бракуе да самага высокага узровня. Разам з нами дасягнеш яго выконваючы заданнi.";
                level.LevelName = "B1";
            }
            else if (proc >= 25)
            {
                ViewBag.grade = "A2"; 
                ViewBag.meaning = "Маш сяредни узровень валоднання моваю. Разам з нами дасягнеш самага высокага узровуня выконваючы заданнi.";
                level.LevelName = "A2";
            }
            else
            {
                ViewBag.grade = "A1"; 
                ViewBag.meaning = "Твой узровень мовы яшче у самым ппачатку, але разам з нами зможаш яго паднесцi. Дастаткова толькi выконваць заданнi.";
                level.LevelName = "A1";

            }

            var lessons = db.Lessons.Where(l => l.Module.ModulLevel == level.LevelName);

            foreach (var item in lessons)
            {
                UserPoints user = new UserPoints();
                user.ForLetters = 0;
                    user.ForListen = 0;
                    user.ForSee = 0;
                    user.ProfileID = profile.ID;
                    user.LessonID = item.ID;
                    db.UserPoints.Add(user);
            }
            db.SaveChanges();
            db.Levels.Add(level);
            db.SaveChanges();
            Session["grade"] = ViewBag.grade; 
            Session["meaning"] = ViewBag.meaning;  

            ViewBag.profileID = 1;
            ViewBag.examID = 1;
            return RedirectToAction("ShowResult", "Results");
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ID,ProfileID,Total,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                Profile myProfile = db.Profiles.Single(p => p.Login == User.Identity.Name);
                result.Profile = myProfile;

                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("ShowResult", "Results");
            }

            ViewBag.profileID = new SelectList(db.Profiles, "ProfileID", "Login", result.ProfileID);
            ViewBag.exam_id = new SelectList(db.Exams, "ID", "ExamTitle", result.ExamID);
            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.profileID = new SelectList(db.Profiles, "ProfileID", "Login", result.ProfileID);
            ViewBag.exam_id = new SelectList(db.Exams, "ID", "ExamTitle", result.ExamID);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID,Total,Grade")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.profileID = new SelectList(db.Profiles, "ProfileID", "Login", result.ProfileID);
            ViewBag.exam_id = new SelectList(db.Exams, "ID", "ExamTitle", result.ExamID);
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowResult()
        {

            return View();

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
