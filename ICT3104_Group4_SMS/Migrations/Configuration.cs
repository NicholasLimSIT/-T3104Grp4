namespace ICT3104_Group4_SMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ICT3104_Group4_SMS.DAL.SmsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ICT3104_Group4_SMS.DAL.SmsContext context)
        {
            /* start of adding of accounts */
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var hasher = new PasswordHasher();

            ////var adminRole = new IdentityRole { Name = "Admin", Id = Guid.NewGuid().ToString() };
            //var studentRole = new IdentityRole { Name = "Student", Id = Guid.NewGuid().ToString() };
            //var hodRole = new IdentityRole { Name = "HOD", Id = Guid.NewGuid().ToString() };
            //var lecturerRole = new IdentityRole { Name = "Lecturer", Id = Guid.NewGuid().ToString() };
            ////context.Roles.Add(adminRole);
            //context.Roles.Add(studentRole);
            //context.Roles.Add(hodRole);
            //context.Roles.Add(lecturerRole);
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
            //    adminUser.Roles.Add(new IdentityUserRole { RoleId = adminRole.Id, UserId = adminUser.Id });
            //    context.Users.Add(adminUser);
            //}
        }
    }
}
