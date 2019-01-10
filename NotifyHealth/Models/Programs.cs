using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class Programs
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }

        //public Status Status { get; set; }

        //public ICollection<Campaigns> Campaigns { get; set; }
        public int? OrganizationID { get; set; }
        
    }
}