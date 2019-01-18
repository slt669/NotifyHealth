using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models.ViewModels
{
    public class DashboardViewModel
    {

        public int NewClientsLast30 { get; set; }

        public int NotificationsSentLast30 { get; set; }

        public int NoOfClients { get; set; }


        public int ReportingMonth { get; set; }

        public int Notifications { get; set; }

    }
}