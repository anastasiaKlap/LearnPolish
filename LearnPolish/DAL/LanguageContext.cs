using LearnPolish.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LearnPolish.DAL
{
    public class LanguageContext : DbContext
    {
        public LanguageContext() : base("DefaultConnection") { } 
        public DbSet<Profile> Profiles { get; set; }  
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Translation> Translations { get; set; } 
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Result> Results { get; set; } 
        public DbSet<Level> Levels { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Repeat> Repeats { get; set; }
        public DbSet<UserPoints> UserPoints { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 

        }

        public System.Data.Entity.DbSet<LearnPolish.Models.Module> Modules { get; set; }
    }
}