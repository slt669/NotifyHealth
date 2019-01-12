using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class Clients
    {
        public int ClientId { get; set; }
        [ReadOnly(true)]
        public int CStatusId { get; set; }
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
        public int? PStatusId { get; set; }
        public int ParticipationId { get; set; }
        [ReadOnly(true)]
        public int ATypeId { get; set; }
        public int? OrganizationID { get; set; }
        //public ClientStatus ClientStatus { get; set; }
        //public PhoneStatus PhoneStatus { get; set; }
        //public ParticipationReason ParticipationReason { get; set; }
        //public AccountType AccountType { get; set; }

        //public ICollection<ClientMemberships> ClientMemberships { get; set; }
        //public ICollection<Queue> Queue { get; set; }
        //public ICollection<Transactions> Transactions { get; set; }
    }
}