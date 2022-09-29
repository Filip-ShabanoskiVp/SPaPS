using System;
using System.Collections.Generic;

namespace application.Models
{
    public partial class ServiceActivity
    {
        public long ServiceActivityId { get; set; }
        public long ServiceId { get; set; }
        public long ActivityId { get; set; }

        public virtual Activity Activity { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
    }
}
