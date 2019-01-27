using System.ComponentModel.DataAnnotations;

namespace NotifyHealth.Models
{
    public class ClientMemberships
    {
        public int ClientMembershipId { get; set; }
        public int ClientId { get; set; }
        public int CampaignId { get; set; }

        [Display(Name = "Start Date")]
        public string Start { get; set; }
        public long SortTime { get; set; }  
        public string Clients { get; set; }
        public string Campaign { get; set; }
        public string Program { get; set; }
    }
}