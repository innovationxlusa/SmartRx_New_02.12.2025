using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.Validation;
using PMSBackend.Domain.SharedContract;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.DTOs
{
    public class PatientWithRelativesDTO
    {
        public long? Id { get; set; }
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
        
        [Range(0, 5000, ErrorMessage = "Age Year must be previous year with 4 digits.")]
        public int? AgeYear { get; set; }
        
        [Range(0, 11, ErrorMessage = "Age Month must be between 0 and 11.")]
        public int? AgeMonth { get; set; }

        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth must be a past date.")]
        public DateTime? DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Gender is required.")]
        [Range(1, 3, ErrorMessage = "Gender must be a valid value (1-3).")]
        public int? Gender { get; set; }//enum
        
        [Range(1, 8, ErrorMessage = "Blood Group must be a valid value (1-8).")]
        public int? BloodGroup { get; set; }//enum       

        public string? Height { get; set; }
        public int? HeightFeet { get; set; }
        public decimal? HeightInches { get; set; }
        public string? HeightMeasurementUnit { get; set; }
        public long? HeightMeasurementUnitId { get; set; }
        public decimal? Weight { get; set; }
        public string? WeightMeasurementUnit { get; set; }
        public long? WeightMeasurementUnitId { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Mobile number must contain digits only.")]
        [MobileNumberLength]
        public string? MobileNo { get; set; }
        
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string? Email { get; set; }
        
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Profile photo size cannot exceed 5MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "Only JPG, JPEG, PNG, and GIF files are allowed.")]
        public IFormFile? ProfilePhoto { get; set; }
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
        [ExistingPatientValidation(ErrorMessage = "ExistingPatientId is required when IsExistingPatient is true.")]
        public bool? IsExistingPatient { get; set; }
        
        public long? ExistingPatientId { get; set; }
        public bool? IsRelative { get; set; }
        public string? RelationToPatient { get; set; }

        public long? RelatedToPatientId { get; set; }
        public int? ProfileProgress { get; set; }
        public List<PatientDropdown>? RelativesDropdown { get; set; }
        public List<RelativeDTO>? Relatives { get; set; }
        //public Dictionary<long, string> RelativesInfo { get; set; }
        public bool? IsActive { get; set; }
        public int TotalPrescriptions { get; set; }
        public string? RxType { get; set; }
        public int? TotalSmartRx { get; set; } = 0;
        public int? TotalWaiting { get; set; } = 0;
        public int? TotalFileOnly { get; set; } = 0;
        public ApiResponseResult? ApiResponseResult { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }

    }
}
