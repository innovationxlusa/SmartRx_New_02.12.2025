using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices.Validation;
using PMSBackend.Domain.SharedContract;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.DTOs
{
    public class PatientUpdateDTO
    {
        // Required fields - these will be provided separately
        // public long PatientId { get; set; } - Provided in command
        // public long LoginUserId { get; set; } - Provided in command

        // All other fields are optional for update
        [OptionalStringLength(0, 50, ErrorMessage = "Patient Code cannot exceed 50 characters.")]
        public string? PatientCode { get; set; }
        [OptionalStringLength(2, 50, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string? FirstName { get; set; }
        
        [OptionalStringLength(2, 50, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string? LastName { get; set; }
        
        [OptionalStringLength(0, 50, ErrorMessage = "Nick Name cannot exceed 50 characters.")]
        public string? NickName { get; set; }

        [OptionalRange(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
        public decimal? Age { get; set; }
        
        [OptionalRange(0, 120, ErrorMessage = "Age Year must be between 0 and 120.")]
        public int? AgeYear { get; set; }
        
        [OptionalRange(0, 11, ErrorMessage = "Age Month must be between 0 and 11.")]
        public int? AgeMonth { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        
        [OptionalRange(1, 3, ErrorMessage = "Gender must be a valid value (1-3).")]
        public int? Gender { get; set; }//enum
        
        [OptionalRange(1, 8, ErrorMessage = "Blood Group must be a valid value (1-8).")]
        public int? BloodGroup { get; set; }//enum       

        public string? Height { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public string? HeightMeasurementUnit { get; set; }
        public long? HeightMeasurementUnitId { get; set; }
        public decimal? Weight { get; set; }
        public string? WeightMeasurementUnit { get; set; }
        public long? WeightMeasurementUnitId { get; set; }

        [OptionalRegularExpression(@"^\d+$", ErrorMessage = "Mobile number must contain digits only.")]
        public string? MobileNo { get; set; }
        
        [OptionalEmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [OptionalStringLength(0, 100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string? Email { get; set; }
        
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Profile photo size cannot exceed 5MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "Only JPG, JPEG, PNG, and GIF files are allowed.")]
        public IFormFile? ProfilePhoto { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        
        [OptionalStringLength(0, 500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }
        
        public long? PoliceStationId { get; set; }
        public long? CityId { get; set; }
        
        [OptionalRegularExpression(@"^\d{4}$", ErrorMessage = "Postal code must be 4 digits.")]
        public string? PostalCode { get; set; }
        
        [OptionalRegularExpression(@"^(\+88)?01[3-9]\d{8}$", ErrorMessage = "Emergency contact must be a valid Bangladeshi mobile number.")]
        [OptionalStringLength(0, 15, ErrorMessage = "Emergency contact cannot exceed 15 characters.")]
        public string? EmergencyContact { get; set; }
        
        [OptionalRange(1, 4, ErrorMessage = "Marital Status must be a valid value (1-4).")]
        public int? MaritalStatus { get; set; }//enum
        
        [OptionalStringLength(0, 100, ErrorMessage = "Profession cannot exceed 100 characters.")]
        public string? Profession { get; set; }
        
        public bool? IsExistingPatient { get; set; }
        public long? ExistingPatientId { get; set; }
        public bool? IsRelative { get; set; }
        public string? RelationToPatient { get; set; }
        public long? RelatedToPatientId { get; set; }
        public int? ProfileProgress { get; set; }
        public List<RelativeContract>? Relatives { get; set; }

        public bool? IsActive { get; set; }
    }
}
