using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class SmsInitializer : CreateDatabaseIfNotExists<SmsContext>
    {
        protected override void Seed(SmsContext context)
        {
            /* start of adding of accounts */
            //var userStore = new UserStore<ApplicationUser>(context);\
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var hasher = new PasswordHasher();

            //var userRole = new IdentityRole { Name = "Admin", Id = Guid.NewGuid().ToString() };
            //context.Roles.Add(userRole);
            //if (!roleManager.RoleExists("Admin"))
            //{
            //    var adminUser = new ApplicationUser         // role = admin
            //    {
            //        UserName = "admin@mail.com",
            //        PasswordHash = hasher.HashPassword("Password1!"),
            //        Email = "admin@mail.com",
            //        EmailConfirmed = true,
            //        SecurityStamp = Guid.NewGuid().ToString()
            //    };
            //    adminUser.Roles.Add(new IdentityUserRole { RoleId = userRole.Id, UserId = adminUser.Id });
            //    context.Users.Add(adminUser);
            //}
        }
    }
}