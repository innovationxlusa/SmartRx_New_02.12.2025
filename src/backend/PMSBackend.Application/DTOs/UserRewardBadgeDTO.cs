using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;
using System;

namespace PMSBackend.Application.DTOs
{
    public class UserRewardBadgeDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BadgeId { get; set; }
        public string BadgeName { get; set; } = string.Empty;
        public BadgeType BadgeType { get; set; }
        public int Heirarchy { get; set; }
        public int? RequiredPoints { get; set; }
        public int? RequiredActivities { get; set; }
        public string? BadgeDescription { get; set; }
        public bool? BadgeIsActive { get; set; }
        public DateTime EarnedDate { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UserRewardBadgesDTO
    {
        public UserRewardBadgeDTO? UserRewardBadge { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
    }

    public static class UserRewardBadgeMapper
    {
        public static UserRewardBadgeDTO ToDto(this SmartRx_UserRewardBadgeEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new UserRewardBadgeDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                BadgeId = entity.BadgeId,
                BadgeName = entity.Badge?.Name ?? string.Empty,
                BadgeType = entity.Badge?.BadgeType ?? BadgeType.PointsMilestone,
                Heirarchy = entity.Badge?.Heirarchy ?? 0,
                RequiredPoints = entity.Badge?.RequiredPoints,
                RequiredActivities = entity.Badge?.RequiredActivities,
                BadgeDescription = entity.Badge?.Description,
                BadgeIsActive = entity.Badge?.IsActive,
                EarnedDate = entity.EarnedDate,
                CreatedById = entity.CreatedById,
                CreatedDate = entity.CreatedDate,
                ModifiedById = entity.ModifiedById,
                ModifiedDate = entity.ModifiedDate
            };
        }
    }
}

