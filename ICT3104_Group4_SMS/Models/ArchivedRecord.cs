namespace ICT3104_Group4_SMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ArchivedRecord
    {
        public ArchivedRecord()
        {
            
        }

        public int Id { get; set; }
        public string studentName { get; set; }
        public string moduleName { get; set; }
        public double score { get; set; }
        public string grade { get; set; }


    }
}
