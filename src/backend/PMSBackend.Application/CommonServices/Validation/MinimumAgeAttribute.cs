using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        public override bool IsValid(object? value)
        {
            if (value is DateTime dob)
            {
                var age = DateTime.Today.Year - dob.Year;
                if (dob > DateTime.Today.AddYears(-age)) age--; // Adjust for birthday not yet reached
                return age >= _minimumAge;
            }
            return false;
        }
    }

}
