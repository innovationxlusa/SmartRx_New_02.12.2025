namespace PMSBackend.Application.DTOs
{
    public class SmartRxVital
    {
        public long Id { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long VitalId { get; set; }
        public decimal VitalValue { get; set; }
        public string VitalValueString { get; set; }
        public string VitalValueStandardString { get; set; }
        public string VitalValueOverWeightString { get; set; }
        public decimal? VitalMidNextRange { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ApplicableEntity { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public string MeasurementUnit { get; set; }
        public string? MeasurementUnitDetails { get; set; }
        public string? Status { get; set; }
        //public string? VitalRangeDetails { get; set; }




    }
}
