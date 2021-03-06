﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NotifyHealth.Models
{
    public class Campaigns
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Status Id")]
        public virtual int StatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> Statuses { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public int? OrganizationID { get; set; }

        [Display(Name = "Related Notifications")]
        public int RelatedNotifications { get; set; }

        [Display(Name = "Tracks Id")]
        public virtual int ProgramId { get; set; }

        [Display(Name = "Track")]
        public virtual IEnumerable<SelectListItem> Programs { get; set; }

        [Display(Name = "Track")]
        public string Program { get; set; }
    }
}