using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class Transactions
    {
        public int TransactionId { get; set; }
        public int ClientId { get; set; }
        public int NotificationId { get; set; }
        public string Result { get; set; }
        public string Timestamp { get; set; }

        public string Client { get; set; }
        public string Notification { get; set; }
    }
}