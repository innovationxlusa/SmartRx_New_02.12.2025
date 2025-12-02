using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class PrescriptionUploadDTO
    {
        public long Id { get; set; }
        public string PrescriptionCode { get; set; }
        public long? PatientId { get; set; }
        public bool? IsExistingPatient { get; set; }
        public bool? HasExistingRelative { get; set; }
        public string? RelativePatientIds { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }
        public int? filStoredForThisPrescriptionCount { get; set; }
        public long FolderId { get; set; }
        public long UserId { get; set; }
        public bool? IsSmartRxRequested { get; set; }

        public bool? IsLocked { get; set; }
        public long? LockedBy { get; set; }
        public DateTime? LockedDate { get; set; }

        public bool? IsReported { get; set; }
        public long? ReportBy { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ReportReason { get; set; }
        public string? ReportDetails { get; set; }

        public bool? IsRecommended { get; set; }
        public long? RecommendedBy { get; set; }
        public DateTime? RecommendedDate { get; set; }

        public bool? IsApproved { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? IsCompleted { get; set; }
        public long? CompletedBy { get; set; }
        public DateTime? CompletedDate { get; set; }

        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? Tag3 { get; set; }
        public string? Tag4 { get; set; }
        public string? Tag5 { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }

        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}