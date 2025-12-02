using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    public class MedicineInfoModel
    {
        public long MedicineId { get; set; }
        public string MedicineName { get; set; }
        public long? BrandId { get; set; }
        public long? ManufacturerId { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? ManufacturerName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ManufacturerOriginRegion { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Importer { get; set; }
        public DateTime? ManufacturCompanyEstablishedDate { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? ManufacturingProducts { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string? BrandCode { get; set; }
        public long? BrandPublicId { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? BrandName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? BrandDescription { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? Type { get; set; }
        [Column(TypeName = "nvarchar(400)")]
        public string? Slug { get; set; }
        public long? MedicineDosageFormId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? MedicineDosageFormName { get; set; } // Dosage Form
        [Column(TypeName = "nvarchar(15)")]
        public string? MedicineDosageFormShortForm { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? MedicineDosageFormDescription { get; set; }
        public long? GenericId { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? GenericName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? GenericDescription { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Strength { get; set; }
        public long? MeasurementUnitId { get; set; }
        [Column(TypeName = "nchar(4)")]
        public string? MeasurementUnitCode { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MeasurementUnitName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? MeasurementUnit { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MeasurementUnitDetails { get; set; }
        [Column(TypeName = "nvarchar(1000)")]

        public string? MeasurementUnitDescription { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? MeasurementUnitType { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? UnitPriceValue { get; set; }
        public long? UnitPriceId { get; set; }
        [Column(TypeName = "nchar(4)")]
        public string? UnitPriceCode { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? UnitPriceName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? PriceUnitDetails { get; set; }
        [Column(TypeName = "nvarchar(1000)")]

        public string? PriceUnitDescription { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? UnitPriceType { get; set; }
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
        public string? Interaction { get; set; }
        public string? OverdoseEffects { get; set; }
        public string? TherapeuticClass { get; set; }
        public string? StorageCondition { get; set; }
        public string? UserFor { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? CompanyDiscount { get; set; }
        public bool IsActive { get; set; }
        public string? WishList { get; set; }

        public bool? Wished { get; set; }

    }
}
