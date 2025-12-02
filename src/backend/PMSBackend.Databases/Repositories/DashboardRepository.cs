using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Databases.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PMSDbContext _dbContext;

        public DashboardRepository(PMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<DashboardSummaryContract> GetDashboardSummaryAsync(long userId, long? patientId, CancellationToken cancellationToken)
        {
            // Build base query for patients filtered by userId
            var patientQuery = _dbContext.Smartrx_PatientProfile
                .AsNoTracking()
                .Where(p => p.UserId == userId);
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                patientQuery = patientQuery.Where(p => p.Id == patientId.Value && !p.IsDeleted);
            }
            
            var totalPatients = await patientQuery.CountAsync(cancellationToken);

            // Build base query for doctors filtered by userId
            var doctorQuery = _dbContext.Smartrx_Doctor
                .AsNoTracking()
                .Where(m=>!m.PatientDoctor.IsDeleted)
                .Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => m.Id == d.SmartRxMasterId && m.UserId == userId));
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                doctorQuery = doctorQuery.Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => 
                    m.Id == d.SmartRxMasterId && 
                    m.UserId == userId && 
                    m.PatientId == patientId.Value));
            }
            
            var totalDoctors = await doctorQuery
                .Select(d => d.DoctorId)
                .Distinct()
                .CountAsync(cancellationToken);

            var totalDoctorsFee = await doctorQuery
              .SumAsync(d => d.ChamberFee ?? 0, cancellationToken);

            // Build base query for SmartRx filtered by userId
            var smartRxQuery = _dbContext.Smartrx_Master
                .AsNoTracking()
                .Where(m => m.UserId == userId && m.IsRecommended == true && m.IsApproved == true && m.IsCompleted == true);
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                smartRxQuery = smartRxQuery.Where(m => m.PatientId == patientId.Value);
            }
            
            var totalSmartRx = await smartRxQuery.CountAsync(cancellationToken);

            // Build base query for pending prescriptions filtered by userId
            var pendingQuery = _dbContext.Prescription_UploadedPrescription
                .AsNoTracking()
                .Where(p => p.UserId == userId && p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false) && !p.IsDeleted);
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                pendingQuery = pendingQuery.Where(p => p.PatientId == patientId.Value && !p.IsDeleted && !p.PatientProfile!.IsDeleted);
            }
            
            var totalPending = await pendingQuery.CountAsync(cancellationToken);

            // Build base query for Rx file only filtered by userId
            //var rxFileOnlyQuery = _dbContext.Prescription_UploadedPrescription
            //    .AsNoTracking()
            //    .Where(p => p.UserId == userId
            //        && !(_dbContext.Smartrx_Master.AsNoTracking().Any(m => m.PrescriptionId == p.Id && m.IsRecommended == true && m.IsApproved == true && m.IsCompleted == true))
            //        && !(p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)));

            var rxFileOnlyQuery = _dbContext.Prescription_UploadedPrescription
              .AsNoTracking()
              .Where(p => p.UserId == userId                 
                  && !(p.IsSmartRxRequested !=null && p.IsSmartRxRequested == true && (p.IsCompleted == null || p.IsCompleted == false)) && !p.IsDeleted);

            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                rxFileOnlyQuery = rxFileOnlyQuery.Where(p => p.PatientId == patientId.Value && !p.IsDeleted && !p.PatientProfile!.IsDeleted);
            }
            
            var totalRxFileOnly = await rxFileOnlyQuery.CountAsync(cancellationToken);

            var totalEdex = 0; // Placeholder           
          

            // Build base query for medicines filtered by userId
            var medicineQuery = _dbContext.SmartRx_PatientMedicine
                .AsNoTracking()
                .Where(m => _dbContext.Smartrx_Master.AsNoTracking().Any(s => s.Id == m.SmartRxMasterId && s.UserId == userId));
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                medicineQuery = medicineQuery.Where(m => _dbContext.Smartrx_Master.AsNoTracking().Any(s => 
                    s.Id == m.SmartRxMasterId && 
                    s.UserId == userId && 
                    s.PatientId == patientId.Value));
            }
            
            var totalMedicinesCost = await medicineQuery.SumAsync(
                m =>
                    ((m.Medicine != null ? (m.Medicine.UnitPrice ?? 0m) : 0m)) * m.DurationOfContinuationCount *
                    (
                        m.Dose1InADay +
                        m.Dose2InADay +
                        m.Dose3InADay +
                        m.Dose4InADay +
                        m.Dose5InADay +
                        m.Dose6InADay +
                        m.Dose7InADay +
                        m.Dose8InADay +
                        m.Dose9InADay +
                        m.Dose10InADay +
                        m.Dose11InADay +
                        m.Dose12InADay
                    ),
                cancellationToken);

            // Build base query for tests filtered by userId
            var testQuery = _dbContext.SmartRx_PatientInvestigation
                .AsNoTracking()
                .Where(i => _dbContext.Smartrx_Master.AsNoTracking().Any(s => s.Id == i.SmartRxMasterId && s.UserId == userId));
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                testQuery = testQuery.Where(i => _dbContext.Smartrx_Master.AsNoTracking().Any(s => 
                    s.Id == i.SmartRxMasterId && 
                    s.UserId == userId && 
                    s.PatientId == patientId.Value));
            }
            
            var totalTestsCost = await testQuery.SumAsync(i => i.TestPrice ?? 0m, cancellationToken);

            // Build base query for transport cost filtered by userId
            var transportCostQuery = _dbContext.Smartrx_Doctor
                .AsNoTracking()
                .Where(m=>!m.PatientDoctor.IsDeleted)
                .Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => m.Id == d.SmartRxMasterId && m.UserId == userId));
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                transportCostQuery = transportCostQuery.Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => 
                    m.Id == d.SmartRxMasterId && 
                    m.UserId == userId && 
                    m.PatientId == patientId.Value));
            }
            
            var totalTransportCost = await transportCostQuery.SumAsync(d => d.TransportExpense ?? 0, cancellationToken);

            // Build base query for other costs filtered by userId
            var otherCostQuery = _dbContext.Smartrx_Doctor
                .AsNoTracking()
                .Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => m.Id == d.SmartRxMasterId && m.UserId == userId));
            
            // Apply patientId filter if provided
            if (patientId.HasValue)
            {
                otherCostQuery = otherCostQuery.Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => 
                    m.Id == d.SmartRxMasterId && 
                    m.UserId == userId && 
                    m.PatientId == patientId.Value));
            }
            
            var totalOtherCosts = await otherCostQuery.SumAsync(d => d.OtherExpense ?? 0, cancellationToken);

            return new DashboardSummaryContract
            {
                UserSummary = new DashboardUserSummaryContract
                {
                    UserId = userId,
                    TotalPatients = totalPatients,
                    TotalDoctors = totalDoctors,
                    TotalRxFileOnly = totalRxFileOnly,
                    TotalSmartRx = totalSmartRx,
                    TotalPending = totalPending,
                    TotalEdex = totalEdex
                },
                ExpenseSummary = new DashboardExpenseSummaryContract
                {
                    UserId = userId,
                    TotalDoctorsFee = totalDoctorsFee,
                    TotalMedicinesCost = totalMedicinesCost,
                    TotalTestsCost = totalTestsCost,
                    TotalTransportCost = totalTransportCost,
                    TotalOtherCosts = totalOtherCosts
                }
            };
        }
        public async Task<DashboardCountsContract> GetDashboardSummaryCountAsync(long userId, long? patientId, CancellationToken cancellationToken)
        {
            try
            {
                // Get total prescription count from Prescription_UploadedPrescription table
                // Filter by userId (required) and optionally by patientId
                var prescriptionQuery = _dbContext.Prescription_UploadedPrescription
                    .AsNoTracking()
                    .Where(p => p.UserId == userId);
                
                if (patientId.HasValue)
                {
                    prescriptionQuery = prescriptionQuery.Where(p => p.PatientId == patientId.Value);
                }
                
                var totalPrescriptionCount = await prescriptionQuery.CountAsync(cancellationToken);

                // Get total active patient count from Smartrx_PatientProfile table
                // Filter by userId (required) and optionally by patientId
                var patientQuery = _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .Where(p => p.IsActive == true && p.UserId == userId);
                
                if (patientId.HasValue)
                {
                    patientQuery = patientQuery.Where(p => p.Id == patientId.Value);
                }
                
                var totalActivePatientCount = await patientQuery.CountAsync(cancellationToken);

                // Get total active doctor count from Smartrx_Doctor table
                // Filter by userId (required) and optionally by patientId through Smartrx_Master
                var doctorQuery = _dbContext.Smartrx_Doctor
                    .AsNoTracking()
                    .Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => m.Id == d.SmartRxMasterId && m.UserId == userId));
                
                if (patientId.HasValue)
                {
                    doctorQuery = doctorQuery.Where(d => _dbContext.Smartrx_Master.AsNoTracking().Any(m => 
                        m.Id == d.SmartRxMasterId && 
                        m.UserId == userId && 
                        m.PatientId == patientId.Value));
                }
                
                var totalActiveDoctorCount = await doctorQuery
                    .Select(d => d.DoctorId)
                    .Distinct()
                    .CountAsync(cancellationToken);

                // Get total patient count for inputted vital from Smartrx_Vital table
                // Filter by userId through Smartrx_Master and optionally by patientId
                var vitalQuery = _dbContext.Smartrx_Vital
                    .AsNoTracking()
                    .Where(v => _dbContext.Smartrx_Master.AsNoTracking().Any(m => m.Id == v.SmartRxMasterId && m.UserId == userId));
                
                if (patientId.HasValue)
                {
                    vitalQuery = vitalQuery.Where(v => v.PatientId == patientId.Value);
                }

                int totalPatientCountForVital = await vitalQuery
                    .Select(v => v.PatientId)
                    .Distinct()
                    .CountAsync(cancellationToken);

                return new DashboardCountsContract
                {
                    TotalPrescriptionCount = totalPrescriptionCount,
                    TotalActivePatientCount = totalActivePatientCount,
                    TotalActiveDoctorCount = totalActiveDoctorCount,
                    TotalPatientCountForVital = totalPatientCountForVital,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve dashboard counts: " + ex.Message);
            }
        }

       
    }
}