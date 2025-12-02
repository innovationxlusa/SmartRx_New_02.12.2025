using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Reward_Rules")]
    public class Configuration_RewardRuleEntity : BaseEntity
    {
        [Required, MaxLength(10)]
        public string ActivityCode { get; set; }   // e.g., FILE_UPLOAD, VIEW_DOCTOR_FIRST

        [Required, MaxLength(250)]
        public string ActivityName { get; set; }
        [Required, MaxLength(250)]
        public string ActivityHeader { get; set; }
        [Required]
        public string ActivityTaken { get; set; }

        [Required]
        public RewardType RewardType { get; set; }  // NEW: 1/2/3

        public string RewardDetails { get; set; }

        public bool IsDeductible { get; set; } = false;
        public bool IsVisibleBenifit{ get; set; } = false;


        [Column(TypeName = "decimal(18,2)")]
        public double Points { get; set; }         // also negative if deduction

        public bool IsActive { get; set; } = true;


    }
}
