using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        public string Card { get; set; }
        public int LessonID { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual List<Translation> Translations { get; set; }
        public virtual List<Favorite> Favorites { get; set; }
        public virtual List<Repeat> Repeats { get; set; }
    }
}