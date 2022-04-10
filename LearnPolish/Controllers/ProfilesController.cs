using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearnPolish.DAL;
using LearnPolish.Models;
using PagedList;

namespace LearnPolish.Controllers
{
    public class ProfilesController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Profiles
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var profile = from p in db.Profiles
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                profile = profile.Where(p => p.Login.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    profile = profile.OrderByDescending(p => p.Login);
                    break;
               
                default:  // Name ascending 
                    profile = profile.OrderBy(s => s.Login);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(profile.ToPagedList(pageNumber, pageSize)); 
    }
        public ActionResult MyProfil()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Session["user"] = profile.ID; 
            Level level = db.Levels.Single(l => l.ProfileID == profile.ID);
            Session["level"] = level.LevelName;  

            int allPoint = 0;
            var allLesson = db.Lessons.Where(l => l.Module.ModulLevel == level.LevelName).ToList(); ;
            foreach (var item in allLesson)
            {
                int allImage = db.Images.Where(i => i.LessonID == item.ID).Count();
                allPoint += allImage;
            }
            Session["allPoint"] = allPoint;

            int howMachPoint = (int)Math.Ceiling((double)allPoint * 75 / 100);
            Session["howMachPoint"] = howMachPoint;

            UserPoints userPoints = new UserPoints();
            userPoints = db.UserPoints.Where(u => u.ProfileID == profile.ID).First();

            var points = db.UserPoints.Where(u => u.ProfileID == profile.ID).ToList();

            int userPForLetter = 0, userPForListen = 0, userPForSee = 0;
            foreach(var item in points)
            {
                userPForLetter += item.ForLetters;
                userPForListen += item.ForListen;
                userPForSee += item.ForSee;

            }
            Session["SumForLetter"] = userPForLetter;
            Session["howMachForLetter"] = howMachPoint - userPForLetter;

            Session["SumForListen"] = userPForListen;
            Session["howMachForListen"] = howMachPoint - userPForListen;

            Session["SumForSee"] = userPForSee;
            Session["howMachForSee"] = howMachPoint - userPForSee;

            int procentForLetter = (userPForLetter * 100) / allPoint;
            Session["procentForLetter"] = userPoints.ForLetters;

            
            int procentForListen = (userPForListen * 100) / allPoint;
            Session["procentForListen"] = userPoints.ForListen;

            int procentForSee = (userPForSee * 100) / allPoint;
            Session["procentForSee"] = userPoints.ForSee;

            if (procentForLetter >= 75 && procentForListen >= 75 && procentForSee >= 75) 
            {
                Session["nextLevel"] = '1';
            }
            else
            {
                Session["nextLevel"] = null;
            }
            Session["glyphicon"] = null;

            return View(db.Modules.OrderBy(m => m.ModulLevel).ToList());
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Login,Email,FirstName,LastName")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Login,Email,FirstName,LastName")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
