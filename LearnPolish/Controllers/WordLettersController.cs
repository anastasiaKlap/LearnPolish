using LearnPolish.DAL;
using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnPolish.Controllers
{
    public class WordLettersController : Controller
    {
        // GET: WordLetters
        private LanguageContext db = new LanguageContext();



        [HttpPost]
        public ActionResult Index(int id)
        {
            Session["correctAns"] = 0;
            Session["questionN"] = 1;
            var imag = db.Lessons.Find(id).Images.OrderBy(i => i.ID).ToList();
            Session["AllAns"] = imag.Count(); 
            Lesson lesson = db.Lessons.Find(id);
            List<Image> images = lesson.Images.OrderBy(i => i.ID).ToList();
            Image image = images[0];
            TempData["image"] = image;
            return RedirectToAction("NextWordLetters", new { IdL = image.LessonID });

        }


        public ActionResult NextWordLetters(int IdL)
        {
            TempData["IdL"] = IdL;
            return View((Image)TempData["image"]);

        }

        [HttpPost]
        public ActionResult FindeNextWordLetters(Image img, string Word)
        {
            int IdL = (int)TempData["IdL"];
            int id = img.ID;
            var images = db.Lessons.Find(IdL).Images.OrderBy(i => i.ID).ToList();
            var min = db.Images.Min(i => i.ID);

            Session["questionN"] = Convert.ToInt32(Session["questionN"]) + 1;

            string w = Word.ToLower().Trim();
            var translation = db.Translations.Where(t => t.ImageID == id).First();
            string x = translation.TranslationToPolish.ToLower();
            if (x == w && id != min)
            {
                Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) + 1;
            }
            else if (x == w && id == min)
            {
                Session["correctAns"] = 1;
            }
            else if (x != w && id == min)
            {
                Session["correctAns"] = 0;
            }

            if (x != w)
            {
                Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
                Repeat repeat = new Repeat();
                Image i = db.Images.Find(id);
                repeat.ProfileID = profile.ID;
                repeat.Image = i;
                if (!db.Repeats.Any(f => f.ImageID == id && f.ProfileID == profile.ID))
                {
                    repeat.ToRepeat = true;
                    db.Repeats.Add(repeat);
                    db.SaveChanges();
                }
            }

            if (id == images.Last().ID)
            {
                Session["gameNumber"] = 1;
                Session["idL"] = IdL;
                return RedirectToAction("Result", "Lessons");

            }

            //int qId = (int)aaa.ID + 1;
            int index = images.FindIndex(i => i.ID == img.ID);
            Image image = images[index + 1];


            TempData["image"] = image;

            return RedirectToAction("NextWordLetters", new { IdL = IdL });


        }
    }
}