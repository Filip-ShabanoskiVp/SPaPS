using System;
using System.Collections.Generic;

namespace application.Models
{
    public partial class ClientActivity
    {
        public long ClientActivityId { get; set; }
        public long ClientId { get; set; }
        public long ActivityId { get; set; }

        public virtual Activity Activity { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;
    }
}
