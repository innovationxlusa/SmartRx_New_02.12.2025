using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.DTOs
{
    public class RewardPointConversionsCreateDTO
    {
        [Required]
        public long UserId { get; set; }

        public RewardType FromType { get; set; }

        public RewardType ToType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Amount { get; set; }
       
    }

    public class RewardPointConversionsUpdateDTO
    {
        [Required]
        public long Id { get; set; }
        public RewardType FromType { get; set; }

        public RewardType ToType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Amount { get; set; }
    }

    public class RewardPointConversionsResponseDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public RewardType FromType { get; set; }
        public RewardType ToType { get; set; }
        public double Amount { get; set; }
        public double ConvertedPoints { get; set; }
        public double Rate { get; set; }
    }

    public class RewardPointConversionsDeleteDTO
    {
        [Required]
        public long Id { get; set; }
    }
}
