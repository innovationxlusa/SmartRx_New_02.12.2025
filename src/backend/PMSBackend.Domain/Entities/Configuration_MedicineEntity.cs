using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_Medicine")]
    public class Configuration_MedicineEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }
        public long BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Configuration_MedicineBrandEntity Brand { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Type { get; set; }
        [Column(TypeName = "nvarchar(400)")]
        public string? Slug { get; set; }
        public long DosageFormId { get; set; }
        [ForeignKey("DosageFormId")]
        public virtual Configuration_MedicineDosageFormEntity MedicineDosageForm { get; set; }
        public long GenericId { get; set; }
        [ForeignKey("GenericId")]
        public virtual Configuration_MedicineGenericEntity Generic { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Strength { get; set; }
        public long? MeasurementUnitId { get; set; }
        [ForeignKey("MeasurementUnitId")]
        public virtual Configuration_UnitEntity? MeasurementUnit { get; set; } = new Configuration_UnitEntity();
        [Column(TypeName = "decimal(5,2)")]
        public decimal? UnitPrice { get; set; }
        public long? PriceInUnitId { get; set; }

        [ForeignKey("PriceInUnitId")]
        public virtual Configuration_UnitEntity? PriceUnit { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? PackageType { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? PackageSize { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? PackageQuantity { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? DAR { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Indication { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Pharmacology { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? DoseDescription { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Administration { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Contradiction { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? SideEffects { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? PrecautionsAndWarnings { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? PregnencyAndLactation { get; set; }
        [Column(TypeName = "nvarchar(1)")]
        public string? ModeOfAction { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Interaction { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? OverdoseEffects { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? TherapeuticClass { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? StorageCondition { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? UserFor { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? CompanyDiscountPercentage { get; set; }


        public bool IsActive { get; set; }
        public virtual IList<SmartRx_PatientMedicineEntity> SmartRx_PatientMedicine { get; set; }

    }
}