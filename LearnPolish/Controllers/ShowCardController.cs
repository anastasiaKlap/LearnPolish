using LearnPolish.DAL;
using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnPolish.Controllers
{
    public class ShowCardController : Controller
    {
        // GET: ShowCard
        private LanguageContext db = new LanguageContext();
         
        [HttpPost]
        public ActionResult Index(int id)
        {
            Session["questionN"] = 1;
            var imag = db.Lessons.Find(id).Images.OrderBy(i => i.ID).ToList();
            Session["AllAns"] = imag.Count();

            Lesson lesson = db.Lessons.Find(id);
            List<Image> images  = lesson.Images.OrderBy(i => i.ID).ToList();
            Image image = images[0]; 
            TempData["image"] = image;
            return RedirectToAction("NextCard");

        } 
        public ActionResult NextCard()
        {  

            return View((Image)TempData["image"]);

        }
        [HttpGet]
        public ActionResult FindeNextCard(int id, int IdL)
        {
            Session["questionN"] = Convert.ToInt32(Session["questionN"]) + 1;

            var images = db.Lessons.Find(IdL).Images.OrderBy(i => i.ID).ToList();

            if (id== images.Last().ID)
            {
                return RedirectToAction("LessonsForYou", "Lessons");

            }

            //int qId = (int)aaa.ID + 1;
            int index = images.FindIndex(i => i.ID == id);
            Image image = images[index + 1];

             
            TempData["image"] = image;
            return RedirectToAction("NextCard");

        }
    }
}