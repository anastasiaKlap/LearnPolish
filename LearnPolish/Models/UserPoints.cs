using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class UserPoints
    {
        [Key]
        public int ID { get; set; } 
        public int ForLetters { get; set; }
        public int ForListen { get; set; }
        public int ForSee { get; set; }

        public int ProfileID { get; set; }
        public int LessonID { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
} 