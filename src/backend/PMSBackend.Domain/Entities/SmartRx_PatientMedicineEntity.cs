using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientMedicine")]
    public class SmartRx_PatientMedicineEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }
        public long MedicineId { get; set; }
        [ForeignKey("MedicineId")]
        public virtual Configuration_MedicineEntity Medicine { get; set; }



        [Column(TypeName = "nvarchar(50)")]
        public int? FrequencyInADay { get; set; }// 1+1+1, if value =3, divide 24 hours by 3= take medicine after each 8 hours and next 3 colum data need to input with value dose (E.g. 10mg).



        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose1InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose2InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose3InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose4InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose5InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose6InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose7InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose8InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose9InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose10InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose11InADay { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Dose12InADay { get; set; }


        public bool? IsMoreThanRegularDose { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? DescriptionForMoreThanRegularDose { get; set; }
        public bool? IsBeforeMeal { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string DurationOfContinuation { get; set; }// 1 month/3 months/Continue etc.
        public int DurationOfContinuationCount { get; set; }
        public DateTime DurationOfContinuationStartDate { get; set; }
        public DateTime DurationOfContinuationEndDate { get; set; }


        [Column(TypeName = "nvarchar(1000)")]
        public string? Rules { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? Restrictions { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? Notes { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Wishlist { get; set; }
    }
}
