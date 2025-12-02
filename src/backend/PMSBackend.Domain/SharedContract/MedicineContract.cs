namespace PMSBackend.Domain.SharedContract
{
    public class MedicineContract
    {
        public long? Id { get; set; }
        public long? MedicineId { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public bool? Wished { get; set; }
    }
}
