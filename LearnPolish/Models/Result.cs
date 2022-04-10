using System;
using System.ComponentModel.DataAnnotations;

namespace LearnPolish.Models
{
    public class Result
    {
        [Key]
        public int ID { get; set; }
        public int ExamID { get; set; }
        public int ProfileID { get; set; }
        public int Total { get; set; }
        public string Grade { get; set; } 

        public virtual Profile Profile { get; set; }
        public virtual Exam Exam { get; set; }

    }
}