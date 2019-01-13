using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotifyHealth.Models
{
    public class Programs
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Status Id")]
        public virtual int StatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> Statuses { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        //public ICollection<Campaigns> Campaigns { get; set; }
        public int? OrganizationID { get; set; }
        
    }
}