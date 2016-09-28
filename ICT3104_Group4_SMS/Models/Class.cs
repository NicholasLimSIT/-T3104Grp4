using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.Models
{
    public class Class
    {
        
        public Class()
        {
            this.Modules = new HashSet<Module>();
        }

        public int Id { get; set; }
        [Display(Name = "Class Name")]
        public string name { get; set; }

       
        public virtual ICollection<Module> Modules { get; set; }

    }
}