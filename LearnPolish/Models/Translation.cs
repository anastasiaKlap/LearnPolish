using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Translation
    {
        [Key]
        public int ID { get; set; }
        public string Word { get; set; }
        public string TranslationToPolish { get; set; }
        public string SwitchedСharacter { get; set; }
        public int ImageID { get; set; }
        public int LessonID { get; set; }
        public virtual Image Image { get; set; }
        //public virtual Lesson Lesson { get; set; }

    }
}