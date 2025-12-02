using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Reward")]
    public class Configuration_Reward : BaseEntity
    {
        [Required]
        public long UserActivityId { get; set; }
        [ForeignKey("UserActivityId")]
        public virtual Configuration_UserActivityEntity UserActivity { get; set; }
        [Required, MaxLength(10)]
        public string RewardCode { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Details { get; set; }
        public bool IsDeduction { get; set; }      
        public bool IsCashable { get; set; }
        public double? NonCashablePoints { get; set; }
        public double? CashablePoints { get; set; }
        public bool? IsCashedMoney { get; set; }
        public double? CashedMoney { get; set; }
        public bool IsVisibleToUser { get; set; }
        public bool? IsActive { get; set; }
    }
}
