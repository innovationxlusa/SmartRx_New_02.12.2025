using System.ComponentModel.DataAnnotations;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Application.DTOs
{
    public class RewardPointConversionRequestDTO
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "FromType must be 1 (Noncashable), 2 (Cashable), or 3 (Money)")]
        public int FromType { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "ToType must be 1 (Noncashable), 2 (Cashable), or 3 (Money)")]
        public int ToType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public double Amount { get; set; }

        public string? Remarks { get; set; }
    }

    public class RewardPointConversionResponseDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public RewardType FromType { get; set; }
        public string FromTypeName { get; set; } = string.Empty;
        public RewardType ToType { get; set; }
        public string ToTypeName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public double ConvertedPoints { get; set; }
        public double Rate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
