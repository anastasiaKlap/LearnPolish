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
    public class RepeatsController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Repeats
        public ActionResult Index()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            int isEmpty = profile.Repeats.Count();
            Session["isEmpty"] = isEmpty; 
            return View(profile.Repeats.ToList());
        }
        public ActionResult DeleteRepeat(int id, int ImageId)
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            Repeat repeat = profile.Repeats.Find(f => f.ID == id);
            Image image = db.Images.Find(ImageId);
            repeat.Image = image;

            db.Repeats.Remove(repeat);
            db.SaveChanges();
            return RedirectToAction("Index", "Repeats");
        }

        public ActionResult DeleteAll()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            var repeats = profile.Repeats.Where(r => r.ProfileID == profile.ID).ToList();

            foreach (var item in repeats)
            {  
                db.Repeats.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Repeats");
        }

        // GET: Repeats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repeat repeat = db.Repeats.Find(id);
            if (repeat == null)
            {
                return HttpNotFound();
            }
            return View(repeat);
        }

        // GET: Repeats/Create
        public ActionResult Create()
        {
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card");
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login");
            return View();
        }

        // POST: Repeats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ToRepeat,ProfileID,ImageID")] Repeat repeat)
        {
            if (ModelState.IsValid)
            {
                db.Repeats.Add(repeat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", repeat.ImageID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", repeat.ProfileID);
            return View(repeat);
        }

        // GET: Repeats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repeat repeat = db.Repeats.Find(id);
            if (repeat == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", repeat.ImageID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", repeat.ProfileID);
            return View(repeat);
        }

        // POST: Repeats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ToRepeat,ProfileID,ImageID")] Repeat repeat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repeat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", repeat.ImageID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "ID", "Login", repeat.ProfileID);
            return View(repeat);
        }

        // GET: Repeats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repeat repeat = db.Repeats.Find(id);
            if (repeat == null)
            {
                return HttpNotFound();
            }
            return View(repeat);
        }

        // POST: Repeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repeat repeat = db.Repeats.Find(id);
            db.Repeats.Remove(repeat);
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
