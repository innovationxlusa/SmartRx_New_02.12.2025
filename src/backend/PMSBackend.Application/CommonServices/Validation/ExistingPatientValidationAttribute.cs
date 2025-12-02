using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.CommonServices.Validation
{
    public class ExistingPatientValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var isExistingPatientProperty = instance.GetType().GetProperty("IsExistingPatient");
            var existingPatientIdProperty = instance.GetType().GetProperty("ExistingPatientId");

            if (isExistingPatientProperty != null && existingPatientIdProperty != null)
            {
                var isExistingPatient = (bool?)isExistingPatientProperty.GetValue(instance);
                var existingPatientId = (long?)existingPatientIdProperty.GetValue(instance);

                if (isExistingPatient == true && (existingPatientId == null || existingPatientId <= 0))
                {
                    return new ValidationResult("ExistingPatientId is required when IsExistingPatient is true.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
