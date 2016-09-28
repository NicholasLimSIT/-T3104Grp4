using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.Models
{
    public class Programme
    {
        
        public Programme()
        {
            this.Modules = new HashSet<Module>();
        }

        public int Id { get; set; }
        public string name { get; set; }

        
        public virtual ICollection<Module> Modules { get; set; }
    }

}
