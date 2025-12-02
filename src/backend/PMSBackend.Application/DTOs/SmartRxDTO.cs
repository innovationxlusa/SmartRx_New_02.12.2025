using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxDTO
    {
        public long SmartRxId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public SmartRxPatientProfile? PatientInfo { get; set; }
        public IList<SmartRxPrescription>? Prescriptions { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }
        public SmartRxOverviewDTO? SmartRxOverview { get; set; }
        public long UserId { get; set; }
        public long LoginUserId { get; set; }
        public long PatientId { get; set; }
        public string? Remarks { get; set; }
        public bool? IsExistingPatient { get; set; }
        public bool? HasAnyRelative { get; set; }
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? Tag3 { get; set; }
        public string? Tag4 { get; set; }
        public string? Tag5 { get; set; }
        public decimal MedicineTotalExpense { get; set; }
        public decimal InvestigationTotalExpense { get; set; }


    }
}
