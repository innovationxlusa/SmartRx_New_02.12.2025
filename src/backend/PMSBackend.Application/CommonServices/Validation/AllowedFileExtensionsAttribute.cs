using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success;

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !_extensions.Contains(extension))
            {
                return new ValidationResult($"File extension not allowed. Allowed: {string.Join(", ", _extensions)}");
            }

            return ValidationResult.Success;
        }
    }
}
