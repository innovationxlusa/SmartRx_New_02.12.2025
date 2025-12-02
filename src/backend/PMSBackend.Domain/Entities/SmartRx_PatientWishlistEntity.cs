using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientWishlist")]
    public class SmartRx_PatientWishlistEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity PrescriptionUpload { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string WishListType { get; set; }// medicine, lab

        public long? PatientMedicineId { get; set; }
        [ForeignKey("PatientMedicineId")]
        public virtual Configuration_MedicineEntity? PatientMedicine { get; set; }

        public long? PatientWishlistMedicineId { get; set; }
        [ForeignKey("PatientWishlistMedicineId")]
        public virtual Configuration_MedicineEntity? PatientWishListMedicine { get; set; }

        public long? PatientTestId { get; set; }
        [ForeignKey("PatientTestId")]
        public virtual Configuration_InvestigationEntity? InvestigationTest { get; set; }
        public long? PatientRecommendedTestCenterId { get; set; }
        [ForeignKey("PatientRecommendedTestCenterId")]
        public virtual Configuration_HospitalEntity? RecommendedTestCenter { get; set; }
    }
}
