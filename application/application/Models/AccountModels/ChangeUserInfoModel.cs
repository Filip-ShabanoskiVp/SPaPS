using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace application.Models.AccountModels
{
    public class ChangeUserInfoModel
    {
        public string? Email { get; set; }


        [Required(ErrorMessage = "Внесете мобилен телефон")]
        [Display(Name = "Мобилен телефон")]
        public string? PhoneNumber { get; set; }


        [Required(ErrorMessage = "Внесете тип")]
        [Display(Name = "Тип")]
        public int? ClientTypeId { get; set; }

        [Required(ErrorMessage = "Внесете име")]
        [Display(Name = "Име")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Внесете адреса")]
        [Display(Name = "Адреса")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Внесете број на ид")]
        [Display(Name = "Број на ид")]
        public string? IdNo { get; set; }

        [Required(ErrorMessage = "Внесете Град")]
        [Display(Name = "Град")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Внесете Држава")]
        [Display(Name = "Држава")]
        public int? CountryId { get; set; }
    }
}
