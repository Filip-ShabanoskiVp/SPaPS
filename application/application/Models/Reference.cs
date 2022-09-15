﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace application.Models
{
    public partial class Reference
    {
        public long ReferenceId { get; set; }
        public long ReferenceTypeId { get; set; }
        [DisplayName("Opis na referenca")]
        public string Description { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ReferenceType ReferenceType { get; set; } = null!;
    }
}