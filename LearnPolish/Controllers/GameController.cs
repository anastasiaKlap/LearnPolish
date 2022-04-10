using LearnPolish.DAL;
using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnPolish.Controllers
{
    public class GameController : Controller
    {
        // GET: Game

        private LanguageContext db = new LanguageContext();
       

        // GET: Translations/Details/5
        public ActionResult Card(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            Image image = lesson.Images.FirstOrDefault(i => i.LessonID == id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

    }
}