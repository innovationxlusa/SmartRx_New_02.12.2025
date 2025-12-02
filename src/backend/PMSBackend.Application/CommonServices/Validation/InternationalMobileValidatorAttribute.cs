using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class InternationalMobileValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var mobile = value.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(mobile))
                return ValidationResult.Success;

            // Basic format check
            if (!mobile.StartsWith("+") || mobile.Length < 5)
            {
                return new ValidationResult("Mobile number must start with '+' followed by country code and number.");
            }

            // Dictionary of country calling codes and length rules
            var countryRules = new Dictionary<string, (string Name, int Min, int Max)>
        {
            { "+880", ("Bangladesh", 11, 11) },
            { "+91",  ("India", 10, 10) },
            { "+1",   ("United States", 10, 10) },
            { "+44",  ("United Kingdom", 10, 12) },
            { "+966", ("Saudi Arabia", 9, 9) },
            { "+971", ("UAE", 9, 9) },
            { "+61",  ("Australia", 9, 9) },
            { "+81",  ("Japan", 10, 10) },
        };

            // Try to detect country prefix
            var detected = countryRules.FirstOrDefault(c => mobile.StartsWith(c.Key));
            if (string.IsNullOrEmpty(detected.Key))
            {
                return new ValidationResult("Unrecognized country code in mobile number.");
            }

            var countryName = detected.Value.Name;
            var prefix = detected.Key;
            var numberWithoutPrefix = mobile.Substring(prefix.Length);

            // Validate length
            if (numberWithoutPrefix.Length < detected.Value.Min || numberWithoutPrefix.Length > detected.Value.Max)
            {
                return new ValidationResult(
                    $"Invalid mobile number length for {countryName}. Expected {detected.Value.Min}–{detected.Value.Max} digits after the country code."
                );
            }

            // Must contain only digits after '+'
            if (!numberWithoutPrefix.All(char.IsDigit))
            {
                return new ValidationResult("Mobile number must contain only digits after the country code.");
            }

            return ValidationResult.Success;
        }
    }
}