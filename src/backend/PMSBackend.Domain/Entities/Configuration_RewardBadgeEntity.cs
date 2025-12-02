using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{

    [Table("Configuration_RewardBadge")]
    public class Configuration_RewardBadgeEntity : BaseEntity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public BadgeType BadgeType { get; set; }  // ActivityBased, PointsMilestone, Special
        public int Heirarchy { get; set; }
        public int? RequiredPoints { get; set; }  // e.g. 500 total points needed  
        public int? RequiredActivities { get; set; } // optional for activity-based

        [MaxLength(500)]
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        // Navigation property: multiple rewards or patient rewards can link to a badge
        public virtual ICollection<SmartRx_PatientReward>? PatientRewards { get; set; }
    }
}

