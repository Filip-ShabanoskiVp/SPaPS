using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace application.Models
{
    public partial class Request
    {
        public long RequestId { get; set; }

        [Display(Name = "Датум на барање")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Сервис")]
        public long ServiceId { get; set; }

        [Display(Name = "Тип на градба")]
        public int? BuildingTypeId { get; set; }

        [Display(Name = "Големина на градба")]
        public int? BuildingSize { get; set; }

        [Display(Name = "Датум од")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Датум до")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Боја")]
        public string? Color { get; set; }

        [Display(Name = "Забелешка")]
        public string? Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        [Display(Name = "Активен")]
        public bool? IsActive { get; set; }

        [Display(Name = "Број на прозорци")]
        public int? NoOfWindows { get; set; }

        [Display(Name = "Број на врати")]
        public int? NoOfDoors { get; set; }
        public long? ContractorId { get; set; }

        [Display(Name = "Сервиси")]
        public virtual Service? Service { get; set; } = null!;
    }
}
