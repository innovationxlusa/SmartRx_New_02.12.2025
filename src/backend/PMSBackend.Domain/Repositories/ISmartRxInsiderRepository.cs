using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Domain.Repositories
{
    public interface ISmartRxInsiderRepository
    {
        Task<SmartRx_MasterEntity?> GetDetailsBySmartRxIdAsync(long id);
        Task<SmartRx_PatientProfileEntity?> GetPatientProfileById(long id);

        Task<IList<Prescription_UploadEntity>>? GetAllPrescriptionOfOnePatientBySmartRxIdAsync(long smartRxId);
        Task<IList<SmartRx_PatientChiefComplaintEntity>?> GetPatientChiefComplaintListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);
        Task<PatientDoctorContract?> GetPatientOnePrescriptionDoctorBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken);
        Task<IList<SmartRx_PatientDoctorEntity>?> GetPatientDoctorsListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);

        Task<PatientDoctorContract> UpdateSmartRxDoctorReviewByUser(long smartRxId, long prescriptionId, long doctorId, int? travelTimeMinute,
             int? chamberWaitTimeHour, int? chamberWaitTimeMinute, int? doctorConsultingDuration, decimal? feeCharged, long? chamberFeeMeasurementUnitId, decimal? transportExpense, decimal? otherExpense, decimal? rating, string? comments, long loginUserId, CancellationToken cancellationToken);

        Task<IList<SmartRx_PatientHistoryEntity>?> GetPatientHistoryListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);
        Task<IList<SmartRx_PatientVitalsEntity>?> GetPatientVitalListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);

        Task<Configuration_ChiefComplaintEntity?> GetChiefComplaintDetailsById(long id);
        Task<IList<Configuration_SmartRxAcronymEntity>?> GetPatientChiefComplaintAcronymBySmartRxId(string acronym, string abbreviation, CancellationToken cancellationToken);
        //Task<IList<SmartRx_PatientMedicineEntity>?> GetPatientMedicineListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);
        Task<MedicineWithWishlistContract?> GetPatientMedicineListBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken);

        Task<IList<Configuration_VitalFAQEntity>?> GetPatientSingleVitalFAQByVitalId(long vitalId);
        Task<IList<Configuration_MedicineFAQEntity>?> GetPatientSingleFAQDrugInformationByIdAsync(long medicineId);
        Task<IList<Configuration_InvestigationFAQEntitiy>?> GetPatientSingleTestFAQByIdAsync(long investigationId);
        //Task<IList<SmartRx_PatientInvestigationEntity>?> GetPatientInvestigationListBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken);
        Task<InvestigationWithWishlistContract?> GetPatientInvestigationListBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken);
        Task<IList<SmartRx_PatientWishlistEntity>?> GetPatientInvestigationWishListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);
        Task<IList<SmartRx_PatientAdviceEntity>?> GetPatientAdviceListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken);
        Task<IList<Configuration_AdviceFAQEntity>?> GetPatientAdviceFAQListBySmartRxId(long smartRxId, long? prescriptionId);

        Task<SmartRx_PatientMedicineEntity> GetPatientSingleMedicineBySmartRxId(long smartRxId, long? prescriptionId, long? medicineId, CancellationToken cancellationToken);
        Task<SmartRx_PatientMedicineEntity?> GetMedicineForWishListAsync(long smartrxId, long prescriptionId, long medicineId);
        //Task<IList<SmartRx_PatientWishlistEntity>?> UpdateMedicineWishListAsync(long smartRxId, long prescriptionId, long medicineId, long loginUserId, List<long> wishlistMedicneIds);
        //Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetAllTestCenterWithPatientTestList(long smartRxMasterId, long prescriptionId, string? testCenterName, List<long> doctorsTestList, bool isDoctorRecommended, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken);
        //Task<IList<DiagnosticCenterWiseTestContract>> GetAllTestWithPatientsSelectedCenterList(List<long> doctorsTestList, long smartrxId, long prescriptionId, bool isDoctorRecommended, CancellationToken cancellationToken);
        Task<IEnumerable<DiagnosticCenterWiseTestContract>> GetAllTestWithPatientsSelectedCenterList(List<long> doctorsTestList, long smartrxId, long prescriptionId, bool isDoctorRecommended, CancellationToken cancellationToken);
        Task<IList<SmartRx_PatientInvestigationEntity>> AddOrEditPatientInvestigationAsync(List<SmartRx_PatientInvestigationEntity> smartRxPatientTestSelectionList, long smartRxId, long prescriptionId, long loginUserId);
        Task<IList<TestCenterContract>> GetAllTestCenter(CancellationToken cancellationToken);
        Task<IList<MedicineContract>> GetAllMedicine(CancellationToken cancellationToken);
        Task<SmartRx_PatientInvestigationEntity> GetPatientSingleInvestigationBySmartRxId(long smartRxId, long? prescriptionId, long? investigationId, CancellationToken cancellationToken);
        Task<SmartRx_PatientInvestigationEntity> UpdateInvestigationWishlistAsync(SmartRx_PatientInvestigationEntity entity, CancellationToken cancellationToken);
        Task<SmartRx_PatientMedicineEntity> UpdateMedicineWishlistAsync(SmartRx_PatientMedicineEntity entity, CancellationToken cancellationToken);
        //Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetHospitalsWithBranchAndTestsAsync(string? testCenterName, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken);
        Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetHospitalsWithBranchAndTestsAsync(string? testCenterName, long smartRxMasterId, long prescriptionId, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken);
        Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetAllTestCenterWithPatientTestList(long smartRxMasterId, long prescriptionId, string? testCenterName, List<long> doctorsTestList, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken);

        // Get all SmartRx with vitals by user ID
        Task<PaginatedResult<SmartRxWithVitalsContract>> GetAllSmartRxWithVitalsByUserIdWithPagingAsync(long userId, long? PatientId, string? vitalName, string? searchKeyword, string? searchColumn, DateTime? fromDate, DateTime? toDate, PagingSortingParams pagingSorting, CancellationToken cancellationToken);
    }
}