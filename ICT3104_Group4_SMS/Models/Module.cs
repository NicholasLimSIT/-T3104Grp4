using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.Models
{
    public class Module
    {
        
           
            public Module()
            {
                this.Recommendations = new HashSet<Recommendation>();
            }

            public int Id { get; set; }
            public string name { get; set; }
            public string grade { get; set; }
            public string userId { get; set; }
            public int programmeId { get; set; }
            public Nullable<int> classId { get; set; }

            public virtual ApplicationUser AspNetUser { get; set; }
            public virtual Class Class { get; set; }
            public virtual Programme Programme { get; set; }
            
            public virtual ICollection<Recommendation> Recommendations { get; set; }
        }

    
}