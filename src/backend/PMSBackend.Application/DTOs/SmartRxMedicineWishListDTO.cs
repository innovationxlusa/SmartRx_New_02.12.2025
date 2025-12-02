namespace PMSBackend.Application.DTOs
{
    public class SmartRxMedicineWishListDTO
    {
        public long? Id { get; set; }
        public long? MedicineId { get; set; }
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public bool? Wished { get; set; }
        public string? MedicineName { get; set; }
        public Dictionary<long, string>? WishedMedicines { get; set; }

    }
}
