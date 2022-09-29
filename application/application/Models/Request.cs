using System;
using System.Collections.Generic;

namespace application.Models
{
    public partial class Request
    {
        public long RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public long ServiceId { get; set; }
        public int? BuildingTypeId { get; set; }
        public int? BuildingSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Color { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
