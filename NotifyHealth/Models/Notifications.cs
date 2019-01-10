using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class Notifications
    {
        public int NotificationId { get; set; }
        public int NTypeId { get; set; }
        public int Period { get; set; }
        public string Text { get; set; }
        public int StatusId { get; set; }
        public int? CampaignId { get; set; }

        //public NotificationType NotificationType { get; set; }
        //public Status Status { get; set; }
        public Campaigns Campaigns { get; set; }

        //public ICollection<Queue> Queue { get; set; }
        //public ICollection<Transactions> Transactions { get; set; }
    }
}