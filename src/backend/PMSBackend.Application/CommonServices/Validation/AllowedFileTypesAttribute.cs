using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class AllowedFileTypesAttribute : ValidationAttribute
    {
        private readonly HashSet<string> _allowedTypes;

        public AllowedFileTypesAttribute(string[] allowedTypes)
        {
            _allowedTypes = allowedTypes.Select(t => t.ToLowerInvariant()).ToHashSet();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success;

            var contentType = file.ContentType.ToLowerInvariant();

            if (!_allowedTypes.Contains(contentType))
            {
                return new ValidationResult($"File type '{contentType}' is not allowed. Allowed types: {string.Join(", ", _allowedTypes)}");
            }

            return ValidationResult.Success;
        }
    }
}