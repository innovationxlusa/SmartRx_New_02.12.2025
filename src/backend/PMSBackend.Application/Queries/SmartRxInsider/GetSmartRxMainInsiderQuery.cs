using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.CommonServices.PatientSmartRx;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxMainInsiderQuery : IRequest<SmartRxDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long? PrescriptionId { get; set; }
        public long? PatientId { get; set; }
      //  public long LoginUserId { get; set; }

    }

    public class GetSmartRxMainInsiderQueryHandler : IRequestHandler<GetSmartRxMainInsiderQuery, SmartRxDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;
        private readonly IMedicineCompareRepository _medicineCompareRepository;
        private readonly ISmartRxOtherExpenseRepository _smartRxOtherExpenseRepository;

        public GetSmartRxMainInsiderQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository, IMedicineCompareRepository medicineCompareRepository, ISmartRxOtherExpenseRepository smartRxOtherExpenseRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
            _medicineCompareRepository = medicineCompareRepository;
            _smartRxOtherExpenseRepository = smartRxOtherExpenseRepository;
        }
        public async Task<SmartRxDTO> Handle(GetSmartRxMainInsiderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                SmartRxDTO responseResult = new SmartRxDTO();
                var smartRx = await _smartRxInsiderRepository.GetDetailsBySmartRxIdAsync(request.SmartRxMasterId);
                if (smartRx == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No smartrx found!"
                    };
                    return responseResult;
                }
                var patient = await _smartRxInsiderRepository.GetPatientProfileById(smartRx.PatientId);
                if (patient is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No patient found!"
                    };
                    return responseResult;
                }

                responseResult.SmartRxId = smartRx.Id;
                responseResult.UserId = smartRx.UserId;
                //responseResult.LoginUserId = request.LoginUserId;
                responseResult.PatientId = smartRx.PatientId;
                responseResult.PrescriptionDate = smartRx.PrescriptionDate ?? new DateTime(2000, 01, 01);
                //responseResult.DiscountPercentageOnInvestigationByDoctor=smartRx.DiscountPercentageOnInvestigationByDoctor;
                //responseResult.DiscountPercentageOnInvestigationByDoctor = smartRx.DiscountPercentageOnMedicineByDoctor;
                responseResult.HasAnyRelative = smartRx.HasAnyRelative;
                responseResult.IsExistingPatient = smartRx.IsExistingPatient;
                responseResult.Tag1 = smartRx.Tag1;
                responseResult.Tag2 = smartRx.Tag2;
                responseResult.Tag3 = smartRx.Tag3;
                responseResult.Tag4 = smartRx.Tag4;
                responseResult.Tag5 = smartRx.Tag5;
                responseResult.Remarks = smartRx.Remarks;
                responseResult.PatientInfo = new SmartRxPatientProfile();


                responseResult.PatientInfo.PatientCode = patient.PatientCode;
                responseResult.PatientInfo.FirstName = patient.FirstName;
                responseResult.PatientInfo.LastName = patient.LastName;
                responseResult.PatientInfo.NickName = patient.NickName;
                responseResult.PatientInfo.Age = patient.Age;
                responseResult.PatientInfo.AgeYear = patient.AgeYear;
                responseResult.PatientInfo.AgeMonth = patient.AgeMonth;
                responseResult.PatientInfo.DateOfBirth = patient.DateOfBirth ?? null;
                responseResult.PatientInfo.Gender = patient.Gender;
                responseResult.PatientInfo.BloodGroup = patient.BloodGroup;
                responseResult.PatientInfo.Height = patient.Height;
                responseResult.PatientInfo.HeightFeet = patient.HeightFeet ?? 0;
                responseResult.PatientInfo.HeightInch = patient.HeightInches ?? 0;
                responseResult.PatientInfo.HeightMeasuremetUnit = patient.HeightUnit != null && patient.HeightUnit.Name != null ? patient.HeightUnit.Name : "ftin";
                responseResult.PatientInfo.Weight = patient.Weight;
                responseResult.PatientInfo.WeightMeasuremetUnit = patient.WeightUnit != null && patient.WeightUnit.Name != null ? patient.WeightUnit.Name : "kg";
                responseResult.PatientInfo.PhoneNumber = patient.PhoneNumber;
                responseResult.PatientInfo.Email = patient.Email;
                responseResult.PatientInfo.ProfilePhotoName = patient.ProfilePhotoName;
                responseResult.PatientInfo.ProfilePhotoPath = patient.ProfilePhotoPath;
                responseResult.PatientInfo.Address = patient.Address!;
                responseResult.PatientInfo.PoliceStationId = patient.PoliceStationId;
                responseResult.PatientInfo.CityId = patient.CityId;
                responseResult.PatientInfo.DistrictId = (patient.City != null && patient.City.DistrictId != null) ? patient.City!.DistrictId : 0;
                responseResult.PatientInfo.PostalCode = patient.PostalCode;
                responseResult.PatientInfo.CountryId = (patient.City != null && patient.City.CountryId != null) ? patient.City.CountryId ?? 1 : 1;
                responseResult.PatientInfo.EmergencyContact = patient.EmergencyContact;
                responseResult.PatientInfo.MaritalStatus = patient.MaritalStatus;
                responseResult.PatientInfo.ProfileProgress = patient.ProfileProgress;
                responseResult.PatientInfo.IsActive = patient.IsActive;



                List<SmartRxPrescription> srPrescriptionList = new List<SmartRxPrescription>();
                var prescriptions = await _smartRxInsiderRepository.GetAllPrescriptionOfOnePatientBySmartRxIdAsync(smartRx.Id);
                if (prescriptions is not null)
                {
                    foreach (var pr in prescriptions)
                    {
                        var prescription = new SmartRxPrescription()
                        {
                            PrescriptionId = pr!.Id,
                            PrescriptionCode = pr!.PrescriptionCode,
                            PatientId = pr.PatientId,
                            IsExistingPatient = pr.IsExistingPatient,
                            HasExistingRelative = pr.HasExistingRelative,
                            RelativePatientIds = pr.RelativePatientIds,
                            FileName = pr.FileName,
                            FilePath = pr.FilePath,
                            FileExtension = pr.FileExtension,
                            NumberOfFilesStoredForThisPrescription = pr.NumberOfFilesStoredForThisPrescription,
                            UserId = pr.UserId,
                            FolderId = pr.FolderId,
                            IsSmartRxRequested = pr.IsSmartRxRequested,
                            IsLocked = pr.IsLocked,
                            LockedById = pr.LockedById,
                            LockedDate = pr.LockedDate,
                            IsReported = pr.IsReported,
                            ReportById = pr.ReportById,
                            ReportDate = pr.ReportDate,
                            ReportReason = pr.ReportReason,
                            ReportDetails = pr.ReportDetails,
                            IsRecommended = pr.IsRecommended,
                            RecommendedById = pr.RecommendedById,
                            RecommendedDate = pr.RecommendedDate,
                            IsApproved = pr.IsApproved,
                            ApprovedById = pr.ApprovedById,
                            ApprovedDate = pr.ApprovedDate,
                            IsCompleted = pr.IsCompleted,
                            CompletedById = pr.CompletedById,
                            CompletedDate = pr.CompletedDate,
                            Tag1 = pr.Tag1,
                            Tag2 = pr.Tag2,
                            Tag3 = pr.Tag3,
                            Tag4 = pr.Tag4,
                            Tag5 = pr.Tag5,
                            NextAppoinmentDate = pr.NextAppoinmentDate,
                            NextAppoinmentTime = pr.NextAppoinmentTime,
                            DiscountPercentageOnInvestigationByDoctor = pr.DiscountPercentageOnInvestigationByDoctor,
                            DiscountPercentageOnMedicineByDoctor = pr.DiscountPercentageOnMedicineByDoctor

                        };

                        List<SmartRxChiefComplaintDTO> patientChiefComplaints = new List<SmartRxChiefComplaintDTO>();
                        var chiefComplaints = await _smartRxInsiderRepository.GetPatientChiefComplaintListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (chiefComplaints is not null && chiefComplaints.Any())
                        {
                            foreach (var cc in chiefComplaints)
                            {
                                var complaint = new SmartRxChiefComplaintDTO();
                                complaint.SmartRxMasterId = cc.SmartRxMasterId;
                                complaint.UploadedPrescriptionId = cc.UploadedPrescriptionId;
                                complaint.Description = cc.Description;

                                // Extract tokens from description and look up acronyms/abbreviations
                                var matchedAcronyms = new List<AcronymsDTO>();
                                var seenIds = new HashSet<long>();
                                var description = cc.Description ?? string.Empty;
                                var tokens = description
                                    .Split(new char[] { ' ', ',', ';', '.', ':', '\\', '/', '\n', '\r', '\t', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .ToList();

                                foreach (var token in tokens)
                                {
                                    var results = await _smartRxInsiderRepository.GetPatientChiefComplaintAcronymBySmartRxId(token, token, cancellationToken);
                                    if (results != null && results.Any())
                                    {
                                        foreach (var r in results)
                                        {
                                            if (!seenIds.Contains(r.Id))
                                            {
                                                seenIds.Add(r.Id);
                                                var acr = new AcronymsDTO();
                                                acr.Id = r.Id;
                                                acr.Acronym = r.Acronym;
                                                acr.Abbreviation = r.Abbreviation;
                                                acr.Elaboration = r.Elaboration;
                                                acr.Details = r.Details;
                                                matchedAcronyms.Add(acr);
                                            }
                                        }
                                    }
                                }

                                complaint.Acronyms = matchedAcronyms;
                                patientChiefComplaints.Add(complaint);
                            }
                        }
                        prescription.ChiefComplaints = patientChiefComplaints;

                        List<SmartRxHistoryDTO> historyList = new List<SmartRxHistoryDTO>();
                        var histories = await _smartRxInsiderRepository.GetPatientHistoryListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (histories is not null && histories.Any())
                        {
                            Parallel.ForEach(histories, async hs =>
                            {
                                var history = new SmartRxHistoryDTO();
                                history.SmartRxMasterId = smartRx.Id;
                                history.PrescriptionId = pr.Id;
                                history.Title = hs.Title;
                                history.Details = hs.Details;
                                historyList.Add(history);
                            });
                            prescription.Histories = historyList;
                        }

                        var doctor = await _smartRxInsiderRepository.GetPatientOnePrescriptionDoctorBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (doctor != null)
                        {
                            var doc = new SmartRxDoctorDTO()
                            {
                                SmartRxMasterId = doctor.SmartRxMasterId,
                                PrescriptionId = doctor.PrescriptionId,
                                DoctorId = doctor.DoctorId,

                                ChamberWaitTimeHour = doctor.ChamberWaitTimeHour,
                                ChamberWaitTimeMinute = doctor.ChamberWaitTimeMinute,
                                ConsultingDurationInMinutes = doctor.ConsultingDurationInMinutes,
                                TravelTimeMinute = doctor.TravelTimeMinute,

                                ChamberFee = doctor.ChamberFee,
                                ChamberFeeMeasurementUnitId = doctor.ChamberFeeMeasurementUnitId,
                                ChamberFeeMeasurementUnit = doctor.ChamberFeeMeasurementUnit,
                                TransportFee = doctor.TransportFee,
                                OtherExpense = doctor.OtherExpense,

                                DoctorRating = doctor.DoctorRating,
                                Comments = doctor.Comments
                            };

                            doc.PatientDoctor = new DoctorProfileDTO()
                            {
                                DoctorId = doctor.DoctorId,
                                DoctorTitle = doctor.PatientDoctor.DoctorTitle,
                                DoctorFirstName = doctor.PatientDoctor.DoctorFirstName,
                                DoctorLastName = doctor.PatientDoctor.DoctorLastName,
                                ProfilePhotoName = doctor.PatientDoctor.ProfilePhotoName,
                                ProfilePhotoPath = doctor.PatientDoctor.ProfilePhotoPath,
                                DoctorSpecializedArea = doctor.PatientDoctor.DoctorSpecializedArea,
                                DoctorBMDCRegNo = doctor.PatientDoctor.DoctorBMDCRegNo,
                                DoctorCode = doctor.PatientDoctor.DoctorCode,
                            };
                            if (doctor.DoctorEducations != null)
                            {
                                doc.DoctorEducations = doctor!.DoctorEducations!.Select(e => new EducationDTO()
                                {
                                    EducationCode = e.EducationCode,
                                    EducationDegreeName = e.EducationDegreeName,
                                    EducationDescription = e.EducationDescription,
                                    EducationId = e.EducationId,
                                    EducationInstitutionName = e.EducationInstitutionName
                                }).ToList();
                            }
                            if (doctor!.DoctorChambers != null)
                            {
                                doc.Chambers = doctor!.DoctorChambers!.Select(c => new SmartRxDoctorChamberDTO()
                                {
                                    HospitalId = c.HospitalId,
                                    ChamberName = c.ChamberName,
                                    ChamberAddress = c.ChamberAddress,
                                    ChamberGoogleAddress = c.ChamberGoogleAddress,
                                    ChamberCityName = c.ChamberCityName,
                                    ChamberCloseDay = c.ChamberCloseDay,
                                    ChamberDescription = c.ChamberDescription,
                                    ChamberDoctorBookingMobileNos = c.ChamberDoctorBookingMobileNos,
                                    ChamberEmail = c.ChamberEmail,
                                    ChamberEndTime = c.ChamberEndTime,
                                    ChamberGoogleRating = c.ChamberGoogleRating,
                                    ChamberHelpline = c.ChamberHelpline,
                                    ChamberOpenDay = c.ChamberOpenDay,
                                    ChamberOtherDoctorsId = c.ChamberOtherDoctorsId,
                                    ChamberPostalCode = c.ChamberPostalCode,
                                    ChamberStartTime = c.ChamberStartTime,
                                    ChamberVisitingHour = c.ChamberVisitingHour,
                                    DepartmentName = c.DepartmentName,
                                    DepartmentSectionName = c.DepartmentSectionName,
                                    DoctorDesignaitonInChamber = c.DoctorDesignaitonInChamber,
                                    DoctorId = c.DoctorId,
                                    DoctorSpecialization = c.DoctorSpecialization,
                                    HospitalName = c.HospitalName,
                                    IsMainChamber = c.IsMainChamber,
                                    VisitingHour = c.VisitingHour,
                                    DoctorVisitingDaysInChamber = c.DoctorVisitingDaysInChamber,
                                    Remarks = c.Remarks,
                                    IsActive = c.IsActive,
                                    HospitalLogoFileName = c.HospitalLogoFileName,
                                    HospitalLogoFilePath = c.HospitalLogoFilePath,
                                    HospitalLogoFileExtension = c.HospitalLogoFileExtension
                                }).ToList();
                                prescription.Doctor = doc;
                            }
                            prescription.Doctor = doc;
                        }
                        var vitals = await _smartRxInsiderRepository.GetPatientVitalListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (vitals is not null && vitals.Any())
                        {
                            List<SmartRxVital> srxVitalList = new List<SmartRxVital>();

                            string diastolic = string.Empty;
                            decimal diastolicValue = 0;
                            decimal? diastolicLowValue = 0;
                            decimal? diastolicMediumValue = 0;
                            decimal? diastolicHighValue = 0;

                            string systolic = string.Empty;
                            decimal systolicValue = 0;
                            decimal? systolicLowValue = 0;
                            decimal? systolicMediumValue = 0;
                            decimal? systolicHighValue = 0;

                            var nonBmiVitals = vitals.Where(v => v.Vital?.Name != "BMI").ToList();
                            var bmiVitals = vitals.Where(v => v.Vital?.Name == "BMI").ToList();
                            string systolicStatus = null;
                            string diastolicStatus = null;
                            // Process non-BMI vitals in parallel
                            Parallel.ForEach(nonBmiVitals, vt =>
                            {
                                lock (srxVitalList)
                                {
                                    SmartRxVital vital = CalcualteVitalMeasurements.CalcualteVitalDataMeasurements(vt, ref diastolic, ref diastolicValue, ref diastolicLowValue, ref diastolicMediumValue, ref diastolicHighValue, ref systolic, ref systolicValue, ref systolicLowValue, ref systolicMediumValue, ref systolicHighValue, ref systolicStatus, ref diastolicStatus);
                                    srxVitalList.Add(vital);
                                }
                            });

                            // Process BMI separately at the end
                            foreach (var vt in bmiVitals)
                            {
                                var vital = new SmartRxVital();
                                var vtVitalDetails = vt.Vital;
                                var measurement = vt.Vital.Unit;

                                vital.PrescriptionId = vt.PrescriptionId;
                                vital.SmartRxMasterId = vt.SmartRxMasterId;
                                vital.VitalId = vt.VitalId;

                                // Tuple<int, int> height = BmiCalculator.ConvertDecimalHeightToFeetInches(heightInMeter);
                                if (responseResult.PatientInfo is not null && responseResult.PatientInfo.HeightFeet > 0 && responseResult.PatientInfo.Weight > 0)
                                {
                                    decimal bmi = BmiCalculator.CalculateBmi(responseResult.PatientInfo!.HeightFeet, responseResult.PatientInfo!.HeightInch, responseResult.PatientInfo.Weight.Value) ?? 0;
                                    vt.VitalValue = bmi;
                                    vital.VitalValue = bmi;
                                    vital.VitalValueString = Common.FormatDecimal(bmi);
                                    if (bmi < vtVitalDetails.LowRange)
                                        vital.Status = vtVitalDetails.LowStatus;
                                    else if (bmi >= vtVitalDetails.LowRange && bmi < vtVitalDetails.MidRange)
                                        vital.Status = vtVitalDetails.MidNextStatus;
                                    else if (bmi >= vtVitalDetails.MidRange && bmi < vtVitalDetails.MidNextRange)
                                        vital.Status = vtVitalDetails.MidNextStatus;
                                    else if (bmi >= vtVitalDetails.HighRange)
                                        vital.Status = vtVitalDetails.HighStatus;
                                }


                                vital.Code = vtVitalDetails.Code;
                                vital.Name = vtVitalDetails.Name;
                                vital.Description = vt.Vital.Description;
                                vital.ApplicableEntity = vtVitalDetails.ApplicableEntity!;
                                vital.VitalMidNextRange = vt.Vital.MidRange;


                                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.LowRange))} - {Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))}";
                                vital.VitalValueOverWeightString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))} - {Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidNextRange))}";

                                vital.MeasurementUnit = measurement.MeasurementUnit;
                                vital.MeasurementUnitDetails = measurement.Details;

                                srxVitalList.Add(vital);
                            }
                            prescription.Vitals = srxVitalList;
                        }


                        var medicines = await _smartRxInsiderRepository.GetPatientMedicineListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (medicines is not null)
                        {
                            List<SmartRxMedicinesDTO> srxMedicineList = new List<SmartRxMedicinesDTO>();
                            decimal totalMedicineExpense = 0;
                            foreach (var md in medicines.Medicines)
                            {
                                var medicine = new SmartRxMedicinesDTO();
                                medicine.Id = md.Id;
                                medicine.SmartRxMasterId = md.SmartRxMasterId;
                                medicine.PrescriptionId = pr.Id;
                                medicine.MedicineId = md.MedicineId;
                                medicine.MedicineName = md.Medicine.Name;

                                medicine.MedicineBrandId = md.Medicine.BrandId;
                                medicine.MedicineBrandName = md.Medicine.Brand.Name;
                                medicine.MedicineBrandCode = md.Medicine.Brand.BrandCode;
                                medicine.MedicineBrandPublicId = md.Medicine.Brand.BrandPublicId;
                                medicine.MedicineBrandDescription = md.Medicine.Brand.Description;

                                medicine.MedicineManufacturerId = md.Medicine.Brand.ManufacturerId;
                                medicine.MedicineManufacturerName = md.Medicine.Brand.Manufacturer.Name;
                                medicine.MedicineManufacturerProducts = md.Medicine.Brand.Manufacturer.Products;
                                medicine.MedicineManufacturerOriginRegion = md.Medicine.Brand.Manufacturer.OriginRegion;
                                medicine.MedicineImporter = md.Medicine.Brand.Manufacturer.Importer;
                                medicine.MedicineManufacturingEstablishedDate = md.Medicine.Brand.Manufacturer.EstablishedDate;
                                medicine.MedicineManufacturerUrl = md.Medicine.Brand.Manufacturer.CompanyUrl;
                                medicine.MedicineManufacturerLogoFileName = md.Medicine.Brand.Manufacturer.FileName;
                                medicine.MedicineManufacturerLogoFilePath = md.Medicine.Brand.Manufacturer.FilePath;
                                medicine.MedicineManufacturerLogoFileExtension = md.Medicine.Brand.Manufacturer.FileExtension;

                                medicine.MedicineType = md.Medicine.Type;
                                medicine.MedicineSlug = md.Medicine.Slug;
                                medicine.MedicineDosageFormId = md.Medicine.DosageFormId;
                                medicine.MedicineDosageFormName = md.Medicine.MedicineDosageForm.Name;
                                medicine.MedicineDosageFormDescription = md.Medicine.MedicineDosageForm.Description;
                                medicine.MedicineShortForm = md.Medicine.MedicineDosageForm.ShortForm;

                                medicine.MedicineGenericId = md.Medicine.GenericId;
                                medicine.MedicineGenericName = md.Medicine.Generic.Name;
                                medicine.MedicineGenericDescription = md.Medicine.Generic.Description;

                                medicine.MedicineStrength = md.Medicine.Strength;
                                medicine.MedicineMeasurementUnitId = md.Medicine.MeasurementUnitId;
                                medicine.MedicineMeasurementUnitCode = md.Medicine.MeasurementUnit!.Code;
                                medicine.MedicineMeasurementUnitName = md.Medicine.MeasurementUnit.Name;
                                medicine.MedicineMeasurementUnit = md.Medicine.MeasurementUnit.MeasurementUnit;
                                medicine.MedicineMeasurementUnitDescription = md.Medicine.MeasurementUnit.Description;
                                medicine.MedicineMeasurementUnitDetails = md.Medicine.MeasurementUnit.Details;
                                medicine.MedicineMeasurementUnitType = md.Medicine.MeasurementUnit.Type;

                                medicine.MedicinePriceInUnitId = md.Medicine.PriceInUnitId;
                                medicine.MedicinePriceInUnitCode = md.Medicine.PriceUnit!.Code;
                                medicine.MedicinePriceInUnitName = md.Medicine.PriceUnit.Name;
                                medicine.MedicinePriceInUnitDetails = md.Medicine.PriceUnit.Details;
                                medicine.MedicinePriceInUnitDescription = md.Medicine.PriceUnit.Description;
                                medicine.MedicinePriceInUnit = md.Medicine.PriceUnit.MeasurementUnit;
                                medicine.MedicinePriceInUnitType = md.Medicine.PriceUnit.Type;
                                medicine.MedicineUnitPrice = md.Medicine.UnitPrice;

                                medicine.MedicinePackageType = md.Medicine.PackageType;
                                medicine.MedicinePackageSize = md.Medicine.PackageSize;
                                medicine.MedicinePackageQuantity = md.Medicine.PackageQuantity;
                                medicine.MedicineDAR = md.Medicine.DAR;
                                medicine.MedicineIndication = md.Medicine.Indication;
                                medicine.MedicinePharmacology = md.Medicine.Pharmacology;
                                medicine.MedicineDoseDescription = md.Medicine.DoseDescription;
                                medicine.MedicineAdministration = md.Medicine.Administration;
                                medicine.MedicineContradiction = md.Medicine.Contradiction;
                                medicine.MedicineSideEffects = md.Medicine.SideEffects;
                                medicine.MedicinePrecautionsAndWarnings = md.Medicine.PrecautionsAndWarnings;
                                medicine.MedicinePregnencyAndLactation = md.Medicine.PregnencyAndLactation;
                                medicine.MedicineModeOfAction = md.Medicine.ModeOfAction;
                                medicine.MedicineInteraction = md.Medicine.Interaction;
                                medicine.MedicineOverdoseEffects = md.Medicine.OverdoseEffects;
                                medicine.MedicineTherapeuticClass = md.Medicine.TherapeuticClass;
                                medicine.MedicineStorageCondition = md.Medicine.StorageCondition;
                                medicine.MedicineUserFor = md.Medicine.UserFor;
                                medicine.MedicineCompanyDiscount = md.Medicine.CompanyDiscountPercentage;
                                medicine.MedicineIsActive = md.Medicine.IsActive;


                                medicine.FrequencyInADay = md.FrequencyInADay;
                                medicine.Dose1InADay = md.Dose1InADay;
                                medicine.Dose2InADay = md.Dose2InADay;
                                medicine.Dose3InADay = md.Dose3InADay;
                                medicine.Dose4InADay = md.Dose4InADay;
                                medicine.Dose5InADay = md.Dose5InADay;
                                medicine.Dose6InADay = md.Dose6InADay;
                                medicine.Dose7InADay = md.Dose7InADay;
                                medicine.Dose8InADay = md.Dose8InADay;
                                medicine.Dose9InADay = md.Dose9InADay;
                                medicine.Dose10InADay = md.Dose10InADay;
                                medicine.Dose11InADay = md.Dose11InADay;
                                medicine.Dose12InADay = md.Dose12InADay;
                                medicine.IsMoreThanRegularDose = md.IsMoreThanRegularDose;
                                medicine.DescriptionForMoreThanRegularDose = md.DescriptionForMoreThanRegularDose;
                                medicine.IsBeforeMeal = md.IsBeforeMeal;
                                medicine.DurationOfContinuation = md.DurationOfContinuation;
                                medicine.DurationOfContinuationCount = md.DurationOfContinuationCount;
                                medicine.DurationOfContinuationStartDate = md.DurationOfContinuationStartDate;
                                medicine.DurationOfContinuationEndDate = md.DurationOfContinuationEndDate;
                                medicine.Rules = md.Rules;
                                medicine.Restrictions = md.Restrictions;
                                medicine.Notes = md.Notes;
                                medicine.Wishlist = md.Wishlist;

                                var duration = md.DurationOfContinuationEndDate.Day - md.DurationOfContinuationStartDate.Day;
                                if (duration <= 0)
                                    duration = md.DurationOfContinuationCount;
                                decimal dosePerDay = md.Dose1InADay + md.Dose2InADay + md.Dose3InADay + md.Dose4InADay + md.Dose5InADay + md.Dose6InADay + md.Dose7InADay + md.Dose8InADay + md.Dose9InADay + md.Dose10InADay + md.Dose11InADay + md.Dose12InADay;

                                var totalMedicineCountInDuration = duration * dosePerDay;
                                decimal unitPrice = Convert.ToDecimal(md.Medicine.UnitPrice ?? 0);
                                decimal discountedPrice = md.Medicine.CompanyDiscountPercentage ?? 0 * unitPrice / 100;
                                totalMedicineExpense = unitPrice * totalMedicineCountInDuration - discountedPrice;

                                srxMedicineList.Add(medicine);
                            }

                            prescription.Medicines = srxMedicineList;
                            List<SmartRxMedicineWishListDTO> wishlists = new List<SmartRxMedicineWishListDTO>();
                            Parallel.ForEach(medicines.MedicineWishtlist, async wish =>
                            {
                                var wishlist = new SmartRxMedicineWishListDTO()
                                {
                                    Id = wish.Id,
                                    MedicineId = wish.MedicineId,
                                    PrescriptionId = wish.PrescriptionId,
                                    SmartRxMasterId = wish.SmartRxMasterId,
                                    Wished = wish.Wished,
                                    MedicineName = wish.MedicineName,
                                    WishedMedicines = wish.WishedMedicines
                                };
                                wishlists.Add(wishlist);
                            });
                            prescription.MedicineWishlist = wishlists;
                            responseResult.MedicineTotalExpense = totalMedicineExpense;
                        }

                        var investigations = await _smartRxInsiderRepository.GetPatientInvestigationListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (investigations is not null && investigations.Tests.Any())
                        {
                            List<SmartRxInvestigationListDTO> srxInvestigationList = new List<SmartRxInvestigationListDTO>();
                            decimal investigationTotalExpense = 0;

                            foreach (var inv in investigations.Tests)
                            {
                                var investigation = new SmartRxInvestigationListDTO();
                                investigation.Id = inv.Id;
                                investigation.SmartRxMasterId = inv.SmartRxMasterId;
                                investigation.PrescriptionId = pr.Id;
                                investigation.TestId = inv.TestId;
                                investigation.TestCode = inv.InvestigationTest.Code;
                                investigation.TestName = inv.InvestigationTest.TestName;
                                investigation.TestDescription = inv.InvestigationTest.TestDescription;
                                investigation.TestFullName = inv.InvestigationTest.TestFullName;
                                investigation.TestShortName = inv.InvestigationTest.TestShortName;
                                investigation.TestNameByDiagnosticCenter = inv.InvestigationTest.TestNameByDiagnosticCenter;
                                investigation.TestUnitPrice = inv.InvestigationTest.UnitPrice;
                                investigation.PriceUnitId = inv.InvestigationTest.PriceUnitId;
                                investigation.TestPriceName = inv.InvestigationTest.PriceUnit!.Name;
                                investigation.TestPriceMeasurementUnit = inv.InvestigationTest.PriceUnit.MeasurementUnit;
                                investigation.TestPriceType = inv.InvestigationTest.PriceUnit.Type;
                                investigation.TestNationalUnitPrice = inv.InvestigationTest.UnitPrice;
                                investigation.NationalPriceUnitId = inv.InvestigationTest.PriceUnitId;
                                investigation.NationalTestPriceName = inv.InvestigationTest.PriceUnit.Name;
                                investigation.NationalTestPriceMeasurementUnit = inv.InvestigationTest.PriceUnit.MeasurementUnit;
                                investigation.NationalTestPriceType = inv.InvestigationTest.PriceUnit.Type;

                                investigation.TestSpeciality = inv.InvestigationTest.Speciality;
                                investigation.TestComments = inv.InvestigationTest.Comments;

                                investigation.TestCenterIds = inv.UserSelectedTestCenterIds;
                                investigation.IsDoctorRecommended = inv.DoctorRecommendedTestCenterIds != null ? true : false;
                                if (inv.DoctorRecommendedTestCenterIds is not null)
                                {
                                    List<Configuration_HospitalEntity> testCenters = new List<Configuration_HospitalEntity>();

                                    foreach (var center in inv.DoctorRecommendedTestCenters)
                                    {
                                        Configuration_HospitalEntity testCenter = new Configuration_HospitalEntity();
                                        testCenter.Code = center!.Code;
                                        testCenter.Name = center!.Name;
                                        testCenter.Description = center!.Description;
                                        testCenter.Address = center!.Address;
                                        testCenter.CityId = center!.CityId;
                                        testCenter.YearEstablished = center!.YearEstablished;
                                        testCenter.GoogleRating = center!.GoogleRating;
                                        testCenter.GoogleLocation = center!.GoogleLocation;
                                        testCenter.OpenTime = center!.OpenTime;
                                        testCenter.CloseTime = center!.CloseTime;
                                        testCenter.OpenDay = center!.OpenDay;
                                        testCenter.CloseDay = center!.CloseDay;
                                        testCenter.Weekend = center!.Weekend;
                                        testCenter.Mobile = center!.Mobile;
                                        testCenter.Fax = center!.Fax;
                                        testCenter.Phone = center!.Phone;
                                        testCenter.WebAddress = center!.WebAddress;
                                        testCenter.Remarks = center!.Remarks;
                                        testCenter.IsActive = center!.IsActive;
                                        testCenter.FileName = center!.FileName;
                                        testCenter.FilePath = center!.FilePath;
                                        testCenter.FileExtension = center!.FileExtension;
                                        testCenter.DiagnosticCenterIcon = center!.DiagnosticCenterIcon;

                                        testCenters.Add(testCenter);
                                    }
                                    investigation.RecommendedTestCenters = testCenters;
                                }
                                investigation.IsUserSelectd = inv.UserSelectedTestCenterIds != null ? true : false;
                                if (inv.UserSelectedTestCenterIds is not null)
                                {
                                    List<Configuration_HospitalEntity> testCenters = new List<Configuration_HospitalEntity>();

                                    foreach (var center in inv.UserSelectedTestCenters)
                                    {
                                        Configuration_HospitalEntity testCenter = new Configuration_HospitalEntity();
                                        testCenter.Code = center!.Code;
                                        testCenter.Name = center!.Name;
                                        testCenter.Description = center!.Description;
                                        testCenter.Address = center!.Address;
                                        testCenter.CityId = center!.CityId;
                                        testCenter.YearEstablished = center!.YearEstablished;
                                        testCenter.GoogleRating = center!.GoogleRating;
                                        testCenter.GoogleLocation = center!.GoogleLocation;
                                        testCenter.OpenTime = center!.OpenTime;
                                        testCenter.CloseTime = center!.CloseTime;
                                        testCenter.OpenDay = center!.OpenDay;
                                        testCenter.CloseDay = center!.CloseDay;
                                        testCenter.Weekend = center!.Weekend;
                                        testCenter.Mobile = center!.Mobile;
                                        testCenter.Fax = center!.Fax;
                                        testCenter.Phone = center!.Phone;
                                        testCenter.WebAddress = center!.WebAddress;
                                        testCenter.Remarks = center!.Remarks;
                                        testCenter.IsActive = center!.IsActive;
                                        testCenter.FileName = center!.FileName;
                                        testCenter.FilePath = center!.FilePath;
                                        testCenter.FileExtension = center!.FileExtension;
                                        testCenter.DiagnosticCenterIcon = center!.DiagnosticCenterIcon;

                                        testCenters.Add(testCenter);
                                    }
                                    investigation.UserSelectedTestCenters = testCenters;
                                }

                                investigationTotalExpense += inv.InvestigationTest.UnitPrice;

                                srxInvestigationList.Add(investigation);
                            }

                            prescription.Investigations = srxInvestigationList!;
                            prescription.InvestigationWishList = investigations.TestsWishtlist!;
                            responseResult.InvestigationTotalExpense = investigationTotalExpense;
                        }

                        var advices = await _smartRxInsiderRepository.GetPatientAdviceListBySmartRxId(smartRx.Id, pr.Id, cancellationToken);
                        if (advices is not null && advices.Any())
                        {
                            List<SmartRxAdviceDTO> srxAdviceList = new List<SmartRxAdviceDTO>();
                            Parallel.ForEach(advices, async adv =>
                            {
                                var advice = new SmartRxAdviceDTO();
                                advice.SmartRxMasterId = adv.SmartRxMasterId;
                                advice.PrescriptionId = pr.Id;
                                advice.Advice = adv.Advice;
                                advice.AdviceKeywordToRecommend = adv.AdviceKeywordToRecommend;
                                srxAdviceList.Add(advice);
                            });
                            prescription.Advices = srxAdviceList;

                            var advicesRecommendations = await _smartRxInsiderRepository.GetPatientAdviceFAQListBySmartRxId(smartRx.Id, pr.Id);
                            if (advicesRecommendations is not null && advicesRecommendations.Any())
                            {
                                List<SmartRxAdviceFAQ> srxAdviceRecommendationList = new List<SmartRxAdviceFAQ>();
                                Parallel.ForEach(advicesRecommendations, async recommend =>
                                {
                                    var adviceRecommendation = new SmartRxAdviceFAQ();
                                    adviceRecommendation.Id = recommend.Id;
                                    adviceRecommendation.Question = recommend.Question;
                                    adviceRecommendation.Answer = recommend.Answer;
                                    adviceRecommendation.SearchKeyword = recommend.TagSearchKeyword;
                                    adviceRecommendation.IconFileExtension = recommend.IconFileExtension;
                                    adviceRecommendation.IconFilePath = recommend.IconFilePath;
                                    adviceRecommendation.IconFileName = recommend.IconFileName;

                                    srxAdviceRecommendationList.Add(adviceRecommendation);
                                });
                                prescription.AdviceRecommendations = srxAdviceRecommendationList;
                            }

                            var otherExpense = await _smartRxOtherExpenseRepository.GetSmartRxOtherExpensesAsync(
                                null,
                                smartRx.Id,
                                smartRx.PatientId,
                                smartRx.PrescriptionId,
                                cancellationToken);

                            if (otherExpense is not null && otherExpense.Any())
                            {
                                prescription.OtherExpenses = otherExpense.Select(oe => new SmartRxOtherExpenseDTO
                                {
                                    Id = oe.Id,
                                    SmartRxMasterId = oe.SmartRxMasterId,
                                    PrescriptionId = oe.PrescriptionId,
                                    ExpenseName = oe.ExpenseName,
                                    Description = oe.Description,
                                    Amount = oe.Amount,
                                    CurrencyUnitId = oe.CurrencyUnitId,
                                    CurrencyUnitName = oe.CurrencyUnitName,
                                    ExpenseDate = oe.ExpenseDate,
                                    ExpenseNotes = oe.ExpenseNotes,
                                    LoginUserId = oe.LoginUserId
                                }).ToList();
                            }

                        }

                        var otherExpenses = await _smartRxOtherExpenseRepository.GetSmartRxOtherExpensesAsync(null, smartRx.Id, smartRx.PatientId, smartRx.PrescriptionId, cancellationToken);
                        if (otherExpenses is not null && otherExpenses.Any())
                        {
                            prescription.OtherExpenses = otherExpenses.Select(oe => new SmartRxOtherExpenseDTO
                            {
                                Id = oe.Id,
                                SmartRxMasterId = oe.SmartRxMasterId,
                                PrescriptionId = oe.PrescriptionId,
                                ExpenseName = oe.ExpenseName,
                                Description = oe.Description,
                                Amount = oe.Amount,
                                CurrencyUnitId = oe.CurrencyUnitId,
                                CurrencyUnitName = oe.CurrencyUnitName,
                                ExpenseDate = oe.ExpenseDate,
                                ExpenseNotes = oe.ExpenseNotes,
                                LoginUserId = oe.LoginUserId
                            }).ToList();
                        }



                        srPrescriptionList.Add(prescription);
                    }

                    responseResult.Prescriptions = srPrescriptionList;
                }

                SmartRxOverviewDTO smartRxOverview = new SmartRxOverviewDTO();

                foreach (var singlePrescription in srPrescriptionList)
                {
                    smartRxOverview.TotalDoctorExpense += singlePrescription.Doctor.ChamberFee ?? 0;
                    smartRxOverview.TotalMedicineConsumption += singlePrescription.Medicines!.Sum(m => m.MedicineUnitPrice * m.DurationOfContinuationCount) ?? 0;
                    smartRxOverview.TotalLabInvestigationExpense += singlePrescription.Investigations!.Sum(i => i.TestUnitPrice);
                    smartRxOverview.TotalTransportExpense += singlePrescription.Doctor.TransportFee ?? 0;
                    smartRxOverview.TotalTravelTime += singlePrescription.Doctor.TravelTimeMinute ?? 0;
                    smartRxOverview.TotalWaitingTime += singlePrescription.Doctor.ChamberWaitTimeMinute ?? 0;
                    smartRxOverview.TotalVisitingTime += singlePrescription.Doctor.ConsultingDurationInMinutes ?? 0;
                    smartRxOverview.TotalOtherExpense += singlePrescription.Doctor.OtherExpense ?? 0;
                    smartRxOverview.OtherExpenses = singlePrescription.OtherExpenses;
                }
                responseResult.SmartRxOverview = smartRxOverview;


                await Task.CompletedTask;
                return responseResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
    }
}