using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Question
    {
        
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Exam name Required")]

        [Display(Name = "Exam Name ")]
        public int ExamID { get; set; }

        [Required(ErrorMessage = "Question Required")]

        [Display(Name = "Question Title ")]
        public string QuestionTitle { get; set; }
        [Required(ErrorMessage = "Ans 1 Required")]

        [Display(Name = "Ans 1 ")]
        public string Ans1 { get; set; }
        [Required(ErrorMessage = "Ans 2 Required")]

        [Display(Name = "Ans 2 ")]
        public string Ans2 { get; set; }
        [Required(ErrorMessage = "Ans 3 Required")]

        [Display(Name = "Ans 3 ")]
        public string Ans3 { get; set; }
        [Required(ErrorMessage = "Ans 4 Required")]

        [Display(Name = "Ans 4 ")]
        public string Ans4 { get; set; }
        [Required(ErrorMessage = "Correct Answer Required")]

        [Display(Name = "Correct Answer ")]
        public int CorrectAns { get; set; }
        public int SelectedValue { get; set; } 
        public virtual Exam Exam { get; set; }
    }
}