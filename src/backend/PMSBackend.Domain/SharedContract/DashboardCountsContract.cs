namespace PMSBackend.Domain.SharedContract
{
    public class DashboardCountsContract
    {
        public int TotalPrescriptionCount { get; set; }
        public int TotalActivePatientCount { get; set; }
        public int TotalActiveDoctorCount { get; set; }
        public int TotalPatientCountForVital { get; set; }
    }
}