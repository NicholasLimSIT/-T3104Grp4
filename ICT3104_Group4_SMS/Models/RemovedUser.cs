namespace ICT3104_Group4_SMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class RemovedUser
    {

        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string year { get; set; }
        public string UserName { get; set; }

    }
}