using LearnPolish.DAL;
using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnPolish.Controllers
{
    public class SelectExamController : Controller
    {
        private LanguageContext db = new LanguageContext();
        // GET: SelectExam
        public ActionResult Index()
        {
            Exam exam = db.Exams.Where(e => e.IsActive == true).First();
            return View(exam);
        }
    }
}