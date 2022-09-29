using System.ComponentModel.DataAnnotations;

namespace application.Models.CustomAnnotations
{
    public class DateEqualOrGreaterThanCurrentDate : ValidationAttribute
    {
        
        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            var date = (DateTime)value;

            if (date >= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
