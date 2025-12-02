using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.Linq;



namespace PMSBackend.Databases.Repositories
{
    public class SmartRxInsiderRepository : ISmartRxInsiderRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly IMedicineCompareRepository _medicineCompareRepository;

        public SmartRxInsiderRepository(PMSDbContext context, IMedicineCompareRepository medicineCompareRepository)
        {
            _dbContext = context;
            _medicineCompareRepository = medicineCompareRepository;
        }

        public async Task<IList<Prescription_UploadEntity>>? GetAllPrescriptionOfOnePatientBySmartRxIdAsync(long smartRxId)
        {
            try
            {
                var prescriptions = await _dbContext.Prescription_UploadedPrescription.Where(m => m.SmartRxId == smartRxId).ToListAsync();
                if (prescriptions == null)
                    return null;
                await Task.CompletedTask;
                return prescriptions!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SmartRx_PatientProfileEntity?> GetPatientProfileById(long id)
        {
            try
            {
                var result = await _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) return null;


                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<SmartRx_MasterEntity?> GetDetailsBySmartRxIdAsync(long smartRxId)
        {
            try
            {
                var result = await _dbContext.Smartrx_Master
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == smartRxId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<SmartRx_PatientChiefComplaintEntity>?> GetPatientChiefComplaintListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var chiefComplaints = await _dbContext.Smartrx_ChiefComplaint.Where(m => m.SmartRxMasterId == smartRxId && m.UploadedPrescriptionId == prescriptionId).ToListAsync(cancellationToken);
                if (chiefComplaints == null)
                    return null;               

                await Task.CompletedTask;
                return chiefComplaints!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IList<Configuration_SmartRxAcronymEntity>?> GetPatientChiefComplaintAcronymBySmartRxId(string acronym, string abbreviation, CancellationToken cancellationToken)
        {
            try
            {
                // First, get all acronyms from the database
                var allAcronyms = await _dbContext.Configuration_Acronym.ToListAsync(cancellationToken);

                if (allAcronyms == null || !allAcronyms.Any())
                    return null;

                // Filter on client-side to avoid EF translation issues with string.Split
                var filteredAcronyms = allAcronyms.Where(c =>
                {
                    // Case 1: both params null and both DB values null
                    if (string.IsNullOrEmpty(acronym) && string.IsNullOrEmpty(abbreviation))
                    {
                        return string.IsNullOrEmpty(c.Acronym) && string.IsNullOrEmpty(c.Abbreviation);
                    }

                    // Case 2: acronym check only
                    if (!string.IsNullOrEmpty(acronym) && string.IsNullOrEmpty(abbreviation))
                    {
                        if (string.IsNullOrEmpty(c.Acronym)) return false;
                        var acronymTokens = c.Acronym.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        return acronymTokens.Any(token => string.Equals(token, acronym, StringComparison.OrdinalIgnoreCase));
                    }

                    // Case 3: abbreviation check only
                    if (string.IsNullOrEmpty(acronym) && !string.IsNullOrEmpty(abbreviation))
                    {
                        if (string.IsNullOrEmpty(c.Abbreviation)) return false;
                        var abbreviationTokens = c.Abbreviation.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        return abbreviationTokens.Any(token => string.Equals(token, abbreviation, StringComparison.OrdinalIgnoreCase));
                    }

                    // Case 4: both acronym & abbreviation check
                    if (!string.IsNullOrEmpty(acronym) && !string.IsNullOrEmpty(abbreviation))
                    {
                        bool acronymMatch = false;
                        bool abbreviationMatch = false;

                        if (!string.IsNullOrEmpty(c.Acronym))
                        {
                            var acronymTokens = c.Acronym.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            acronymMatch = acronymTokens.Any(token => string.Equals(token, acronym, StringComparison.OrdinalIgnoreCase));
                        }

                        if (!string.IsNullOrEmpty(c.Abbreviation))
                        {
                            var abbreviationTokens = c.Abbreviation.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            abbreviationMatch = abbreviationTokens.Any(token => string.Equals(token, abbreviation, StringComparison.OrdinalIgnoreCase));
                        }

                        return acronymMatch || abbreviationMatch;
                    }

                    return false;
                }).ToList();

                await Task.CompletedTask;
                return filteredAcronyms;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PatientDoctorContract?> GetPatientOnePrescriptionDoctorBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                PatientDoctorContract docDTO = new PatientDoctorContract();
                var doctor = await _dbContext.Smartrx_Doctor
                             .AsNoTracking()
                             .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                             .Include(p => p.PatientDoctor)
                             //.Include(p => p.ChamberFeeMeasurementUnit) // will return null if not found
                             .FirstOrDefaultAsync(cancellationToken);
                if (doctor == null)
                    return null;

                docDTO.DoctorId = doctor.DoctorId;
                docDTO.SmartRxMasterId = smartRxId;
                docDTO.PrescriptionId = prescriptionId;
                docDTO.DoctorRating = doctor.DoctorRating;
                docDTO.ChamberFee = doctor.ChamberFee;
                docDTO.ChamberFeeMeasurementUnitId = 17;// doctor.ChamberFeeMeasurementUnitId;
                docDTO.ChamberFeeMeasurementUnit = "৳";// doctor.ChamberFeeMeasurementUnit!.Name;
                docDTO.ChamberWaitTimeHour = doctor.ChamberWaitTimeHour;
                docDTO.ChamberWaitTimeMinute = doctor.ChamberWaitTimeMinute;
                docDTO.ConsultingDurationInMinutes = doctor.ConsultingDurationInMinutes;
                docDTO.TravelTimeMinute = doctor.TravelTimeMinute ?? 0;
                docDTO.TransportFee = doctor.TransportExpense ?? 0;
                docDTO.OtherExpense = doctor.OtherExpense;
                docDTO.Comments = doctor.Comments;
                docDTO.PatientDoctor = new DoctorProfileContract()
                {
                    DoctorCode = doctor.PatientDoctor.Code,
                    DoctorTitle = doctor.PatientDoctor.Title,
                    DoctorFirstName = doctor.PatientDoctor.FirstName,
                    DoctorLastName = doctor.PatientDoctor.LastName,
                    ProfilePhotoName = doctor.PatientDoctor.ProfilePhotoName,
                    ProfilePhotoPath = doctor.PatientDoctor.ProfilePhotoPath,
                    DoctorSpecializedArea = doctor.PatientDoctor.SpecializedArea,
                    DoctorBMDCRegNo = doctor.PatientDoctor.BMDCRegNo,
                    DoctorProfessionalSummary = doctor.PatientDoctor.ProfessionalSummary,
                    DoctorId = doctor.PatientDoctor.Id,
                    DoctorRating = doctor.PatientDoctor.Rating,
                    DoctorYearOfExperiences = doctor.PatientDoctor.YearOfExperiences,
                    Comments = doctor.PatientDoctor.Comments
                };

                if (doctor.PatientDoctor.EducationDegreeIds is not null && doctor.PatientDoctor.EducationDegreeIds != string.Empty)
                {
                    docDTO.PatientDoctor.DoctorEducationDegreesStr = doctor.PatientDoctor.EducationDegreeIds;
                    IList<long> doctorEducationIds = doctor.PatientDoctor.EducationDegreeIds!
                          .Split(',', StringSplitOptions.RemoveEmptyEntries) // split by comma
                          .Select(id => long.Parse(id.Trim()))               // convert to long
                          .ToList();
                    var doctorEducationDegress = await _dbContext.Configuration_Education
                        .AsNoTracking()
                        .Where(e => doctorEducationIds.Contains(e.Id))
                        .ToListAsync(cancellationToken);
                    List<DoctorEducationContract> docEducations = new List<DoctorEducationContract>();
                    if (doctorEducationDegress != null)
                    {
                        foreach (var education in doctorEducationDegress)
                        {
                            var edu = new DoctorEducationContract()
                            {
                                EducationCode = education.Code,
                                EducationDegreeName = education.DegreeName,
                                EducationDescription = education.Description,
                                EducationId = education.Id,
                                EducationInstitutionName = education.InstitutionName,
                            };
                            docEducations.Add(edu);
                        }
                        docDTO.DoctorEducations = docEducations;
                    }
                }

                //if (doctor.PatientDoctor.ChamberIds is not null && doctor.PatientDoctor.ChamberIds != string.Empty)
                //{
                //    docDTO.PatientDoctor.DoctorChambersStr = doctor.PatientDoctor.ChamberIds;
                    List<DoctorChamberContract> doctorChamberList = new List<DoctorChamberContract>();
                    var doctorChambers = await _dbContext.Configuration_DoctorChamber
                    .AsNoTracking()
                        .Where(c => c.DoctorId == doctor.DoctorId)
                        .Include(c => c.Hospital)
                        .Include(c => c.Department)
                        .Include(c => c.DepartmentSection)
                        .Include(c => c.City)
                        .Include(c => c.DoctorDesignationInChamber)
                        .ToListAsync(cancellationToken);
                    if (doctorChambers is not null)
                    {
                        foreach (var chamber in doctorChambers)
                        {
                            var ch = new DoctorChamberContract()
                            {
                                ChamberId = chamber.Id,
                                ChamberName = chamber.ChamberName,
                                DepartmentSectionId = chamber.DepartmentSectionId,
                                DepartmentId = chamber.DepartmentId,
                                DepartmentName = chamber.Department?.Name,
                                ChamberAddress = chamber.ChamberAddress,
                                ChamberGoogleAddress = chamber.ChamberGoogleAddress,
                                ChamberCityId = chamber.ChamberCityId,
                                ChamberCityName = chamber.City?.Name,
                                ChamberDescription = chamber.ChamberDescription,
                                ChamberEmail = chamber.ChamberEmail!,
                                ChamberStartTime = chamber.ChamberStartTime!,
                                ChamberEndTime = chamber.ChamberEndTime!,
                                ChamberCloseDay = chamber.ChamberClosedOnDay!,
                                ChamberGoogleRating = chamber.ChamberGoogleRating!,
                                ChamberHelpline = chamber.Helpline_CallCenter!,
                                ChamberPostalCode = chamber.ChamberPostalCode,
                                ChamberVisitingHour = chamber.VisitingHour,
                                HospitalId = chamber.HospitalId,
                                HospitalName = chamber.Hospital?.Name,
                                ChamberDoctorBookingMobileNos = chamber.DoctorBookingMobileNos!,
                                ChamberOtherDoctorsId = chamber.ChamberOtherDoctorsId!,
                                DoctorId = chamber.DoctorId,
                                DoctorDesignationInChamberId = chamber.DoctorDesignationInChamberId,
                                DoctorDesignaitonInChamber = chamber.DoctorDesignationInChamber?.Name,
                                DoctorSpecialization = chamber.DoctorSpecialization,
                                IsMainChamber = chamber.IsMainChamber,
                                Remarks = chamber.Remarks!,
                                VisitingHour = chamber.VisitingHour,
                                DoctorVisitingDaysInChamber = chamber.DoctorVisitingDaysInChamber,
                                IsActive = chamber.IsActive,
                                HospitalLogoFileName = chamber.Hospital?.FileName,
                                HospitalLogoFilePath = chamber.Hospital?.FilePath,
                                HospitalLogoFileExtension = chamber.Hospital?.FileExtension
                            };
                            ch.DepartmentSectionName = chamber.DepartmentSection != null ? chamber.DepartmentSection.Name : string.Empty;
                            doctorChamberList.Add(ch);
                        }
                        docDTO.DoctorChambers = doctorChamberList;
                    }
               // }


                await Task.CompletedTask;
                return docDTO!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IList<SmartRx_PatientDoctorEntity>?> GetPatientDoctorsListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var doctors = await _dbContext.Smartrx_Doctor
                    .AsNoTracking()
                    .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                    .ToListAsync(cancellationToken);
                if (doctors == null)
                    return null;
                await Task.CompletedTask;
                return doctors!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PatientDoctorContract> UpdateSmartRxDoctorReviewByUser(long smartRxId, long prescriptionId, long doctorId, int? travelTimeMinute,
            int? chamberWaitTimeHour, int? chamberWaitTimeMinute, int? doctorConsultingDuration, decimal? feeCharged, long? chamberFeeMeasurementUnitId, decimal? transportExpense, decimal? otherExpense, decimal? rating, string? comments, long loginUserId, CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _dbContext.Smartrx_Doctor.FirstOrDefaultAsync(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId && m.DoctorId == doctorId, cancellationToken);
                if (doctor == null)
                    return null!;

                doctor.TravelTimeMinute = travelTimeMinute;
                doctor.ConsultingDurationInMinutes = doctorConsultingDuration;
                doctor.ChamberWaitTimeHour = chamberWaitTimeHour!;
                doctor.ChamberWaitTimeMinute = chamberWaitTimeMinute!;

                doctor.ChamberFee = feeCharged!;
                doctor.ChamberFeeMeasurementUnitId = chamberFeeMeasurementUnitId;
                doctor.TransportExpense = transportExpense;
                doctor.OtherExpense = otherExpense;

                doctor.DoctorRating = rating ?? 0;
                doctor.Comments = comments;
                doctor.ModifiedById = loginUserId; // Assuming 0 is the system user id
                doctor.ModifiedDate = DateTime.UtcNow;

                _dbContext.Entry(doctor).State = EntityState.Modified;
                _dbContext.Smartrx_Doctor.Update(doctor);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var updatedDoctor = await GetPatientOnePrescriptionDoctorBySmartRxId(smartRxId, prescriptionId, cancellationToken);
                if (updatedDoctor == null)
                    return null!;

                await Task.CompletedTask;
                return updatedDoctor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<SmartRx_PatientHistoryEntity>?> GetPatientHistoryListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var patientHistories = await _dbContext.Smartrx_PatientHistory
                    .AsNoTracking()
                    .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                    .ToListAsync(cancellationToken);
                if (patientHistories == null)
                    return null;
                await Task.CompletedTask;
                return patientHistories!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IList<SmartRx_PatientVitalsEntity>?> GetPatientVitalListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var patientVitals = await _dbContext.Smartrx_Vital.Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                                                   .Include(p => p.Vital)
                                                   .Include(pv => pv.Vital.Unit)
                                                   .ToListAsync(cancellationToken);
                if (patientVitals == null)
                    return null;

                await Task.CompletedTask;
                return patientVitals!;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IList<Configuration_VitalFAQEntity>?> GetPatientSingleVitalFAQByVitalId(long vitalId)
        {
            try
            {
                var patientVitalFAQ = await _dbContext.Configuration_VitalFAQ.Where(m => m.VitalId == vitalId).ToListAsync();

                await Task.CompletedTask;
                return patientVitalFAQ!;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<IList<Configuration_MedicineFAQEntity>?> GetPatientSingleFAQDrugInformationByIdAsync(long medicineId)
        {
            try
            {
                var patientMedicineFAQ = await _dbContext.Configuration_MedicineFAQ.Where(m => m.MedicineId == medicineId).ToListAsync();

                await Task.CompletedTask;
                return patientMedicineFAQ!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IList<Configuration_InvestigationFAQEntitiy>?> GetPatientSingleTestFAQByIdAsync(long investigationId)
        {
            try
            {
                var investigationMedicineFAQ = await _dbContext.Configuration_InvestigationFAQ.Where(m => m.InvestigationId == investigationId).ToListAsync();
                await Task.CompletedTask;
                return investigationMedicineFAQ!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Configuration_ChiefComplaintEntity?> GetChiefComplaintDetailsById(long id)
        {
            try
            {
                var result = await _dbContext.Configuration_ChiefComplaint.FirstOrDefaultAsync(c => c.Id == id);
                if (result == null) return null;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<MedicineWithWishlistContract?> GetPatientMedicineListBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                MedicineWithWishlistContract medicines = new MedicineWithWishlistContract();
                var patientMedicines = await _dbContext.SmartRx_PatientMedicine.Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                                                    .Include(p => p.Medicine)
                                                    .Include(pv => pv.Medicine.Brand)
                                                    .Include(pv => pv.Medicine.Brand.Manufacturer)
                                                    .Include(pv => pv.Medicine.MedicineDosageForm)
                                                    .Include(pv => pv.Medicine.Generic)
                                                    .Include(pv => pv.Medicine.MeasurementUnit)
                                                    .Include(pv => pv.Medicine.PriceUnit)
                                                    .ToListAsync(cancellationToken);
                if (patientMedicines == null)
                    return null;

                medicines.Medicines = patientMedicines;
                var wishListDetails = new List<MedicineWishlistContract>();
                foreach (var medicine in patientMedicines)
                {
                    if (!string.IsNullOrEmpty(medicine.Wishlist))
                    {
                        IList<long> wishedMedicineIds = medicine.Wishlist
                            .Split(',', StringSplitOptions.RemoveEmptyEntries) // split by comma
                            .Select(id => long.Parse(id.Trim()))               // convert to long
                            .ToList();
                        PagingSortingParams pagingAndSorting = new PagingSortingParams()
                        {
                            PageNumber = 0,
                            PageSize = 100,
                            SortBy = "name",
                            SortDirection = "asc"
                        };
                        var wishlist = new MedicineWishlistContract()
                        {
                            Id = medicine.Id,
                            MedicineId = medicine.MedicineId,
                            PrescriptionId = medicine.PrescriptionId,
                            SmartRxMasterId = medicine.SmartRxMasterId,
                            Wished = true,
                            MedicineName = medicine.Medicine.Name,
                        };
                        var sameGenericOtherBrandMedicineList = await _medicineCompareRepository.ListOfSameGenericOtherBrandOfAMedicine(smartRxId, prescriptionId, medicine.MedicineId, pagingAndSorting, cancellationToken);
                        if (sameGenericOtherBrandMedicineList is not null)
                        {
                            Dictionary<long, string> favourite = new Dictionary<long, string>();
                           
                            foreach (var wishedMedicineId in wishedMedicineIds)
                            {
                                var wishedMedicine = sameGenericOtherBrandMedicineList.Data.Where(w => w.MedicineId == wishedMedicineId).FirstOrDefault();
                                if (wishedMedicine is not null)
                                {
                                    favourite.Add(wishedMedicineId, wishedMedicine.MedicineName);
                                }
                            }
                            wishlist.WishedMedicines = favourite;
                            wishListDetails.Add(wishlist);                           
                        }
                    }
                }
                medicines.MedicineWishtlist = wishListDetails ?? new List<MedicineWishlistContract>();
                await Task.CompletedTask;
                return medicines!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InvestigationWithWishlistContract?> GetPatientInvestigationListBySmartRxId(long smartRxId, long prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                InvestigationWithWishlistContract investigations = new InvestigationWithWishlistContract();

                var patientInvestigations = await _dbContext.SmartRx_PatientInvestigation
                                            .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                                            .Include(pv => pv.InvestigationTest)
                                                .ThenInclude(pv => pv.PriceUnit)
                                            .Include(pv => pv.InvestigationTest.NationalPriceUnit)
                                            .Include(pv => pv.DiagnosticCenterWiseTest)
                                                .ThenInclude(x => x.DiagnosticTest)
                                                    .ThenInclude(x => x.PriceUnit)
                                            .Include(pv => pv.DiagnosticCenterWiseTest)
                                                .ThenInclude(x => x.DiagnosticTestCenter)
                                            .Include(pv => pv.DiagnosticCenterWiseTest.DiagnosticTest.NationalPriceUnit)
                                            .ToListAsync(cancellationToken);

                if (patientInvestigations == null)
                    return null;

                foreach (var investigation in patientInvestigations)
                {
                    string? userSelectedtestCenterStr = investigation.UserSelectedTestCenterIds!;
                    if (userSelectedtestCenterStr is not null)
                    {
                        string[] result = userSelectedtestCenterStr!.Split(',');

                        var testCenterList = _dbContext.Configuration_Hospital.Where(i => result.Contains(i.Id.ToString())).ToList();
                        investigation.UserSelectedTestCenters = testCenterList;
                    }
                    string? doctorRecommendedTestCenterStr = investigation.DoctorRecommendedTestCenterIds!;
                    if (doctorRecommendedTestCenterStr is not null)
                    {
                        string[] result = doctorRecommendedTestCenterStr!.Split(',');

                        var testCenterList = _dbContext.Configuration_Hospital.Where(i => result.Contains(i.Id.ToString())).ToList();
                        investigation.DoctorRecommendedTestCenters = testCenterList;
                    }
                }

                investigations.Tests = patientInvestigations;


                List<InvestigationWishlistContract> wishList = new List<InvestigationWishlistContract>();
                foreach (var test in patientInvestigations)
                {
                    if (!string.IsNullOrEmpty(test.Wishlist))
                    {
                        IList<long> wishedTestCenterIds = test.Wishlist
                            .Split(',', StringSplitOptions.RemoveEmptyEntries) // split by comma
                            .Select(id => long.Parse(id.Trim()))               // convert to long
                            .ToList();
                        PagingSortingParams pagingAndSorting = new PagingSortingParams()
                        {
                            PageNumber = 0,
                            PageSize = 100,
                            SortBy = "name",
                            SortDirection = "asc"
                        };
                        var wishlist = new InvestigationWishlistContract()
                        {
                            Id = test.Id,
                            PrescriptionId = test.PrescriptionId,
                            SmartRxMasterId = test.SmartRxMasterId,
                            Wished = true,
                            TestId = test.TestId,
                            TestName = test.InvestigationTest.TestName,
                            TestFullName = test.InvestigationTest.TestFullName,
                        };
                        var wishedTestCenterDetails = await _dbContext.Configuration_Hospital.Where(h => wishedTestCenterIds.Contains(h.Id)).ToListAsync();
                      
                        if (wishedTestCenterDetails is not null)
                        {
                            Dictionary<long, string> favourite = new Dictionary<long, string>();

                            foreach (var wishedTestCenter in wishedTestCenterDetails)
                            {
                                favourite.Add(wishedTestCenter.Id, wishedTestCenter.Name);
                            }
                            wishlist.WishedTestCenters = favourite;
                            wishList.Add(wishlist);

                        }
                    }
                }
                investigations.TestsWishtlist = wishList;

                await Task.CompletedTask;
                return investigations!;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IList<SmartRx_PatientWishlistEntity>?> GetPatientInvestigationWishListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var patientInvestigation = await _dbContext.SmartRx_PatientWishList
                                                .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId && m.WishListType == "Lab")
                                                .Include(pv => pv.InvestigationTest)
                                                 .ThenInclude(pv => pv.PriceUnit)
                                                .Include(pv => pv.InvestigationTest.NationalPriceUnit)
                                                .Include(pv => pv.RecommendedTestCenter)
                                                .ToListAsync(cancellationToken);

                if (patientInvestigation == null)
                    return null;

                await Task.CompletedTask;
                return patientInvestigation!;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public async Task<IList<SmartRx_PatientAdviceEntity>?> GetPatientAdviceListBySmartRxId(long smartRxId, long? prescriptionId, CancellationToken cancellationToken)
        {
            try
            {
                var patientAdvice = await _dbContext.SmartRx_PatientAdvice.Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                                                    .ToListAsync(cancellationToken);
                if (patientAdvice == null)
                    return null;

                await Task.CompletedTask;
                return patientAdvice!;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IList<Configuration_AdviceFAQEntity>?> GetPatientAdviceFAQListBySmartRxId(long smartRxId, long? prescriptionId)
        {
            try
            {
                var patientAdvice = await _dbContext.SmartRx_PatientAdvice.Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                                                    .ToListAsync();
                var adviceTags = patientAdvice.Select(data => data.AdviceKeywordToRecommend);
                List<Configuration_AdviceFAQEntity> adviceRecommendations = new List<Configuration_AdviceFAQEntity>();
                foreach (var advice in adviceTags)
                {
                    var tags = advice.Split(',');

                    var allFAQs = await _dbContext.Configuration_AdviceFAQ
                                                    .AsNoTracking()
                                                    .Where(m => m.TagSearchKeyword != null)
                                                    .ToListAsync();

                    var filteredFAQs = allFAQs
                                        .Where(m =>
                                        {
                                            var dbTags = m.TagSearchKeyword.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                                            return dbTags.Intersect(tags, StringComparer.OrdinalIgnoreCase).Any();
                                            // || dbTags.Any(t => t.Contains(advice, StringComparison.OrdinalIgnoreCase));
                                        })
                                        .ToList();
                 
                    adviceRecommendations.AddRange(filteredFAQs);
                }


                if (adviceRecommendations == null)
                    return null;

                await Task.CompletedTask;
                return adviceRecommendations!;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<SmartRx_PatientMedicineEntity?> GetMedicineForWishListAsync(long smartrxId, long prescriptionId, long medicineId)
        {
            try
            {
                var entity = await _dbContext.SmartRx_PatientMedicine
                    .AsNoTracking()
                    .Where(m => m.SmartRxMasterId == smartrxId && m.PrescriptionId == prescriptionId && m.MedicineId == medicineId)
                    .FirstOrDefaultAsync();

                await Task.CompletedTask;
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetHospitalsWithBranchAndTestsAsync(string? testCenterName, long smartRxMasterId, long prescriptionId, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken)
        {
            try
            {
                if (pagingAndSorting.PageNumber <= 0) pagingAndSorting.PageNumber = 1;
                if (pagingAndSorting.PageSize <= 0) pagingAndSorting.PageSize = 10;
                // Get all hospitals
                var hospitals = await _dbContext.Configuration_Hospital
                              .AsNoTracking()
                              .Where(h => string.IsNullOrEmpty(testCenterName) || h.Name.Contains(testCenterName))
                              .Select(h => new
                              {
                                  h.Id,
                                  h.Name,
                                  h.Location,
                                  h.Branch,
                                  h.Address,
                                  h.DiagnosticCenterIcon,
                                  h.GoogleRating,
                                  h.GoogleLocation,
                                  h.YearEstablished
                              })
                              .ToListAsync(cancellationToken);

                // Get all hospital-test mappings
                var hospitalTestMappings = await (
                    from dct in _dbContext.Configuration_DiagnosisCenterWiseTest
                        // join test in _dbContext.Configuration_Investigation on dct.TestId equals test.Id
                    select new
                    {
                        //HospitalId = dct.TestCenterId,
                        TestCenterId = dct.TestCenterId,
                        TestId = dct.TestId,
                        DiagnosticCenterGivenTestName = dct.DiagnosticCenterGivenTestName,
                        DiagnosticCenterGivenPrice = dct.DiagnosticCenterGivenPrice,
                        DiscountByAuthority = dct.DiscountByAuthority,
                    }
                ).ToListAsync(cancellationToken);

                var selectedTests = await _dbContext.SmartRx_PatientInvestigation
                 .Where(s => s.SmartRxMasterId == smartRxMasterId && s.PrescriptionId == prescriptionId)
                 .Select(s => new { s.TestId, s.UserSelectedTestCenterIds, s.DoctorRecommendedTestCenterIds, s.Wishlist })
                 .ToListAsync(cancellationToken);

                // ✅ Limit to hospitals that match the patient's selected test IDs
                var selectedTestIds = selectedTests.Select(s => s.TestId).Distinct().ToHashSet();

                var diagnosticCenterWiseTestList = (
                    from h in hospitals
                    join t in hospitalTestMappings on h.Id equals t.TestCenterId
                    where selectedTestIds.Contains(t.TestId)
                    select new { h, t }).AsEnumerable();

                List<DiagnosticCenterWiseTestContract> result = new List<DiagnosticCenterWiseTestContract>();

                foreach (var testcenter in diagnosticCenterWiseTestList)
                {
                    var wished = selectedTests.Any(st =>
                     st.TestId == testcenter.t.TestId &&
                     (st.Wishlist?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                         .Any(tid => tid == testcenter.t.TestId) ?? false)
                 );
                    var isUserSelected = selectedTests.Any(st =>
                       st.TestId == testcenter.t.TestId &&
                       (st.UserSelectedTestCenterIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                           .Any(tid => tid == testcenter.t.TestId) ?? false)
                   );
                    var isDoctorRecommended = selectedTests.Any(st =>
                       st.TestId == testcenter.t.TestId &&
                       (st.DoctorRecommendedTestCenterIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                           .Any(tid => tid == testcenter.t.TestId) ?? false)
                   );

                    var tc = new DiagnosticCenterWiseTestContract
                    {
                        DiagnosticTestCenterId = testcenter.h.Id,
                        DiagnosticTestCenterName = testcenter.h.Name,
                        DiagnosticTestCenterLocation = testcenter.h.Location ?? "Not Available",
                        DiagnosticTestCenterBranchName = testcenter.h.Branch ?? "Not Available",
                        DiagnosticTestCenterAddress = testcenter.h.Address ?? "",
                        DiagnosticTestCenterIcon = testcenter.h.DiagnosticCenterIcon,
                        DiagnosticeTestCenterTestId = testcenter.t.TestId,
                        DiagnosticTestCenterGoogleRating = testcenter.h.GoogleRating,
                        DiagnosticTestCenterGoogleLocation = testcenter.h.GoogleLocation ?? "Not Available",
                        DiagnosticTestCenterYearEstablished = testcenter.h.YearEstablished,
                        IsUserSelected = isUserSelected,
                        IsDoctorRecommended = isDoctorRecommended,
                        Wished = wished

                    };
                    result.Add(tc);
                }


                result = result
                                .GroupBy(x => new { x.DiagnosticTestCenterId })
                                .Select(g => g.First())
                                .ToList();

                var totalRecords = result.Count();

                IEnumerable<DiagnosticCenterWiseTestContract> sortedQuery;

                switch (pagingAndSorting.SortBy?.ToLower())
                {
                    case "rating":
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? result
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterGoogleRating))
                                .OrderByDescending(m => m.DiagnosticTestCenterGoogleRating)
                            : result
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterGoogleRating))
                                .OrderBy(m => m.DiagnosticTestCenterGoogleRating);
                        break;

                    case "yearofestablishment":
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? result
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterYearEstablished))
                                .OrderByDescending(m => m.DiagnosticTestCenterYearEstablished)
                            : result
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterYearEstablished))
                                .OrderBy(m => m.DiagnosticTestCenterYearEstablished);
                        break;

                    case "name":
                    default:
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? result.OrderByDescending(m => m.DiagnosticTestCenterName)
                            : result.OrderBy(m => m.DiagnosticTestCenterName);
                        break;
                }

                // ✅ Synchronous paging — data is in memory
                var pagedResult = sortedQuery
                    .Skip((pagingAndSorting.PageNumber - 1) * pagingAndSorting.PageSize)
                    .Take(pagingAndSorting.PageSize)
                    .ToList(); // ✅ Not async!

                return new PaginatedResult<DiagnosticCenterWiseTestContract>(
                    pagedResult,
                    totalRecords,
                    pagingAndSorting.PageNumber,
                    pagingAndSorting.PageSize,
                    pagingAndSorting.SortBy,
                    pagingAndSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<PaginatedResult<DiagnosticCenterWiseTestContract>> GetAllTestCenterWithPatientTestList(long smartRxMasterId, long prescriptionId, string? testCenterName, List<long> doctorsTestList, PagingSortingParams pagingAndSorting, CancellationToken cancellationToken)
        {
            try
            {
                if (pagingAndSorting.PageNumber <= 0) pagingAndSorting.PageNumber = 1;
                if (pagingAndSorting.PageSize <= 0) pagingAndSorting.PageSize = 10;

                // Step 1: Fetch all selected tests from DB
                var selectedTests = await _dbContext.SmartRx_PatientInvestigation
                    .Where(s => s.SmartRxMasterId == smartRxMasterId && s.PrescriptionId == prescriptionId)
                    .Select(s => new { s.TestId, s.UserSelectedTestCenterIds, s.DoctorRecommendedTestCenterIds, s.Wishlist })
                    .ToListAsync(cancellationToken);

                // Step 2: Get test + testcenter combo and filter in-memory
                var query = await (
                    from testcenter in _dbContext.Configuration_Hospital
                    join centerWiseTest in _dbContext.Configuration_DiagnosisCenterWiseTest
                        on testcenter.Id equals centerWiseTest.TestCenterId into centerWiseTests
                    from centerWiseTest in centerWiseTests.DefaultIfEmpty()
                    join test in _dbContext.Configuration_Investigation
                        on centerWiseTest.TestId equals test.Id into testJoin
                    from test in testJoin.DefaultIfEmpty()
                    where test != null
                        && doctorsTestList.Contains(test.Id)
                        && (string.IsNullOrEmpty(testCenterName) || test.TestFullName!.Contains(testCenterName))
                    select new { testcenter, centerWiseTest, test }
                ).ToListAsync(cancellationToken);


                var diagnosticCenterWiseTestList = query.Select(item =>
                {
                    // check if any selectedTest.TestCenterIds includes current testcenter.Id

                    var wished = selectedTests.Any(st =>
                        st.TestId == item.centerWiseTest.TestId &&
                        (st.Wishlist?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                            .Any(tid => tid == item.testcenter.Id) ?? false)
                    );
                    var isUserSelected = selectedTests.Any(st =>
                       st.TestId == item.centerWiseTest.TestId &&
                       (st.UserSelectedTestCenterIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                           .Any(tid => tid == item.testcenter.Id) ?? false)
                   );
                    var isDoctorRecommended = selectedTests.Any(st =>
                       st.TestId == item.centerWiseTest.TestId &&
                       (st.DoctorRecommendedTestCenterIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(id => long.TryParse(id, out var tid) ? tid : (long?)null)
                           .Any(tid => tid == item.testcenter.Id) ?? false)
                   );

                    return new DiagnosticCenterWiseTestContract
                    {
                        DiagnosticTestCenterId = item.testcenter.Id,
                        DiagnosticTestCenterCode = item.testcenter.Code,
                        DiagnosticTestCenterName = item.testcenter.Name,
                        DiagnosticTestCenterDescription = item.testcenter.Description,
                        DiagnosticTestCenterLocation = item.testcenter.Location ?? "Not Available",
                        DiagnosticTestCenterIcon = item.testcenter.DiagnosticCenterIcon,
                        DiagnosticTestCenterBranchName = item.testcenter.Branch ?? "Not Available",
                        DiagnosticTestCenterAddress = item.testcenter.Address ?? "",
                        DiagnosticTestCenterYearEstablished = item.testcenter.YearEstablished,
                        DiagnosticTestCenterGoogleRating = item.testcenter.GoogleRating,
                        DiagnosticTestCenterGoogleLocation = item.testcenter.GoogleLocation ?? "Not Available",
                        DiagnosticTestCenterIsActive = item.testcenter.IsActive,

                        DiagnosticTestCenterWiseTestId = item.centerWiseTest.Id,
                        DiagnosticeTestCenterTestId = item.test.Id,
                        DiagnosticeTestCenterTestCode = item.test.Code ?? "Not Available",
                        DiagnosticeTestCenterTestName = item.test.TestName ?? "Not Available",
                        DiagnosticeTestCenterTestDescription = item.test.TestDescription ?? "Not Available",
                        DiagnosticeTestCenterTestFullName = item.test.TestFullName ?? "Not Available",
                        DiagnosticeTestCenterTestShortName = item.test.TestShortName ?? "Not Available",
                        TestNameByDiagnosticCenter = item.test.TestNameByDiagnosticCenter ?? "Not Available",

                        TestUnitPrice = item.centerWiseTest.DiagnosticCenterGivenPrice ?? item.test.UnitPrice,
                        TestPriceMeasurementUnit = item.centerWiseTest.PriceUnit?.MeasurementUnit ?? item.test.PriceUnit?.MeasurementUnit ?? "Not Available",
                        TestPriceType = item.centerWiseTest.PriceUnit?.Type ?? item.test.PriceUnit?.Type ?? "Not Available",
                        DiagnosticeTestCenterWiseTestUnitPrice = item.centerWiseTest.DiagnosticCenterGivenPrice ?? item.centerWiseTest.DiagnosticTest.UnitPrice,
                        DiagnosticeTestCenterTestUnitPriceMeasurementUnit = item.centerWiseTest.DiagnosticTest.PriceUnit!.MeasurementUnit,
                        IsUserSelected = isUserSelected,
                        IsDoctorRecommended = isDoctorRecommended,
                        Wished = wished
                    };
                }).ToList();

                var totalRecords = diagnosticCenterWiseTestList.Count();

                IEnumerable<DiagnosticCenterWiseTestContract> sortedQuery;

                switch (pagingAndSorting.SortBy?.ToLower())
                {
                    case "rating":
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? diagnosticCenterWiseTestList
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterGoogleRating))
                                .OrderByDescending(m => m.DiagnosticTestCenterGoogleRating)
                            : diagnosticCenterWiseTestList
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterGoogleRating))
                                .OrderBy(m => m.DiagnosticTestCenterGoogleRating);
                        break;

                    case "yearofestablishment":
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? diagnosticCenterWiseTestList
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterYearEstablished))
                                .OrderByDescending(m => m.DiagnosticTestCenterYearEstablished)
                            : diagnosticCenterWiseTestList
                                .Where(m => !string.IsNullOrEmpty(m.DiagnosticTestCenterYearEstablished))
                                .OrderBy(m => m.DiagnosticTestCenterYearEstablished);
                        break;

                    case "name":
                    default:
                        sortedQuery = pagingAndSorting.SortDirection.ToLower() == "desc"
                            ? diagnosticCenterWiseTestList.OrderByDescending(m => m.DiagnosticTestCenterName)
                            : diagnosticCenterWiseTestList.OrderBy(m => m.DiagnosticTestCenterName);
                        break;
                }

                // ✅ Synchronous paging — data is in memory
                var pagedResult = sortedQuery
                    .Skip((pagingAndSorting.PageNumber - 1) * pagingAndSorting.PageSize)
                    .Take(pagingAndSorting.PageSize)
                    .ToList(); // ✅ Not async!

                return new PaginatedResult<DiagnosticCenterWiseTestContract>(
                    pagedResult,
                    totalRecords,
                    pagingAndSorting.PageNumber,
                    pagingAndSorting.PageSize,
                    pagingAndSorting.SortBy,
                    pagingAndSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IList<TestCenterContract>> GetAllTestCenter(CancellationToken cancellationToken)
        {
            try
            {
                var diagnosticCenterWiseTestList = await (from testcenter in _dbContext.Configuration_Hospital

                                                          join centerWiseTest in _dbContext.Configuration_DiagnosisCenterWiseTest
                                                              on testcenter.Id equals centerWiseTest.TestCenterId into centerWiseTests
                                                          from centerWiseTest in centerWiseTests.DefaultIfEmpty()

                                                          join test in _dbContext.Configuration_Investigation
                                                              on centerWiseTest != null ? centerWiseTest.TestId : 0 equals test.Id into tests
                                                          from test in tests.DefaultIfEmpty()
                                                          select new TestCenterContract
                                                          {
                                                              TestCenterId = testcenter.Id,
                                                              TestCenterName = testcenter.Name,
                                                              TestCenterBranch = testcenter.Branch!,
                                                              TestCenterLocation = testcenter.Location!,
                                                              TestCenterIcon = testcenter.DiagnosticCenterIcon!,
                                                              TestUnitPrice = centerWiseTest != null ? centerWiseTest.DiagnosticCenterGivenPrice : test.UnitPrice,
                                                              TestPriceMeasurementUnit = centerWiseTest != null ? centerWiseTest.PriceUnit!.MeasurementUnit : test.PriceUnit!.MeasurementUnit,
                                                          }
                                                ).ToListAsync(cancellationToken);
                return diagnosticCenterWiseTestList;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IList<MedicineContract>> GetAllMedicine(CancellationToken cancellationToken)
        {
            try
            {
                var diagnosticCenterWiseTestList = await (from medicine in _dbContext.SmartRx_PatientMedicine
                                                          select new MedicineContract
                                                          {
                                                              Id = medicine.Id,
                                                              MedicineId = medicine.MedicineId,
                                                              PrescriptionId = medicine.PrescriptionId,
                                                              SmartRxMasterId = medicine.SmartRxMasterId,
                                                              Wished = medicine.Wishlist == null ? false : true
                                                          }
                                                ).ToListAsync(cancellationToken);
                return new List<MedicineContract>();// diagnosticCenterWiseTestList;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<DiagnosticCenterWiseTestContract>> GetAllTestWithPatientsSelectedCenterList(List<long> doctorsTestList, long smartrxId, long prescriptionId, bool isDoctorRecommended, CancellationToken cancellationToken)
        {
            try
            {

                // Step 1: Fetch patient-selected tests and their test details
                var patientTests = await (
                    from patientSelectedTest in _dbContext.SmartRx_PatientInvestigation
                    join test in _dbContext.Configuration_Investigation
                        on patientSelectedTest.TestId equals test.Id
                    where patientSelectedTest.SmartRxMasterId == smartrxId
                          && patientSelectedTest.PrescriptionId == prescriptionId
                          && doctorsTestList.Contains(test.Id)
                    select new
                    {
                        patientSelectedTest,
                        test,
                        userRecommendedTestCenterIds = patientSelectedTest.UserSelectedTestCenterIds,
                        doctorRecommendedTestCenterIds = patientSelectedTest.DoctorRecommendedTestCenterIds,
                        WishList = patientSelectedTest.Wishlist
                    }
                ).ToListAsync(cancellationToken);

                // Step 2: Prepare and collect all test center IDs from all rows
                var allTestCenterIds = patientTests
                                      .SelectMany(pt => ParseIds(pt.userRecommendedTestCenterIds)
                                      .Concat(ParseIds(pt.doctorRecommendedTestCenterIds))
                                      .Concat(ParseIds(pt.WishList)))
                                      .Distinct()
                                      .ToList();

                // Step 3: Load all matching centers in one query
                var centersDict = await _dbContext.Configuration_Hospital
                                 .Where(tc => allTestCenterIds.Contains(tc.Id))
                                 .Include(tc => tc.City).ThenInclude(city => city.District)
                                 .Include(tc => tc.City).ThenInclude(city => city.Country)
                                 .ToDictionaryAsync(tc => tc.Id, cancellationToken);

                var result = new List<DiagnosticCenterWiseTestContract>();

                foreach (var pt in patientTests)
                {
                    var userRecommendedCenterIds = ParseIds(pt.userRecommendedTestCenterIds);
                    var doctorCenterIds = ParseIds(pt.doctorRecommendedTestCenterIds);
                    var wishList = ParseIds(pt.WishList);

                    var allCenterIds = userRecommendedCenterIds.Union(doctorCenterIds).Distinct();

                    foreach (var centerId in allCenterIds)
                    {
                        if (!centersDict.TryGetValue(centerId, out var testCenter)) continue;

                        var dto = new DiagnosticCenterWiseTestContract
                        {
                            SmartRxId = pt.patientSelectedTest.SmartRxMasterId,
                            PrescriptionId = pt.patientSelectedTest.PrescriptionId,
                            InvestigationId = pt.patientSelectedTest.Id,
                            DiagnosticeTestCenterTestId = pt.test.Id,
                            DiagnosticeTestCenterTestCode = pt.test.Code,
                            DiagnosticeTestCenterTestName = pt.test.TestName,
                            DiagnosticeTestCenterTestDescription = pt.test.TestDescription,
                            DiagnosticeTestCenterTestFullName = pt.test.TestFullName,
                            DiagnosticeTestCenterTestShortName = pt.test.TestShortName,
                            TestNameByDiagnosticCenter = pt.test.TestNameByDiagnosticCenter,
                            DiagnosticTestCenterId = testCenter.Id,
                            DiagnosticTestCenterCode = testCenter.Code ?? "Not Available",
                            DiagnosticTestCenterName = testCenter.Name ?? "Not Available",
                            DiagnosticTestCenterDescription = testCenter.Description ?? "Not Available",
                            DiagnosticTestCenterLocation = testCenter.Location ?? "Not Available",
                            DiagnosticTestCenterIcon = testCenter.DiagnosticCenterIcon,
                            DiagnosticTestCenterBranchName = testCenter.Branch ?? "Not Available",
                            DiagnosticTestCenterAddress = testCenter.Address ?? "",
                            DiagnosticTestCenterCityId = testCenter.CityId ?? 0,
                            DiagnosticTestCenterCityCode = testCenter.City?.Code ?? "Not Available",
                            DiagnosticTestCenterCityName = testCenter.City?.Name ?? "Not Available",
                            DiagnosticTestCenterDistrictId = testCenter.City?.DistrictId ?? 0,
                            DiagnosticTestCenterDistrictCode = testCenter.City?.District?.Code ?? "Not Available",
                            DiagnosticTestCenterDistrictName = testCenter.City?.District?.Name ?? "Not Available",
                            DiagnosticTestCenterDivisionId = testCenter.City?.District?.DivisionId ?? 0,
                            DiagnosticTestCenterCountryId = testCenter.City?.CountryId ?? 0,
                            DiagnosticTestCenterCountryCode = testCenter.City?.Country?.Code ?? "Not Available",
                            DiagnosticTestCenterCountryName = testCenter.City?.Country?.Name ?? "Not Available",
                            DiagnosticTestCenterYearEstablished = testCenter.YearEstablished,
                            DiagnosticTestCenterGoogleRating = testCenter.GoogleRating,
                            DiagnosticTestCenterGoogleLocation = testCenter.GoogleLocation ?? "Not Available",
                            DiagnosticTestCenterIsActive = testCenter.IsActive,
                            DiagnosticTestCenterWiseTestId = 0, // Placeholder if needed
                            TestUnitPrice = pt.test.UnitPrice,
                            TestPriceMeasurementUnit = pt.test.PriceUnit?.MeasurementUnit,
                            TestPriceType = pt.test.PriceUnit?.Type,
                            IsUserSelected = userRecommendedCenterIds.Contains(centerId),
                            IsDoctorRecommended = doctorCenterIds.Contains(centerId),
                            Wished = wishList.Contains(centerId)
                        };

                        result.Add(dto);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static IList<long> ParseIds(string csv)
        {
            return (csv ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => long.TryParse(id, out var i) ? i : (long?)null)
                .Where(i => i.HasValue)
                .Select(i => i.Value)
                .ToList();
        }


        public async Task<SmartRx_PatientMedicineEntity> GetPatientSingleMedicineBySmartRxId(long smartRxId, long? prescriptionId, long? medicineId, CancellationToken cancellationToken)
        {
            try
            {
                var patientMedicine = await _dbContext.SmartRx_PatientMedicine
                                            .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId && m.MedicineId == medicineId).FirstOrDefaultAsync(cancellationToken);

                await Task.CompletedTask;
                return patientMedicine!;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IList<SmartRx_PatientWishlistEntity>?> UpdateMedicineWishListAsync(long smartRxId, long prescriptionId, long medicineId, long loginUserId, List<long> wishlistMedicneIds)
        {
            try
            {

                var existingData = await _dbContext.SmartRx_PatientWishList
                              .Where(x => x.SmartRxMasterId == smartRxId && x.PrescriptionId == prescriptionId && x.PatientMedicineId == medicineId)  // Use filter to avoid full delete
                              .ToListAsync();

                _dbContext.SmartRx_PatientWishList.RemoveRange(existingData);
                List<SmartRx_PatientWishlistEntity> wishlist = new List<SmartRx_PatientWishlistEntity>();
                foreach (var singleWishlist in wishlistMedicneIds)
                {
                    bool exists = await _dbContext.Configuration_Medicine
                                        .AnyAsync(m => m.Id == singleWishlist);
                    if (exists)
                    {
                        var entity = new SmartRx_PatientWishlistEntity()
                        {
                            SmartRxMasterId = smartRxId,
                            PrescriptionId = prescriptionId,
                            PatientMedicineId = medicineId,
                            PatientWishlistMedicineId = singleWishlist,
                            CreatedById = loginUserId,
                            CreatedDate = DateTime.Now,
                        };
                        wishlist.Add(entity);
                    }
                }
                _dbContext.SmartRx_PatientWishList.AddRange(wishlist);
                await _dbContext.SaveChangesAsync();

                var updatedList = await _dbContext.SmartRx_PatientWishList
                    .Where(x => x.SmartRxMasterId == smartRxId && x.PrescriptionId == prescriptionId && x.PatientMedicineId == medicineId)
                    .ToListAsync();

                return updatedList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SmartRx_PatientMedicineEntity> UpdateMedicineWishlistAsync(
         SmartRx_PatientMedicineEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                var masterEntity = await _dbContext.Smartrx_Master.Where(data => data.Id == entity.SmartRxMasterId).FirstOrDefaultAsync();
                if (masterEntity is not null)
                {
                    masterEntity.HasMedicineFavourite = !string.IsNullOrEmpty(entity.Wishlist) ? true : false;
                    masterEntity.ModifiedBy = entity.ModifiedBy;
                    masterEntity.ModifiedDate = DateTime.Now;
                    _dbContext.Entry(masterEntity).State = EntityState.Modified;
                }
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(cancellationToken);
                _dbContext.Entry(entity).Reload();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<SmartRx_PatientInvestigationEntity?> GetPatientInvestigationTestCompareListAsync(long smartrxId, long prescriptionId)
        {
            try
            {
                var entity = await _dbContext.SmartRx_PatientInvestigation.Where(m => m.SmartRxMasterId == smartrxId && m.PrescriptionId == prescriptionId).FirstOrDefaultAsync();

                await Task.CompletedTask;
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<SmartRx_PatientInvestigationEntity>> AddOrEditPatientInvestigationAsync(List<SmartRx_PatientInvestigationEntity> smartRxPatientTestSelectionList, long smartRxId, long prescriptionId, long loginUserId)
        {
            try
            {
                var existingTestCenterList = await _dbContext.SmartRx_PatientInvestigation
                    .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                    .Include(p => p.PriceUnit)
                    .Include(p => p.InvestigationTest)
                    .ToListAsync();

                if (existingTestCenterList is not null && existingTestCenterList.Count > 0)
                {
                    foreach (var item in existingTestCenterList)
                    {
                        var centers = smartRxPatientTestSelectionList!
                            .Where(i => i.SmartRxMasterId == smartRxId && i.PrescriptionId == prescriptionId && i.Id == item.Id)
                            .FirstOrDefault();
                        // Update the test center IDs
                        item.UserSelectedTestCenterIds = centers == null ? null : centers.UserSelectedTestCenterIds;
                        item.ModifiedById = loginUserId;
                        item.ModifiedDate = DateTime.Now;

                        // Update the related test centers if UserSelectedTestCenterIds is not null or empty
                        if (!string.IsNullOrEmpty(item.UserSelectedTestCenterIds))
                        {
                            string[] result = item.UserSelectedTestCenterIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            if (result.Length > 0)
                            {
                                var testCenterIds = result.Select(long.Parse).ToList();
                                item.UserSelectedTestCenters = await _dbContext.Configuration_Hospital
                                    .Where(i => testCenterIds.Contains(i.Id))
                                    .ToListAsync();
                            }
                        }
                        else
                        {
                            item.UserSelectedTestCenters = new List<Configuration_HospitalEntity>();
                        }

                        // Mark the entity as modified
                        _dbContext.Entry(item).State = EntityState.Modified;
                    }
                }

                await _dbContext.SaveChangesAsync();
                
                // Reload the entities to get the updated data
                var updatedList = await _dbContext.SmartRx_PatientInvestigation
                    .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId)
                    .Include(p => p.PriceUnit)
                    .Include(p => p.InvestigationTest)
                    .ToListAsync();

                return updatedList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update patient investigation test centers: {ex.Message}", ex);
            }
        }

        public async Task<SmartRx_PatientInvestigationEntity> GetPatientSingleInvestigationBySmartRxId(long smartRxId, long? prescriptionId, long? investigationId, CancellationToken cancellationToken)
        {
            try
            {
                var patientInvestigation = await _dbContext.SmartRx_PatientInvestigation
                                            .Where(m => m.SmartRxMasterId == smartRxId && m.PrescriptionId == prescriptionId && m.Id == investigationId).FirstOrDefaultAsync(cancellationToken);

                await Task.CompletedTask;
                return patientInvestigation!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SmartRx_PatientInvestigationEntity> UpdateInvestigationWishlistAsync(
          SmartRx_PatientInvestigationEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                var masterEntity = await _dbContext.Smartrx_Master.Where(data => data.Id == entity.SmartRxMasterId).FirstOrDefaultAsync();
                if (masterEntity is not null)
                {
                    masterEntity.HasInvestigationFavourite = !string.IsNullOrEmpty(entity.Wishlist) ? true : false;
                    masterEntity.ModifiedBy = entity.ModifiedBy;
                    masterEntity.ModifiedDate = DateTime.Now;
                    _dbContext.Entry(masterEntity).State = EntityState.Modified;
                }
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(cancellationToken);
                _dbContext.Entry(entity).Reload();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<PaginatedResult<SmartRxWithVitalsContract>> GetAllSmartRxWithVitalsByUserIdWithPagingAsync(long userId, long? PatientId, string? vitalName, string? searchKeyword, string? searchColumn, DateTime? fromDate, DateTime? toDate, PagingSortingParams pagingSorting, CancellationToken cancellationToken)
        {
            try
            {
                // Validate and set default paging parameters
                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                // ========== STEP 1: INITIAL FILTERING ==========
                var preQuery = _dbContext.Smartrx_Master
                    .AsNoTracking()
                    .Where(sm => sm.UserId == userId &&
                                sm.IsRecommended == true &&
                                sm.IsApproved == true &&
                                sm.IsCompleted == true &&
                                sm.PatientVitals.Any());

                // Only active patient profiles
                preQuery = preQuery.Where(sm => sm.PatientProfile.IsActive == true);

                if (PatientId is not null && PatientId > 0)
                {
                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.PatientId == PatientId));
                }
                // Apply optional date range filters when provided
                if (fromDate is not null && fromDate != default(DateTime))
                {
                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.CreatedDate.Value.Date >= fromDate.Value.Date));
                }
                if (toDate is not null && toDate != default(DateTime))
                {
                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.CreatedDate.Value.Date <= toDate.Value.Date));
                }

                // Apply vital name filter when provided
                if (!string.IsNullOrWhiteSpace(vitalName))
                {
                    var vitalNameFilter = vitalName.Trim().ToLower();
                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.Vital.Name.ToLower().Contains(vitalNameFilter)));
                }

                // ========== STEP 2: SEARCH KEYWORD FILTERING ==========
                // Apply search filter only if SearchKeyword is provided
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    var searchTerm = searchKeyword.Trim().ToLower();
                    var maleMatch = searchTerm == "male" || searchTerm == "1";
                    var femaleMatch = searchTerm == "female" || searchTerm == "2";
                    // blood group flags
                    var aPosMatch = searchTerm == "a+" || searchTerm == "a positive" || searchTerm == "a_pos" || searchTerm == "a_positive" || searchTerm == "1";
                    var aNegMatch = searchTerm == "a-" || searchTerm == "a negative" || searchTerm == "a_neg" || searchTerm == "a_negative" || searchTerm == "2";
                    var bPosMatch = searchTerm == "b+" || searchTerm == "b positive" || searchTerm == "b_pos" || searchTerm == "b_positive" || searchTerm == "3";
                    var bNegMatch = searchTerm == "b-" || searchTerm == "b negative" || searchTerm == "b_neg" || searchTerm == "b_negative" || searchTerm == "4";
                    var abPosMatch = searchTerm == "ab+" || searchTerm == "ab positive" || searchTerm == "ab_pos" || searchTerm == "ab_positive" || searchTerm == "5";
                    var abNegMatch = searchTerm == "ab-" || searchTerm == "ab negative" || searchTerm == "ab_neg" || searchTerm == "ab_negative" || searchTerm == "6";
                    var oPosMatch = searchTerm == "o+" || searchTerm == "o positive" || searchTerm == "o_pos" || searchTerm == "o_positive" || searchTerm == "7";
                    var oNegMatch = searchTerm == "o-" || searchTerm == "o negative" || searchTerm == "o_neg" || searchTerm == "o_negative" || searchTerm == "8";

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        // If no searchColumn is provided or it's empty, search across all columns
                        if (string.IsNullOrWhiteSpace(searchColumn))
                        {
                            // Prioritize exact intent matches with if-else for clarity and performance
                            if (maleMatch || femaleMatch)
                            {
                                preQuery = preQuery.Where(sm =>
                                    (maleMatch && sm.PatientProfile.Gender == (int)Gender.Male) ||
                                    (femaleMatch && sm.PatientProfile.Gender == (int)Gender.Female));
                            }
                            else if (aPosMatch || aNegMatch || bPosMatch || bNegMatch || abPosMatch || abNegMatch || oPosMatch || oNegMatch)
                            {
                                preQuery = preQuery.Where(sm =>
                                    sm.PatientProfile.BloodGroup != null &&
                                    sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm));
                            }
                            else
                            {
                                // Break into individual debuggable sub-queries and union the results
                                var qFirstName = preQuery.Where(sm => sm.PatientProfile.FirstName != null && sm.PatientProfile.FirstName.ToLower().Contains(searchTerm));
                                var qLastName = preQuery.Where(sm => sm.PatientProfile.LastName != null && sm.PatientProfile.LastName.ToLower().Contains(searchTerm));
                                var qPatientCode = preQuery.Where(sm => sm.PatientProfile.PatientCode != null && sm.PatientProfile.PatientCode.ToLower().Contains(searchTerm));
                                var qNickName = preQuery.Where(sm => sm.PatientProfile.NickName != null && sm.PatientProfile.NickName.ToLower().Contains(searchTerm));
                                var qPhone = preQuery.Where(sm => sm.PatientProfile.PhoneNumber != null && sm.PatientProfile.PhoneNumber.ToLower().Contains(searchTerm));
                                var qEmail = preQuery.Where(sm => sm.PatientProfile.Email != null && sm.PatientProfile.Email.ToLower().Contains(searchTerm));
                                var qAddress = preQuery.Where(sm => sm.PatientProfile.Address != null && sm.PatientProfile.Address.ToLower().Contains(searchTerm));
                                var qEmergency = preQuery.Where(sm => sm.PatientProfile.EmergencyContact != null && sm.PatientProfile.EmergencyContact.ToLower().Contains(searchTerm));
                                var qProfession = preQuery.Where(sm => sm.PatientProfile.Profession != null && sm.PatientProfile.Profession.ToLower().Contains(searchTerm));
                                var qAge = preQuery.Where(sm => sm.PatientProfile.Age != null && sm.PatientProfile.Age.ToString().ToLower().Contains(searchTerm));
                                var qDob = preQuery.Where(sm => sm.PatientProfile.DateOfBirth != null && sm.PatientProfile.DateOfBirth.ToString().ToLower().Contains(searchTerm));
                                var qRemarks = preQuery.Where(sm => sm.Remarks != null && sm.Remarks.ToLower().Contains(searchTerm));
                                var qVitalName = preQuery.Where(sm => sm.PatientVitals.Any(v => v.Vital.Name != null && v.Vital.Name.ToLower().Contains(searchTerm)));
                                var qVitalValue = preQuery.Where(sm => sm.PatientVitals.Any(v => v.VitalValue != null && v.VitalValue.ToString().ToLower().Contains(searchTerm)));
                                var qVitalStatus = preQuery.Where(sm => sm.PatientVitals.Any(v => v.VitalStatus != null && v.VitalStatus.ToString().ToLower().Contains(searchTerm)));
                                var qCreatedDate = preQuery.Where(sm => sm.PatientVitals.Any(v => v.CreatedDate != null && v.CreatedDate.ToString().ToLower().Contains(searchTerm)));
                                var qHeight = preQuery.Where(sm => sm.PatientVitals.Any(v => v.HeightFeet != null && (v.HeightFeet.ToString() + "ft " + v.HeightFeet.ToString() + "in").ToLower().Contains(searchTerm)));

                                var unioned = qFirstName
                                    .Union(qLastName)
                                    .Union(qPatientCode)
                                    .Union(qNickName)
                                    .Union(qPhone)
                                    .Union(qEmail)
                                    .Union(qAddress)
                                    .Union(qEmergency)
                                    .Union(qProfession)
                                    .Union(qAge)
                                    .Union(qDob)
                                    .Union(qRemarks)
                                    .Union(qVitalName)
                                    .Union(qVitalValue)
                                    .Union(qVitalStatus)
                                    .Union(qCreatedDate)
                                    .Union(qHeight);

                                preQuery = unioned;
                            }
                        }
                        else
                        {
                            switch (searchColumn.ToLower())
                            {
                                case "gender":
                                    preQuery = preQuery.Where(sm =>
                                        (maleMatch && sm.PatientProfile.Gender == (int)Gender.Male) ||
                                        (femaleMatch && sm.PatientProfile.Gender == (int)Gender.Female));
                                    break;
                            case "firstname":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.FirstName != null && sm.PatientProfile.FirstName.ToLower().Contains(searchTerm));
                                break;
                            case "lastname":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.LastName != null && sm.PatientProfile.LastName.ToLower().Contains(searchTerm));
                                break;
                            case "patientcode":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.PatientCode != null && sm.PatientProfile.PatientCode.ToLower().Contains(searchTerm));
                                break;
                            case "nickname":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.NickName != null && sm.PatientProfile.NickName.ToLower().Contains(searchTerm));
                                break;
                            case "phonenumber":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.PhoneNumber != null && sm.PatientProfile.PhoneNumber.ToLower().Contains(searchTerm));
                                break;
                            case "email":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.Email != null && sm.PatientProfile.Email.ToLower().Contains(searchTerm));
                                    break;
                                case "address":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.Address != null && sm.PatientProfile.Address.ToLower().Contains(searchTerm));
                                    break;
                                case "emergencycontact":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.EmergencyContact != null && sm.PatientProfile.EmergencyContact.ToLower().Contains(searchTerm));
                                    break;
                                case "profession":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.Profession != null && sm.PatientProfile.Profession.ToLower().Contains(searchTerm));
                                    break;
                                case "age":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.Age != null && sm.PatientProfile.Age.ToString().ToLower().Contains(searchTerm));
                                    break;
                                case "birthdate":
                                case "dateofbirth":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.DateOfBirth != null && sm.PatientProfile.DateOfBirth.ToString().ToLower().Contains(searchTerm));
                                    break;
                                case "bloodgroup":
                                    preQuery = preQuery.Where(sm => sm.PatientProfile.BloodGroup != null && (
                                        sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm) ||
                                        (aPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (aNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (bPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (bNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (abPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (abNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (oPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                        (oNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm))
                                    ));
                                break;
                            case "remarks":
                                    preQuery = preQuery.Where(sm => sm.Remarks != null && sm.Remarks.ToLower().Contains(searchTerm));
                                break;
                            case "vitalname":
                                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.Vital.Name != null && v.Vital.Name.ToLower().Contains(searchTerm)));
                                break;
                            case "vitalvalue":
                                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.VitalValue != null && v.VitalValue.ToString().ToLower().Contains(searchTerm)));
                                    break; 
                                case "vitalstatus":
                                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.VitalStatus != null && v.VitalStatus.ToString().ToLower().Contains(searchTerm)));
                                    break;
                                case "createddate":
                                    preQuery = preQuery.Where(sm => sm.PatientVitals.Any(v => v.CreatedDate != null && v.CreatedDate.ToString().ToLower().Contains(searchTerm)));
                                break;
                            case "all":
                            default:
                                    preQuery = preQuery.Where(sm =>
                                        (sm.PatientProfile.FirstName != null && sm.PatientProfile.FirstName.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.LastName != null && sm.PatientProfile.LastName.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.PatientCode != null && sm.PatientProfile.PatientCode.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.NickName != null && sm.PatientProfile.NickName.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.PhoneNumber != null && sm.PatientProfile.PhoneNumber.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.Email != null && sm.PatientProfile.Email.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.Address != null && sm.PatientProfile.Address.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.EmergencyContact != null && sm.PatientProfile.EmergencyContact.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.Profession != null && sm.PatientProfile.Profession.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.Age != null && sm.PatientProfile.Age.ToString().ToLower().Contains(searchTerm)) ||
                                        (sm.PatientProfile.DateOfBirth != null && sm.PatientProfile.DateOfBirth.ToString().ToLower().Contains(searchTerm)) ||
                                        (maleMatch && sm.PatientProfile.Gender == (int)Gender.Male) ||
                                        (femaleMatch && sm.PatientProfile.Gender == (int)Gender.Female) ||
                                        // blood group checks (string field)
                                        (sm.PatientProfile.BloodGroup != null && (
                                            (aPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (aNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (bPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (bNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (abPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (abNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (oPosMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm)) ||
                                            (oNegMatch && sm.PatientProfile.BloodGroup.ToString().ToLower().Contains(searchTerm))
                                        )) ||
                                        (sm.Remarks != null && sm.Remarks.ToLower().Contains(searchTerm)) ||
                                        (sm.PatientVitals.Any(v => v.Vital.Name != null && v.Vital.Name.ToLower().Contains(searchTerm))) ||
                                        (sm.PatientVitals.Any(v => v.VitalValue != null && v.VitalValue.ToString().ToLower().Contains(searchTerm)) ||
                                        (sm.PatientVitals.Any(v => v.VitalStatus != null && v.VitalStatus.ToString().ToLower().Contains(searchTerm)) ||
                                        (sm.PatientVitals.Any(v => v.CreatedDate != null && v.CreatedDate.ToString().ToLower().Contains(searchTerm)) ||
                                        (sm.PatientVitals.Any(v => v.HeightFeet != null && (v.HeightFeet.ToString() + "ft " + v.HeightFeet.ToString() + "in").ToLower().Contains(searchTerm))

                                        )))));
                                break;
                            }
                        }
                    }
                }

                // ========== STEP 3: COUNT CALCULATION ==========
                // Get total count after all filtering is applied
                var totalRecords = await preQuery.CountAsync(cancellationToken);

                // ========== STEP 4: SORTING ==========
                // Apply sorting on the entity query after filtering but before projection
                var sortDirection = string.IsNullOrWhiteSpace(pagingSorting.SortDirection)
                    ? "desc"
                    : pagingSorting.SortDirection.Trim().ToLower();
                var sortBy = pagingSorting.SortBy?.ToLower();
                var sortDescending = sortDirection == "desc";

                IQueryable<SmartRx_MasterEntity> sortedPreQuery = preQuery;
                switch (sortBy)
                {
                    case "firstname":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PatientProfile.FirstName ?? string.Empty)
                            : preQuery.OrderBy(sm => sm.PatientProfile.FirstName ?? string.Empty);
                        break;
                    case "lastname":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PatientProfile.LastName ?? string.Empty)
                            : preQuery.OrderBy(sm => sm.PatientProfile.LastName ?? string.Empty);
                        break;
                    case "patientcode":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PatientProfile.PatientCode ?? string.Empty)
                            : preQuery.OrderBy(sm => sm.PatientProfile.PatientCode ?? string.Empty);
                        break;
                    case "prescriptiondate":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PrescriptionDate)
                            : preQuery.OrderBy(sm => sm.PrescriptionDate);
                        break;
                    case "age":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PatientProfile.Age ?? 0)
                            : preQuery.OrderBy(sm => sm.PatientProfile.Age ?? 0);
                        break;
                    case "gender":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PatientProfile.Gender)
                            : preQuery.OrderBy(sm => sm.PatientProfile.Gender);
                        break;
                    case "totalprescriptions":
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm =>
                                _dbContext.Smartrx_Master
                                    .AsNoTracking()
                                    .Count(sm2 =>
                                    sm2.PatientId == sm.PatientId &&
                                    sm2.IsRecommended == true &&
                                    sm2.IsApproved == true &&
                                    sm2.IsCompleted == true))
                            : preQuery.OrderBy(sm =>
                                _dbContext.Smartrx_Master
                                    .AsNoTracking()
                                    .Count(sm2 =>
                                    sm2.PatientId == sm.PatientId &&
                                    sm2.IsRecommended == true &&
                                    sm2.IsApproved == true &&
                                    sm2.IsCompleted == true));
                        break;
                    default:
                        sortedPreQuery = sortDescending
                            ? preQuery.OrderByDescending(sm => sm.PrescriptionDate)
                            : preQuery.OrderBy(sm => sm.PrescriptionDate);
                        break;
                }

                // ========== STEP 5: PROJECTION ==========
                var projectedQuery = sortedPreQuery
                    .Select(sm => new SmartRxWithVitalsContract
                    {
                        SmartRxId = sm.Id,
                        PatientId = sm.PatientId,
                        PrescriptionId = sm.PrescriptionId,
                        PrescriptionDate = sm.PrescriptionDate,
                        Remarks = sm.Remarks,
                        PatientInfo = new SmartRxPatientProfileWithVitalsContract
                        {
                            Id = sm.PatientProfile.Id,
                            PatientCode = sm.PatientProfile.PatientCode,
                            FirstName = sm.PatientProfile.FirstName,
                            LastName = sm.PatientProfile.LastName,
                            NickName = sm.PatientProfile.NickName,
                            Age = sm.PatientProfile.Age,
                            AgeYear = sm.PatientProfile.AgeYear,
                            AgeMonth = sm.PatientProfile.AgeMonth,
                            DateOfBirth = sm.PatientProfile.DateOfBirth,
                            Gender = sm.PatientProfile.Gender,
                            GenderString = sm.PatientProfile.Gender == (int)Gender.Male ? Gender.Male.ToString() : Gender.Female.ToString(),
                            BloodGroup = sm.PatientProfile.BloodGroup,
                            Height = sm.PatientProfile.Height,
                            HeightFeet = sm.PatientProfile.HeightFeet,
                            HeightInches = sm.PatientProfile.HeightInches,
                            HeightMeasurementUnit = sm.PatientProfile.HeightUnit.Name,
                            Weight = sm.PatientProfile.Weight,
                            WeightMeasurementUnit = sm.PatientProfile.WeightUnit.Name,
                            PhoneNumber = sm.PatientProfile.PhoneNumber,
                            Email = sm.PatientProfile.Email,
                            ProfilePhotoName = sm.PatientProfile.ProfilePhotoName,
                            ProfilePhotoPath = sm.PatientProfile.ProfilePhotoPath,
                            Address = sm.PatientProfile.Address,
                            PoliceStationId = sm.PatientProfile.PoliceStationId,
                            CityId = sm.PatientProfile.CityId,
                            PostalCode = sm.PatientProfile.PostalCode,
                            EmergencyContact = sm.PatientProfile.EmergencyContact,
                            MaritalStatus = sm.PatientProfile.MaritalStatus,
                            Profession = sm.PatientProfile.Profession,
                            IsExistingPatient = sm.PatientProfile.IsExistingPatient,
                            ExistingPatientId = sm.PatientProfile.ExistingPatientId,
                            ProfileProgress = sm.PatientProfile.ProfileProgress,
                            IsActive = sm.PatientProfile.IsActive,
                            TotalPrescriptions = _dbContext.Smartrx_Master
                                .AsNoTracking()
                                .Count(sm2 =>
                                sm2.PatientId == sm.PatientId &&
                                sm2.IsRecommended == true &&
                                sm2.IsApproved == true &&
                                sm2.IsCompleted == true),
                            RxType = "SmartRx" // Since we're only getting completed SmartRx records
                        },
                        Vitals = sm.PatientVitals
                            .Where(v => string.IsNullOrWhiteSpace(vitalName) || v.Vital.Name.ToLower().Contains(vitalName.Trim().ToLower()))
                            .Where(v => fromDate == null || fromDate == default(DateTime) || v.CreatedDate.Value.Date >= fromDate.Value.Date)
                            .Where(v => toDate == null || toDate == default(DateTime) || v.CreatedDate.Value.Date <= toDate.Value.Date)
                            .Where(v => searchKeyword == null || v.Vital.Name.Contains(searchKeyword) || v.VitalValue.ToString().Contains(searchKeyword) || v.VitalStatus!.Contains(searchKeyword))
                            .Select(v => new SmartRxVitalContract
                            {
                                Id = v.Id,
                                SmartRxMasterId = v.SmartRxMasterId,
                                PrescriptionId = v.PrescriptionId,
                                VitalId = v.VitalId,
                                VitalValue = v.VitalValue,
                                VitalName = v.Vital.Name,
                                ApplicableEntity = v.Vital.ApplicableEntity,
                                VitalUnit = v.Vital.Unit.MeasurementUnit,
                                VitalStatus = v.VitalStatus,
                                HeightFeet = v.HeightFeet,
                                HeightInches = v.HeightInches,
                                CreatedDate = v.CreatedDate
                            }).ToList()
                    });

                // ========== STEP 6: PAGING ==========
                // Apply paging as the final step
                var pagedData = await projectedQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync(cancellationToken);

                // Compute VitalStatus using CalcualteVitalMeasurements for the paged results
                if (pagedData != null && pagedData.Count > 0)
                {
                    var smartRxIdSet = pagedData.Select(x => x.SmartRxId).Distinct().ToList();
                    var prescriptionIdSet = pagedData.Select(x => x.PrescriptionId).Distinct().ToList();

                    var vitalsForPageQuery = _dbContext.Smartrx_Vital
                        .AsNoTracking()
                        .Where(v => smartRxIdSet.Contains(v.SmartRxMasterId) && prescriptionIdSet.Contains(v.PrescriptionId));

                    // Apply the same filtering for vitals as in the main query
                    if (!string.IsNullOrWhiteSpace(vitalName))
                    {
                        var vitalNameFilter = vitalName.Trim().ToLower();
                        vitalsForPageQuery = vitalsForPageQuery.Where(v => v.Vital.Name.ToLower().Contains(vitalNameFilter));
                    }

                    if (fromDate is not null && fromDate != default(DateTime))
                    {
                        vitalsForPageQuery = vitalsForPageQuery.Where(v => v.CreatedDate.Value.Date >= fromDate.Value.Date);
                    }

                    if (toDate is not null && toDate != default(DateTime))
                    {
                        vitalsForPageQuery = vitalsForPageQuery.Where(v => v.CreatedDate.Value.Date <= toDate.Value.Date);
                    }

                    var vitalsForPage = await vitalsForPageQuery
                        .Include(v => v.Vital)
                        .ThenInclude(vt => vt.Unit)
                        .ToListAsync(cancellationToken);

                    var vitalsBySmartRx = vitalsForPage
                        .GroupBy(v => new { v.SmartRxMasterId, v.PrescriptionId })
                        .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var item in pagedData)
                    {
                        if (item.Vitals == null || item.Vitals.Count == 0) continue;

                        var key = new { SmartRxMasterId = item.SmartRxId, PrescriptionId = item.PrescriptionId };
                        if (!vitalsBySmartRx.TryGetValue(key, out var entityVitals)) continue;

                        var statusByVitalEntityId = new Dictionary<long, string>();

                        string diastolic = string.Empty;
                        decimal diastolicValue = 0m;
                        decimal? diastolicLowValue = null, diastolicMediumValue = null, diastolicHighValue = null;

                        string systolic = string.Empty;
                        decimal systolicValue = 0m;
                        decimal? systolicLowValue = null, systolicMediumValue = null, systolicHighValue = null;

                        string systolicStatus = null;
                        string diastolicStatus = null;

                        foreach (var vt in entityVitals)
                        {
                            var vitalCalculated = PMSBackend.Application.CommonServices.PatientSmartRx.CalcualteVitalMeasurements
                                .CalcualteVitalDataMeasurements(
                                    vt,
                                    ref diastolic,
                                    ref diastolicValue,
                                    ref diastolicLowValue,
                                    ref diastolicMediumValue,
                                    ref diastolicHighValue,
                                    ref systolic,
                                    ref systolicValue,
                                    ref systolicLowValue,
                                    ref systolicMediumValue,
                                    ref systolicHighValue,
                                    ref systolicStatus,
                                    ref diastolicStatus
                                );

                            statusByVitalEntityId[vt.Id] = vitalCalculated?.Status;
                        }

                        foreach (var vitalContract in item.Vitals)
                        {
                            if (statusByVitalEntityId.TryGetValue(vitalContract.Id, out var status) && !string.IsNullOrWhiteSpace(status))
                            {
                                vitalContract.VitalStatus = status;
                            }
                        }
                    }
                }

                return new PaginatedResult<SmartRxWithVitalsContract>(
                    pagedData,
                    totalRecords,
                    pagingSorting.PageNumber,
                    pagingSorting.PageSize,
                    pagingSorting.SortBy,
                    pagingSorting.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load completed SmartRx with vitals for user: " + ex.Message);
            }
        }
    }

}
