using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace application.Models.AccountModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Внесете корисничко име")]
        [DisplayName("Корисничко име")]
        public string email { get; set; }

        [Required(ErrorMessage = "Внесете лозинка")]
        [DisplayName("Лозинка")]
        public string password { get; set; }
    }
}
