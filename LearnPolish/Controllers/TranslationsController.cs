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
    public class TranslationsController : Controller
    {
        private LanguageContext db = new LanguageContext();

        // GET: Translations
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var translations = db.Translations.Include(t => t.Image);
            return View(translations.ToList());
        }

        // GET: Translations/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            return View(translation);
        }

        [Authorize(Roles = "Admin")]
        // GET: Translations/Create
        public ActionResult Create()
        {
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card"); 
            return View();
        }

        // POST: Translations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Word,TranslationToPolish,SwitchedСharacter,ImageID,LessonID")] Translation translation)
        {
            if (ModelState.IsValid)
            {
                if (db.Translations.Any(t => t.Word == translation.Word || t.TranslationToPolish == translation.TranslationToPolish))
                {
                    return RedirectToAction("Index");
                }

                Image image = db.Images.Single(i => i.ID == translation.ImageID);
                translation.LessonID = image.LessonID;
                translation.Word = translation.Word.Replace(" ", string.Empty);
                translation.TranslationToPolish = translation.TranslationToPolish.Replace(" ", string.Empty).ToLower();
                translation.SwitchedСharacter = Shuffle(translation.TranslationToPolish);
                db.Translations.Add(translation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", translation.ImageID); 
            return View(translation);
        }

        // GET: Translations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", translation.ImageID); 
            return View(translation);
        }

        // POST: Translations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Word,TranslationToPolish,SwitchedСharacter,ImageID,LessonID")] Translation translation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(translation).State = EntityState.Modified;
                translation.Word = translation.Word.Replace(" ", string.Empty);
                translation.TranslationToPolish = translation.TranslationToPolish.Replace(" ", string.Empty).ToLower();
                translation.SwitchedСharacter = Shuffle(translation.TranslationToPolish);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageID = new SelectList(db.Images, "ID", "Card", translation.ImageID); 
            return View(translation);
        }

        // GET: Translations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            return View(translation);
        }

        // POST: Translations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Translation translation = db.Translations.Find(id);
            db.Translations.Remove(translation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static string Shuffle(string normWord)
        {
            char[] array = normWord.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
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