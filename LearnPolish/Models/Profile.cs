using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnPolish.Models
{
    public class Profile
    {
        [Key]
        public int ID { get; set; }

        [StringLength(30, ErrorMessage = "Nick nie może być wiekszy niż 40 znakow.")]
        public string Login { get; set; }

        [DataType(DataType.EmailAddress)]
        public String Email { get; set; } 
        public virtual List<Level> Levels { get; set; }
        public virtual List<Result> Results { get; set; }
        public virtual List<Favorite> Favorites { get; set; }
        public virtual List<Repeat> Repeats { get; set; }
        public virtual List<UserPoints> UserPoints { get; set; }
    }
}