using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Repeat
    {
        [Key]
        public int ID { get; set; }
        public bool ToRepeat { get; set; }
        public Repeat()
        {
            ToRepeat = false;

        }
        public int ProfileID { get; set; }
        public int ImageID { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Image Image { get; set; }
    }
} 