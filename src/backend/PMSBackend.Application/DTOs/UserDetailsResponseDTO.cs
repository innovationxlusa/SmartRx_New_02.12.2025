using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Application.DTOs
{
    public class UserDetailsResponseDTO
    {
        public long Id { get; set; }
        public string UserCode { get; set; } = default!;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; } = string.Empty!;
        public string? Email { get; set; } = string.Empty!;
        public string? GoogleId { get; set; } = string.Empty!;
        public string? FacebookId { get; set; } = string.Empty!;
        public string? TwitterId { get; set; } = string.Empty!;
        public string? FirstName { get; set; } = string.Empty!;
        public string? LastName { get; set; } = string.Empty!;
        public int? AuthMethod { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        public UserWiseFolderDTO UserFolderDetails { get; set; }
        public IList<SmartRxUserRoleEntity> Roles { get; set; } = default!;
        public ApiResponseResult? ApiResponseResult { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }
}
