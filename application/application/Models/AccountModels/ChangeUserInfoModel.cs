using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace application.Models.AccountModels
{
    public class ChangeUserInfoModel
    {
        public string? Email { get; set; }

        [Display(Name = "Мобилен телефон")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Тип")]
        public int? ClientTypeId { get; set; }

        [Display(Name = "Име")]
        public string? Name { get; set; } 

        [Display(Name = "Адреса")]
        public string? Address { get; set; } 

        [Display(Name = "Број на ид")]
        public string? IdNo { get; set; } 

        [Display(Name = "Град")]
        public int? CityId { get; set; }

        [Display(Name = "Држава")]
        public int? CountryId { get; set; }
    }
}
