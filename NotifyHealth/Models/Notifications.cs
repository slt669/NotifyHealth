using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotifyHealth.Models
{
    public class Notifications
    {
        public int NotificationId { get; set; }
        public int Period { get; set; }
        public string Text { get; set; }
        [Display(Name = "Status Id")]
        public virtual int StatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> Statuses { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "NTypeId")]
        public virtual int NTypeId { get; set; }

        [Display(Name = "NotificationTypes")]
        public virtual IEnumerable<SelectListItem> NotificationTypes { get; set; }

        [Display(Name = "NotificationType")]
        public string NotificationType { get; set; }

        [Display(Name = "CampaignId")]
        public virtual int CampaignId { get; set; }

        [Display(Name = "Campaigns")]
        public virtual IEnumerable<SelectListItem> Campaigns { get; set; }

        [Display(Name = "Campaign")]
        public string Campaign { get; set; }
        public int? OrganizationID { get; set; }
        //public ICollection<Queue> Queue { get; set; }
        //public ICollection<Transactions> Transactions { get; set; }
        [Display(Name = "ProgramID")]
        public virtual int ProgramID { get; set; }

        [Display(Name = "Programs")]
        public virtual IEnumerable<SelectListItem> Programs { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }
    }
}