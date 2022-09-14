using System.ComponentModel;

namespace application.Models.AccountModels
{
    public class LoginModel
    {
        [DisplayName("Korisnicko ime")]
        public string email { get; set; }

        [DisplayName("Lozinka")]
        public string password { get; set; }
    }
}
