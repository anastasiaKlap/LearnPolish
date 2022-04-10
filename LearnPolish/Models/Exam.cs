using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Exam
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Exam Name Required")] 

        [Display(Name = "Exam Name ")]
        public string ExamTitle { get; set; }
        public bool IsActive { get; set; }
        public Exam()
        {
            IsActive = false;

        }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}