using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class ClientPhone
    {
      

        public string Number { get; set; }
        public string CarrierName { get; set; }
        public string Address { get; set; }

        public string Wireless { get; set; }
        public int ParsedStatus { get; set; }

        public string Status { get; set; }
    }

}