using System.ComponentModel.DataAnnotations;

namespace application.Models.AccountModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Внесете стара лозинка")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Внесете нова лозинка")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потврдете ја лозинка")]
        [Compare("NewPassword", ErrorMessage = "Пасвордите не се исти!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
