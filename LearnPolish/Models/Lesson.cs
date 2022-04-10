using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Lesson
    {
        [Key]
        public int ID { get; set; }
        public string LessonName { get; set; }
        public string Photo { get; set; } 
        public int ModuleID { get; set; }
        public virtual Module Module { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<UserPoints> UserPoints { get; set; }
    }
}