using ICT3104_Group4_SMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.DAL
{
    public class SmsContext : IdentityDbContext<ApplicationUser>
    {
        public SmsContext() : base("3104_SmsDB")
        {
            //Database.SetInitializer<CafeWithLoveContext>(new CafeWithLoveInitializer());
        }
    }
}