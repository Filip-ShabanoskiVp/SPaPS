using System.ComponentModel.DataAnnotations;

namespace application.Models.AccountModels
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Внесете емаил")]
        public string Email { get; set; }
    }
}
