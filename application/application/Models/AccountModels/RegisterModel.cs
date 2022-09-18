using System.ComponentModel.DataAnnotations;

namespace application.Models.AccountModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name ="Корисничко име")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Внесете мобилен телефон")]
        [Display(Name = "Мобилен телефон")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Внесете ид клиент")]
        [Display(Name = "Тип")]
        public int? ClientTypeId { get; set; }

        [Required(ErrorMessage = "Внесете име")]
        [Display(Name = "Име")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Внесете адреса")]
        [Display(Name = "Адреса")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Внесете број на ид")]
        [Display(Name = "Број на ид")]
        public string IdNo { get; set; } = null!;

        [Required(ErrorMessage = "Внесете Град")]
        [Display(Name = "Град")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Внесете Држава")]
        [Display(Name = "Држава")]
        public int? CountryId { get; set; }

    }
}
