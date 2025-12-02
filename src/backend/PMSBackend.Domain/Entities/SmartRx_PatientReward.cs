using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientReward")]
    public class SmartRx_PatientReward : BaseEntity
    {

        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        public long? SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity? SmartRxMaster { get; set; }

        public long? PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity? Prescription { get; set; }

        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity PatientProfile { get; set; }

        [Required]
        public long RewardId { get; set; }
        // --- Navigation Properties ---
        [ForeignKey(nameof(RewardId))]
        public virtual Configuration_Reward Rewards { get; set; }

        [Required]
        public long? BadgeId { get; set; }
        // --- Navigation Properties ---
        [ForeignKey(nameof(BadgeId))]
        public virtual Configuration_RewardBadgeEntity? RewardBadge { get; set; }

        // --- Points Tracking ---
        [Column(TypeName = "decimal(18,2)")]
        public double EarnedNonCashablePoints { get; set; }

        public bool IsDeductPoints { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public double ConsumedNonCashablePoints { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double ConvertedCashableToNonCashablePoints { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double TotalNonCashablePoints { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public double EarnedCashablePoints { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double ConsumedCashablePoints { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double ConvertedNonCashableToCashablePoints { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double TotalCashablePoints { get; set; }

        // --- Monetary Tracking ---
        [Column(TypeName = "decimal(18,2)")]
        public double? ConvertedCashableToMoney { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double? EncashedMoney { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double? TotalMoney { get; set; }


        [MaxLength(500)]
        public string? Remarks { get; set; }


    }
}
