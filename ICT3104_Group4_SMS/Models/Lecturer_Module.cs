//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICT3104_Group4_SMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lecturer_Module
    {
        public int Id { get; set; }
        public string lecturerId { get; set; }
        public int moduleId { get; set; }
    
        public virtual Module Module { get; set; }
    }
}
