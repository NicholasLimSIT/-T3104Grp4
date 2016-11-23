using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICT3104_Group4_SMS.Models
{
    public class GradeRecViewModel
    {
        public Grade GradeItem { get; set; }
        public Recommendation RecItem { get; set; }
        [Display(Name="Module")]
        public String ModuleName { get; set; }
        [Display(Name="Student")]
        public String StudentName { get; set; }
    }
}