using System;
using System.Collections.Generic;

namespace application.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ClientActivities = new HashSet<ClientActivity>();
            ServiceActivities = new HashSet<ServiceActivity>();
        }

        public long ActivityId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ClientActivity> ClientActivities { get; set; }
        public virtual ICollection<ServiceActivity> ServiceActivities { get; set; }
    }
}
