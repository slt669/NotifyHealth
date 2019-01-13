using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotifyHealth.Models
{
    public class Campaigns
    {
        public int? CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public int? ProgramId { get; set; }
        public string Program { get; set; }

        public Programs Programs { get; set; }
        [Display(Name = "Status Id")]
        public virtual int StatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> Statuses { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public int? OrganizationID { get; set; }
        //public ICollection<ClientMemberships> ClientMemberships { get; set; }
    }
}