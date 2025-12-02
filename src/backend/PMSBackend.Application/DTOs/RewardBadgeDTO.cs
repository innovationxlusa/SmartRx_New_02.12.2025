using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Application.DTOs
{
    public class RewardBadgeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public BadgeType BadgeType { get; set; }
        public int Heirarchy { get; set; }
        public int? RequiredPoints { get; set; }
        public int? RequiredActivities { get; set; }
        public bool? IsActive { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
