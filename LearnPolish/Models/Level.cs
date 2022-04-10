using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Level
    {
        [Key]
        public int ID { get; set; }

        public string LevelName { get; set; }
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }

        // Level = levels[0] 
    }
}