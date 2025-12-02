using System.ComponentModel.DataAnnotations;
using PMSBackend.Application.CommonServices.Validation;

namespace PMSBackend.Application.DTOs
{
    public class RelativeDTO
    {
        public long Id { get; set; }
        public string? PatientCode { get; set; }
        
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string? FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string? LastName { get; set; }
        
        [StringLength(50, ErrorMessage = "Nick Name cannot exceed 50 characters.")]
        public string? NickName { get; set; }

        [Range(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
        public decimal? Age { get; set; }
        
        [Range(0, 120, ErrorMessage = "Age Year must be between 0 and 120.")]
        public int? AgeYear { get; set; }
        
        [Range(0, 11, ErrorMessage = "Age Month must be between 0 and 11.")]
        public int? AgeMonth { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        
        [Range(1, 3, ErrorMessage = "Gender must be a valid value (1-3).")]
        public int? Gender { get; set; }//enum
        
        [Range(1, 8, ErrorMessage = "Blood Group must be a valid value (1-8).")]
        public int? BloodGroup { get; set; }//enum  
        
        [StringLength(20, ErrorMessage = "Height cannot exceed 20 characters.")]
        public string? Height { get; set; }
        
        [RegularExpression(@"^(\+88)?01[3-9]\d{8}$", ErrorMessage = "Phone number must be a valid Bangladeshi mobile number.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }
        
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string? Email { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }
        
        public long? PoliceStationId { get; set; }
        
        public long? CityId { get; set; }
        
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Postal code must be 4 digits.")]
        public string? PostalCode { get; set; }
        
        [RegularExpression(@"^(\+88)?01[3-9]\d{8}$", ErrorMessage = "Emergency contact must be a valid Bangladeshi mobile number.")]
        [StringLength(15, ErrorMessage = "Emergency contact cannot exceed 15 characters.")]
        public string? EmergencyContact { get; set; }
        
        [Range(1, 4, ErrorMessage = "Marital Status must be a valid value (1-4).")]
        public int? MaritalStatus { get; set; }//enum
        
        [StringLength(100, ErrorMessage = "Profession cannot exceed 100 characters.")]
        public string? Profession { get; set; }
        [ExistingPatientValidation(ErrorMessage = "ExistingPatientId is required when Existing Patient checkbox checked.")]
        public bool? IsExistingPatient { get; set; }
        
        public long? ExistingPatientId { get; set; }
        public bool? IsRelative { get; set; }
        public string? RelationToPatient { get; set; }
        public long? RelatedToPatientId { get; set; }
        public int ProfileProgress { get; set; }
        public int? IsActive { get; set; }
    }
}
