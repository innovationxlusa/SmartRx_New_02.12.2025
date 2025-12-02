using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_Master")]
    public class SmartRx_MasterEntity : BaseEntity
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity PrescriptionUser { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity PatientProfile { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }

        public DateTime? PrescriptionDate { get; set; }

        public string ChiefComplaintIds { get; set; }


        //public virtual IList<Prescription_UploadEntity> Prescriptions { get; set; } = new List<Prescription_UploadEntity>();
        //public virtual IList<SmartRx_PatientChiefComplaintEntity> PatientChiefComplaints { get; set; } = new List<SmartRx_PatientChiefComplaintEntity>();
        //public virtual IList<SmartRx_PatientDoctorEntity> PatientDoctors { get; set; } = new List<SmartRx_PatientDoctorEntity>();
        public DateTime? NextAppoinmentDate { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string? NextAppoinmentTime { get; set; }
        //public virtual IList<SmartRx_PatientDoctorEntity> ReferredConsultants { get; set; }=new List<SmartRx_PatientDoctorEntity>();
        public decimal? DiscountPercentageOnMedicineByDoctor { get; set; }
        public decimal? DiscountPercentageOnInvestigationByDoctor { get; set; }
        public string? Remarks { get; set; }

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
        [Column(TypeName = "varchar(500)")]
        public string? ReportReason { get; set; }
        [Column(TypeName = "varchar(4000)")]
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

        public bool? IsRejected { get; set; }
        public long? RejectedById { get; set; }
        [ForeignKey("RejectedById")]
        public virtual SmartRxUserEntity? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectionRemarks { get; set; }


        public bool? IsExistingPatient { get; set; }// if true fill up patientid column
        //public long? ExistingPatientId { get; set; }
        //[ForeignKey("ExistingPatientId")]
        //public virtual SmartRx_PatientProfileEntity? ExistingPatient { get; set; }

        public bool? HasAnyRelative { get; set; } // if true enter list of relatives in below table

        public virtual IList<SmartRx_PatientRelativesEntity>? ExistingRelativePatient { get; set; }



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


        public virtual IList<SmartRx_PatientVitalsEntity> PatientVitals { get; set; }
        public virtual IList<SmartRx_PatientHistoryEntity>? SmartRx_PatientHistory { get; set; }
        public virtual IList<SmartRx_PatientMedicineEntity>? SmartRx_PatientMedicine { get; set; }
        public virtual IList<SmartRx_PatientInvestigationEntity>? SmartRx_PatientInvestigation { get; set; }
        public virtual IList<SmartRx_PatientAdviceEntity>? SmartRx_PatientAdvice { get; set; }
        public virtual IList<SmartRx_PatientOtherExpenseEntity>? SmartRx_PatientOtherExpense { get; set; }

        public bool? HasMedicineFavourite { get; set; }
        public bool? HasInvestigationFavourite { get; set; }




    }

}
