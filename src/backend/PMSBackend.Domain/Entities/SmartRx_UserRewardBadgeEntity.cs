using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    public class SmartRx_UserRewardBadgeEntity : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }

        [Required]
        public long BadgeId { get; set; }
        [ForeignKey("BadgeId")]
        public virtual Configuration_RewardBadgeEntity Badge { get; set; }

        public DateTime EarnedDate { get; set; }


    }
}
