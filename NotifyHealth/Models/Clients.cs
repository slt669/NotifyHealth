using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NotifyHealth.Models
{
    public class Clients
    {
        public int ClientId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Carrier")]
        [ReadOnly(true)]
        public string PhoneCarrier { get; set; }

        [Display(Name = "SMS Address")]
        [ReadOnly(true)]
        public string MessageAddress { get; set; }

        [ReadOnly(true)]
        public int? OrganizationID { get; set; }

        [Display(Name = "CStatusId")]
        public virtual int CStatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> ClientStatuses { get; set; }

        [Display(Name = "Client Status")]
        public string ClientStatus { get; set; }

        [Display(Name = "PStatusId")]
        public virtual int PStatusId { get; set; }

        [Display(Name = "Statuses")]
        public virtual IEnumerable<SelectListItem> PhoneStatuses { get; set; }

        [Display(Name = "Phone Status")]
        public string PhoneStatus { get; set; }

        [Display(Name = "PStatusId")]
        public virtual int ParticipationId { get; set; }

        [Display(Name = "Participation Reasons")]
        public virtual IEnumerable<SelectListItem> ParticipationReasons { get; set; }

        [Display(Name = "Participation Reason")]
        public string ParticipationReason { get; set; }

        [Display(Name = "ATypeId")]
        public virtual int ATypeId { get; set; }

        [Display(Name = "Account Types")]
        public virtual IEnumerable<SelectListItem> AccountTypes { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "CampaignId")]
        public virtual int CampaignId { get; set; }

        [Display(Name = "Account Types")]
        public virtual IEnumerable<SelectListItem> Campaigns { get; set; }

        [Display(Name = "Campaign")]
        public string Campaign { get; set; }

        [Display(Name = "CampaignId")]
        public virtual int CampaignIdDDL { get; set; }

        [Display(Name = "CampaignsDDL")]
        public virtual IEnumerable<SelectListItem> CampaignsDDL { get; set; }

        [Display(Name = "CampaignDDL")]
        public string CampaignDDL { get; set; }


        public List<ClientMemberships> ClientMemberships { get; set; }
       
    }
}