using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace application.Models.CustomModels
{
    public class vm_Service
    {
        public long ServiceId { get; set; }

        [Required(ErrorMessage = "Внесете опис")]
        [Display(Name = "Опис")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Внесете Активности")]
        [Display(Name = "Активности")]
        public List<int?> ActivityIds { get; set; }

        [Display(Name = "Активности")]
        public string? Activities { get; set; }
    }
}
