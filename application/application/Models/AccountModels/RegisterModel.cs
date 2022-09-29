using System.ComponentModel.DataAnnotations;
using application.Models.CustomAnnotations;
using Microsoft.AspNetCore.Identity;

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

        [Required(ErrorMessage = "Внесете Улога")]
        [Display(Name = "Улога")]
        public string? Role { get; set; }

        [Display(Name = "Број на вработени")]
        [Range(0,Int32.MaxValue,ErrorMessage ="Вредноста мора да биде поголема или еднаква на 0")]
        public int? NoOfEmployees { get; set; }

        [Display(Name = "Датум на основање")]
        [DateEqualOrGreaterThanCurrentDate(ErrorMessage ="Датумот мора да биде поголем или еднаков од денешниот датум!")]
        public DateTime? DateEstablished { get; set; }

        [Display(Name = "Активности")]
        public List<int?> ActivityIds { get; set; } = new List<int?>();

    }
}
