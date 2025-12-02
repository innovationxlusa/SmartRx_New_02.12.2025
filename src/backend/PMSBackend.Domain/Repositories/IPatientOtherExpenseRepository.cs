using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface IPatientOtherExpenseRepository
    {
        Task<PatientOtherExpenseContract?> GetOtherExpenseByIdAsync(long id, CancellationToken cancellationToken);
        Task<IList<PatientOtherExpenseContract>> GetOtherExpensesBySmartRxMasterIdAsync(long smartRxMasterId, CancellationToken cancellationToken);
        Task<IList<PatientOtherExpenseContract>> GetOtherExpensesByPrescriptionIdAsync(long prescriptionId, CancellationToken cancellationToken);
        Task<PatientOtherExpenseContract> CreateOtherExpenseAsync(PatientOtherExpenseContract otherExpense, CancellationToken cancellationToken);
        Task<PatientOtherExpenseContract> UpdateOtherExpenseAsync(PatientOtherExpenseContract otherExpense, CancellationToken cancellationToken);
        Task<bool> DeleteOtherExpenseAsync(long id, CancellationToken cancellationToken);
        Task<IList<PatientOtherExpenseContract>> GetOtherExpensesByPatientIdAsync(long patientId, CancellationToken cancellationToken);
    }
}




