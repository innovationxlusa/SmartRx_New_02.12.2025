using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Prescription_Upload")]
    public class Prescription_UploadEntity : BaseEntity
    {
        [Column(TypeName = "nchar(20)")]
        public string PrescriptionCode { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity? PatientProfile { get; set; }

        public long? SmartRxId { get; set; }
        [ForeignKey("SmartRxId")]
        public virtual SmartRx_MasterEntity? SmartRx { get; set; } = null!;
        public bool? IsExistingPatient { get; set; }
        public bool? HasExistingRelative { get; set; }
        public string? RelativePatientIds { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string FileName { get; set; } = default!;
        [Column(TypeName = "nvarchar(1000)")]
        public string FilePath { get; set; } = default!;
        [Column(TypeName = "nvarchar(10)")]
        public string FileExtension { get; set; }

        public int NumberOfFilesStoredForThisPrescription { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }
        public long FolderId { get; set; }
        [ForeignKey("FolderId")]
        public virtual Prescription_UserWiseFolderEntity Folder { get; set; }

        public bool? IsSmartRxRequested { get; set; }
        public bool? IsLocked { get; set; }
        public long? LockedById { get; set; } = default!;
        [ForeignKey("LockedById")]
        public virtual SmartRxUserEntity? LockedBy { get; set; }
        public DateTime? LockedDate { get; set; } = default!;
        public bool? IsReported { get; set; }
        public long? ReportById { get; set; }
        [ForeignKey("ReportById")]
        public virtual SmartRxUserEntity? ReportBy { get; set; }
        public DateTime? ReportDate { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? ReportReason { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? ReportDetails { get; set; }
        public bool? IsRecommended { get; set; }
        public long? RecommendedById { get; set; }
        [ForeignKey("RecommendedById")]
        public virtual SmartRxUserEntity? RecommendedBy { get; set; }
        public DateTime? RecommendedDate { get; set; }

        public bool? IsApproved { get; set; }
        public long? ApprovedById { get; set; }
        [ForeignKey("ApprovedById")]
        public virtual SmartRxUserEntity? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? IsCompleted { get; set; }
        public long? CompletedById { get; set; }
        [ForeignKey("CompletedById")]
        public virtual SmartRxUserEntity? CompletedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Tag1 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Tag2 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Tag3 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Tag4 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Tag5 { get; set; }
        public DateTime? NextAppoinmentDate { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? NextAppoinmentTime { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentageOnMedicineByDoctor { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentageOnInvestigationByDoctor { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual IList<SmartRx_PatientHistoryEntity> SmartRx_PatientHistory { get; set; }// = new List<SmartRx_PatientHistoryEntity>();
        //public virtual IList<SmartRx_PatientVitalsEntity> SmartRx_PatientVital { get; set; } = new List<SmartRx_PatientVitalsEntity>();

        public virtual IList<SmartRx_PatientMedicineEntity> SmartRx_PatientMedicine { get; set; }// = new List<SmartRx_PatientMedicineEntity>();

        public virtual IList<SmartRx_PatientOtherExpenseEntity>? SmartRx_PatientOtherExpense { get; set; }


        //public virtual IList<SmartRx_PatientChiefComplaintEntity> SmartRx_PatientCC { get; set; }
        //public virtual IList<SmartRx_PatientDoctorEntity> SmartRx_PatientDoctor { get; set; }
        //public virtual IList<SmartRx_PatientVitalsEntity >SmartRx_PatientVitals { get; set; }


    }
}
