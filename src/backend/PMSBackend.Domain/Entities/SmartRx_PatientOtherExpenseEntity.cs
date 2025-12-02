using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("SmartRx_PatientOtherExpense")]
    public class SmartRx_PatientOtherExpenseEntity : BaseEntity
    {
        public long SmartRxMasterId { get; set; }
        [ForeignKey("SmartRxMasterId")]
        public virtual SmartRx_MasterEntity SmartRxMaster { get; set; }

        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription_UploadEntity Prescription { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string ExpenseName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public long? CurrencyUnitId { get; set; }
        [ForeignKey("CurrencyUnitId")]
        public virtual Configuration_UnitEntity? CurrencyUnit { get; set; }

        public DateTime ExpenseDate { get; set; }
        public string? ExpenseNotes { get; set; }

    }
}