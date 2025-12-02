namespace PMSBackend.Domain.SharedContract
{
    public class DashboardUserSummaryContract
    {
        public long UserId { get; set; }
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalRxFileOnly { get; set; }
        public int TotalSmartRx { get; set; }
        public int TotalPending { get; set; }
        public int TotalEdex { get; set; }
        public int TotalVital { get; set; }
    }
    
    public class DashboardExpenseSummaryContract
    {
        public long UserId { get; set; }
        public decimal TotalDoctorsFee { get; set; }
        public decimal TotalMedicinesCost { get; set; }
        public decimal TotalTestsCost { get; set; }
        public decimal TotalTransportCost { get; set; }
        public decimal TotalOtherCosts { get; set; }
    }

    public class DashboardSummaryContract
    {
        public DashboardUserSummaryContract UserSummary { get; set; } = new DashboardUserSummaryContract();
        public DashboardExpenseSummaryContract ExpenseSummary { get; set; } = new DashboardExpenseSummaryContract();
    }
}


