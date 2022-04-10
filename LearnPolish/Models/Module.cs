using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Module
    {
        [Key]
        public int ID { get; set; }

        public string ModulLevel { get; set; } 
        public virtual List<Lesson> Lessons { get; set; }

         
    }
}