using System.ComponentModel.DataAnnotations;

namespace application.Models.AccountModels
{
    public class ResetPasswordModel
    {

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Внесете нова лозинка")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Внесете потврдете лозинка")]
        [Compare("NewPassword", ErrorMessage = "Пасвордите не се компатабилни")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
