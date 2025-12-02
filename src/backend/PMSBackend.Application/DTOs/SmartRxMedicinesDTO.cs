using PMSBackend.Application.CommonServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxMedicinesDTO
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long MedicineId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string MedicineName { get; set; }
        public long MedicineBrandId { get; set; }
        public long MedicineManufacturerId { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string MedicineBrandCode { get; set; }
        public long MedicineBrandPublicId { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string MedicineBrandName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? MedicineBrandDescription { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string MedicineManufacturerName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? MedicineManufacturerOriginRegion { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? MedicineImporter { get; set; }
        public DateTime? MedicineManufacturingEstablishedDate { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? MedicineManufacturerProducts { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? MedicineManufacturerUrl { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineManufacturerLogoFileName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string? MedicineManufacturerLogoFilePath { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? MedicineManufacturerLogoFileExtension { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string MedicineType { get; set; }
        [Column(TypeName = "nvarchar(400)")]
        public string? MedicineSlug { get; set; }
        public long MedicineDosageFormId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string MedicineDosageFormName { get; set; } // Dosage Form
        [Column(TypeName = "nvarchar(2000)")]
        public string? MedicineDosageFormDescription { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string? MedicineShortForm { get; set; }
        public long MedicineGenericId { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string MedicineGenericName { get; set; }
        [Column(TypeName = "nvarchar(4000)")]
        public string? MedicineGenericDescription { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicineStrength { get; set; }
        public long? MedicineMeasurementUnitId { get; set; }

        [Column(TypeName = "nchar(4)")]
        public string MedicineMeasurementUnitCode { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MedicineMeasurementUnitName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MedicineMeasurementUnit { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicineMeasurementUnitDetails { get; set; }
        [Column(TypeName = "nvarchar(1000)")]

        public string? MedicineMeasurementUnitDescription { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string MedicineMeasurementUnitType { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MedicineUnitPrice { get; set; }
        public long? MedicinePriceInUnitId { get; set; }
        [Column(TypeName = "nchar(4)")]
        public string MedicinePriceInUnitCode { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MedicinePriceInUnitName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MedicinePriceInUnit { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicinePriceInUnitDetails { get; set; }
        [Column(TypeName = "nvarchar(1000)")]

        public string? MedicinePriceInUnitDescription { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string MedicinePriceInUnitType { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicinePackageType { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? MedicinePackageSize { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? MedicinePackageQuantity { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineDAR { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineIndication { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicinePharmacology { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicineDoseDescription { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineAdministration { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineContradiction { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? MedicineSideEffects { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicinePrecautionsAndWarnings { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? MedicinePregnencyAndLactation { get; set; }
        [Column(TypeName = "nvarchar(1)")]
        public string? MedicineModeOfAction { get; set; }
        public string? MedicineInteraction { get; set; }
        public string? MedicineOverdoseEffects { get; set; }
        public string? MedicineTherapeuticClass { get; set; }
        public string? MedicineStorageCondition { get; set; }
        public string? MedicineUserFor { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MedicineCompanyDiscount { get; set; }
        public bool MedicineIsActive { get; set; }
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
        public bool? Wished { get; set; }
        public IList<SmartRxMedicineWishListDTO> MedicineWishList { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
