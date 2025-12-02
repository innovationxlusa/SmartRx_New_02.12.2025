using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxPrescription
    {
        public long PrescriptionId { get; set; }
        public string PrescriptionCode { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public long? PatientId { get; set; }
        public bool? IsExistingPatient { get; set; }
        public bool? HasExistingRelative { get; set; }
        public string? RelativePatientIds { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string FileExtension { get; set; }
        public int NumberOfFilesStoredForThisPrescription { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public bool? IsSmartRxRequested { get; set; }
        public bool? IsLocked { get; set; }
        public long? LockedById { get; set; } = default!;
        public DateTime? LockedDate { get; set; } = default!;
        public bool? IsReported { get; set; }
        public long? ReportById { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ReportReason { get; set; }
        public string? ReportDetails { get; set; }
        public bool? IsRecommended { get; set; }
        public long? RecommendedById { get; set; }
        public DateTime? RecommendedDate { get; set; }
        public bool? IsApproved { get; set; }
        public long? ApprovedById { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool? IsCompleted { get; set; }
        public long? CompletedById { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? Tag3 { get; set; }
        public string? Tag4 { get; set; }
        public string? Tag5 { get; set; }
        public DateTime? NextAppoinmentDate { get; set; }
        public string? NextAppoinmentTime { get; set; }
        public decimal? DiscountPercentageOnMedicineByDoctor { get; set; }
        public decimal? DiscountPercentageOnInvestigationByDoctor { get; set; }

        public IList<SmartRxChiefComplaintDTO> ChiefComplaints { get; set; }
        public IList<PrescriptionAcronymDTO> ChiefComplaintAcronyms { get; set; }
        public SmartRxDoctorDTO Doctor { get; set; }
        public IList<SmartRxVital> Vitals { get; set; }
        public IList<SmartRxHistoryDTO>? Histories { get; set; }
        public IList<SmartRxMedicinesDTO>? Medicines { get; set; }
        public IList<SmartRxMedicineWishListDTO>? MedicineWishlist { get; set; }
        public IList<SmartRxInvestigationListDTO>? Investigations { get; set; }
        public IList<InvestigationWishlistContract>? InvestigationWishList { get; set; }
        public IList<SmartRxAdviceDTO>? Advices { get; set; }
        public IList<SmartRxAdviceFAQ>? AdviceRecommendations { get; set; }
        public IList<SmartRxOtherExpenseDTO>? OtherExpenses { get; set; }


    }
}
