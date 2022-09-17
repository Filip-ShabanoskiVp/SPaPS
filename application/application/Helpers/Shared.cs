
using PasswordGenerator;

namespace application.Helpers
{
    public class Shared
    {
        public static string GeneratePassword(int passwordLength)
        {
            var password = new Password(passwordLength).IncludeUppercase().IncludeLowercase().IncludeNumeric().IncludeSpecial().Next();
            return password;
        }
    }
}
