using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class Campaigns
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public int? ProgramId { get; set; }

        public Programs Programs { get; set; }
        //public Status Status { get; set; }

        //public ICollection<ClientMemberships> ClientMemberships { get; set; }
    }
}