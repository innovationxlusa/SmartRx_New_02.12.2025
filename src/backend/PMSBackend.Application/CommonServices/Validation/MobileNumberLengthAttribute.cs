using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class MobileNumberLengthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var model = validationContext.ObjectInstance;
            var countryProp = validationContext.ObjectType.GetProperty("MobileNo");

            if (countryProp == null)
                return new ValidationResult("User Name is invalid.");

            var countryCode = countryProp.GetValue(model)?.ToString();
            var mobile = value.ToString();

            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(mobile))
                return ValidationResult.Success;

            // You can define your rules per country here:
            var rules = new Dictionary<string, (int Min, int Max)>
        {
            { "BD", (11, 11) }, // Bangladesh
            { "US", (10, 10) },
            { "IN", (10, 10) },
            { "UK", (10, 12) },
            { "SA", (9, 9) }
        };

            if (rules.TryGetValue(countryCode, out var rule))
            {
                if (mobile.Length < rule.Min || mobile.Length > rule.Max)
                {
                    return new ValidationResult($"Mobile number for {countryCode} must be between {rule.Min} and {rule.Max} digits.");
                }
            }

            return ValidationResult.Success;
        }
    }
}