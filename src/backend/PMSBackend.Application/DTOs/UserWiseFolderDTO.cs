using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class UserWiseFolderDTO
    {
        public long Id { get; set; }
        public long? ParentFolderId { get; set; }
        public long FolderHierarchy { get; set; }
        public string FolderName { get; set; }
        public string Description { get; set; }
        public long? PatientId { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public bool IsFolder { get; set; }

        public bool IsRewardUpdated { get; set; }
        public string RewardTitle { get; set; }
        public double TotalRewardPoints { get; set; }
        public string RewardMessage { get; set; }

        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}
