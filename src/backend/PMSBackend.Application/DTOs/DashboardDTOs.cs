using System;

namespace PMSBackend.Application.DTOs
{
    public class DashboardUserSummaryDTO
    {
        public long UserId { get; set; }
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalRxFileOnly { get; set; }
        public int TotalSmartRx { get; set; }
        public int TotalPending { get; set; }
        public int TotalEdex { get; set; }
    }

    public class DashboardExpenseSummaryDTO
    {
        public long UserId { get; set; }
        public decimal TotalDoctorsCost { get; set; }
        public decimal TotalMedicinesCost { get; set; }
        public decimal TotalTestsCost { get; set; }
        public decimal TotalTransportCost { get; set; }
        public decimal TotalOtherCosts { get; set; }
    }

    public class DashboardSummaryDTO
    {
        public DashboardUserSummaryDTO UserSummary { get; set; } = new DashboardUserSummaryDTO();
        public DashboardExpenseSummaryDTO ExpenseSummary { get; set; } = new DashboardExpenseSummaryDTO();
    }
}


