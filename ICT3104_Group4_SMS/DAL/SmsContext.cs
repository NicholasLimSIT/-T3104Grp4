using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class SmsContext : IdentityDbContext<ApplicationUser>
    {
        public SmsContext() : base("3104group4")
        {
            //Database.SetInitializer<CafeWithLoveContext>(new CafeWithLoveInitializer());
        }

        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Lecturer_Module> Lecturer_Module { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Module_Programme> Module_Programme { get; set; }
        public virtual DbSet<Programme> Programmes { get; set; }
        public virtual DbSet<Recommendation> Recommendations { get; set; }
    }
}