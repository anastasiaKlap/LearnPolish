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
    public class LessonsController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Lessons
        public ActionResult Index()
        { 
            var lessons = db.Lessons.Include(l => l.Module);
            return View(lessons.ToList());
        }

        public ActionResult AllLessons()
        {
            var lessons = db.Lessons.Include(l => l.Module);
            return View(lessons.ToList());
        }
        public ActionResult LessonsForYou()
        { 
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name); 
            var level = db.Levels.Where(l => l.ProfileID == profile.ID).First(); 
            Session["level"] = level.LevelName;
            var lessons = db.Lessons.Where(l => l.Module.ModulLevel == level.LevelName);
             
            int allPoint = 0;
            var allLesson = db.Lessons.Where(l => l.Module.ModulLevel == level.LevelName).ToList(); 
            foreach(var item in allLesson)
            {
                int allImage = db.Images.Where(i => i.LessonID == item.ID).Count();
                allPoint += allImage;
            }

            var points = db.UserPoints.Where(u => u.ProfileID == profile.ID).ToList();
            int userPForLetter = 0, userPForListen = 0, userPForSee = 0;
            foreach (var item in points)
            {
                userPForLetter += item.ForLetters;
                userPForListen += item.ForListen;
                userPForSee += item.ForSee;

            } 
            int procentForLetter = (userPForLetter * 100) / allPoint;
            int procentForListen = (userPForListen * 100) / allPoint;
            int procentForSee = (userPForSee * 100) / allPoint;

            if (procentForLetter >= 75 && procentForListen >= 75 && procentForSee >= 75)
            {
                Session["nextLevel"] = '1';
            }
            else
            {
                Session["nextLevel"] = null;
            }
            return View(lessons.ToList());
        }
        
        public ActionResult Cards(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }
        public ActionResult Result()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            int score = (int)Session["correctAns"];
            int IdL = Convert.ToInt32(Session["idL"]);
            UserPoints userPoints = new UserPoints();
            userPoints = db.UserPoints.Where(u => u.ProfileID == profile.ID && u.LessonID == IdL).First();

            if(Session["gameNumber"].ToString() == "1")
            { 
                userPoints.ForLetters = score;
                
            }
            else if (Session["gameNumber"].ToString() == "2") {
                userPoints.ForListen = score; 
            }
            else
            {
                userPoints.ForSee = score; 
            }
            db.Entry(userPoints).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ShowResult", "Lessons");
        }

        public ActionResult NewLevel()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Level level = new Level();
            level = db.Levels.Where(l => l.ProfileID == profile.ID).First();

            var userP = db.UserPoints.Where(u => u.ProfileID == profile.ID).ToList();

            foreach (var item in userP)
            {
                db.UserPoints.Remove(item);
            }

            db.SaveChanges(); 

            if (level.LevelName == "A1")
            {
                level.LevelName = "A2"; 
            }
            else if(level.LevelName == "A2")
            {
                level.LevelName = "B1"; 
            }
            else if (level.LevelName == "B1")
            {
                level.LevelName = "B2"; 
            }
            else if (level.LevelName == "B2")
            {
            }

            db.Entry(level).State = EntityState.Modified;
            db.SaveChanges();

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

            return RedirectToAction("LessonsForYou", "Lessons");
        }
        public ActionResult ShowResult()
        {

            return View();

        }
        public ActionResult Favorite(int id)
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Favorite favorite = new Favorite();
            Image image = db.Images.Find(id);
            favorite.ProfileID = profile.ID;
            favorite.Image = image;
            if (!db.Favorites.Any(f => f.ImageID == id && f.ProfileID == profile.ID))
            {
                favorite.IsFavorite = true;
                db.Favorites.Add(favorite);
                db.SaveChanges();
                return RedirectToAction("Cards", "Lessons", new { id = image.LessonID });
            }

            return RedirectToAction("Cards", "Lessons", new { id = image.LessonID });
        }

        public ActionResult DeleteFavorite(int id, int ImageId)
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Favorite favorite = profile.Favorites.Find(f => f.ID == id);
            Image image = db.Images.Find(ImageId);
            favorite.Image = image;

            db.Favorites.Remove(favorite);
            db.SaveChanges();
            return RedirectToAction("Favorite", "Favorites"); 
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }
        [Authorize(Roles = "Admin")]
        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "ModulLevel");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LessonName,Photo,ModuleID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["fileOfPhoto"];
                if (file != null && file.ContentLength > 0)
                {
                    lesson.Photo = file.FileName;
                    string s = HttpContext.Server.MapPath("~/LessonImg/") + lesson.Photo;

                    file.SaveAs(s);
                }
                db.Lessons.Add(lesson);
                db.SaveChanges();
                AddNewLessonToUser(lesson.ModuleID, lesson.ID);
                return RedirectToAction("Index");
            } 
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "ModulLevel", lesson.ModuleID);
            return View(lesson);
        }

        public void AddNewLessonToUser(int idM, int idL)
        {
            Module module = db.Modules.Find(idM);
            var level = db.Levels.Where(l => l.LevelName == module.ModulLevel).ToList();

            foreach(var item in level)
            {
                UserPoints user = new UserPoints();
                user.ForLetters = 0;
                user.ForListen = 0;
                user.ForSee = 0;
                user.ProfileID = item.ProfileID;
                user.LessonID = idL;
                db.UserPoints.Add(user);
            }
            db.SaveChanges();
        }
            
        [Authorize(Roles = "Admin")]
        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "ModulLevel", lesson.ModuleID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LessonName,Photo,ModuleID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModuleID = new SelectList(db.Modules, "ID", "ModulLevel", lesson.ModuleID);
            return View(lesson);
        }


        [Authorize(Roles = "Admin")]
        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Game(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
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
