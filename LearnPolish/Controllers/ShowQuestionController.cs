using LearnPolish.DAL;
using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnPolish.Controllers
{
    public class ShowQuestionController : Controller
    {
        // GET: ShowQuestion
        private LanguageContext db = new LanguageContext();



        [HttpGet]
        public ActionResult Index(int id)
        {
            Session["questionN"] = 1;
            var quest = db.Exams.Find(id).Questions.OrderBy(q => q.ID).ToList();
            Session["AllAns"] = quest.Count();

            Exam exam = db.Exams.Find(id);
            List<Question> questions = exam.Questions.OrderBy(i => i.ID).ToList();
            Question question = questions[0];
            TempData["question"] = question;
            return RedirectToAction("NextQuestion", new { IdE = question.ExamID });
            }

        
        public ActionResult NextQuestion(int IdE)
        {
            TempData["IdE"] = IdE;
            return View((Question)TempData["question"]);

        }

        [HttpPost]
        public ActionResult FindNextQuestion(Question que)
        {
            int IdE = (int)TempData["IdE"];
            int id = que.ID;
            var quest = db.Exams.Find(IdE).Questions.OrderBy(q => q.ID).ToList();
            var min = db.Questions.Min(i => i.ID);

            Session["questionN"] = Convert.ToInt32(Session["questionN"]) + 1;


            if (que.CorrectAns == que.SelectedValue && id != min)
            {
                Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) + 1;
            }
            else if (que.CorrectAns == que.SelectedValue && id == min)
            {
                Session["correctAns"] = 1;
            }
            else if (que.CorrectAns != que.SelectedValue && id == min)
            {
                Session["correctAns"] = 0;
            }
            

            if (id == quest.Last().ID)
            {
                return RedirectToAction("Create", "Results");

            }

            //int qId = (int)aaa.ID + 1;
            int index = quest.FindIndex(i => i.ID == que.ID);
            Question question = quest[index + 1];


            TempData["question"] = question;

            return RedirectToAction("NextQuestion", new { IdE = IdE });
             
        }
    }
}