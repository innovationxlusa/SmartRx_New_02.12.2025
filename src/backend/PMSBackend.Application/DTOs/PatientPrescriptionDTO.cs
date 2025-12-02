namespace PMSBackend.Application.DTOs
{
    public class PatientPrescriptionDTO
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public int TotalPrescriptions { get; set; }
        public int SmartRxCount { get; set; }
        public bool IsSmartRx { get; set; }
        public int WaitingCount { get; set; }
        public bool IsWaiting { get; set; }

        public int FileOnlyCount { get; set; }
        public bool IsUncategorized { get; set; }

        public DateTime? LastPrescriptionDate { get; set; }
        public string LastPrescriptionDateStr { get; set; }
        public List<PrescriptionDTO> Prescriptions { get; set; } = new();
    }
}
