using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.Models
{
    public class Recommendation
    {
        public int Id { get; set; }
        public string recomendation { get; set; }
        public int moduleId { get; set; }

        public virtual Module Module { get; set; }

    }
}