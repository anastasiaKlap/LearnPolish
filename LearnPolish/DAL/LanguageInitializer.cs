using LearnPolish.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LearnPolish.DAL
{
    public class LanguageInitializer : DropCreateDatabaseIfModelChanges<LanguageContext>
    {

        protected override void Seed(LanguageContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));


            var profils = new List<Profile>
            {
                new Profile { Login ="Anna", Email ="admin@gmail.com", },
                new Profile { Login ="admin@gmail.com", Email ="admin@gmail.com"}
            };
            profils.ForEach(c => context.Profiles.Add(c));
            context.SaveChanges();

            var user = new ApplicationUser { UserName = "jan@gmail.com" };
            string passwor = "Qwert_123";
            IdentityResult u = userManager.Create(user, passwor);
            userManager.AddToRole(user.Id, "Admin");



            var user3 = new ApplicationUser { UserName = "kowalsi@gmail.com" };
            string passwor3 = "Qwert_123";
            IdentityResult u3 = userManager.Create(user3, passwor3);
            userManager.AddToRole(user3.Id, "User");

            context.Profiles.Add(new Profile { Login = "Anna" });
            context.Profiles.Add(new Profile { Login = "kowalsi@gmail.com" });
            context.SaveChanges();




        }
    }
}