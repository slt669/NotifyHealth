using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotifyHealth.Models
{
    public class NotificationType
    {

        public int NTypeId { get; set; }
        [Display(Name = "Type")]
        public string Value { get; set; }

        public ICollection<Notifications> Notifications { get; set; }
    }
}