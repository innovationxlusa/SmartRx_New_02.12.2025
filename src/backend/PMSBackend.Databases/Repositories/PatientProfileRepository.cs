using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PMSBackend.Databases.Repositories
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        private readonly PMSDbContext _dbContext;
        private readonly ICodeGenerationService _codeGenerationService;
        public PatientProfileRepository(PMSDbContext dbContext, ICodeGenerationService codeGenerationService)
        {
            _dbContext = dbContext;
            _codeGenerationService = codeGenerationService;
        }
        // Method to get patient profile with relatives by patient ID

        public async Task<PatientWithRelativesContract?> GetPatientProfileWithRelativesById(long id, CancellationToken cancellationToken)
        {
            try
            {
                var patientWithRelatives = await _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .Where(p => p.Id == id && !p.IsDeleted)
                    .Select(p => new PatientWithRelativesContract
                    {
                        Id = p.Id,
                        PatientCode = p.PatientCode,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        NickName = p.NickName,
                        Age = p.Age,
                        AgeYear = p.AgeYear,
                        AgeMonth = p.AgeMonth,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        GenderString = p.Gender == (int)Gender.Male ? Gender.Male.ToString() : Gender.Female.ToString(),
                        BloodGroup = p.BloodGroup,
                        Height = p.Height,
                        HeightFeet=p.HeightFeet??0,
                        HeightInches=p.HeightInches ?? 0,
                        HeightMeasurementUnit=p.HeightUnit.Name,
                        Weight = p.Weight,
                        WeightMeasurementUnit=p.WeightUnit.Name,
                        PhoneNumber = p.PhoneNumber,
                        Email = p.Email,
                        ProfilePhotoName = p.ProfilePhotoName,
                        ProfilePhotoPath = p.ProfilePhotoPath,
                        Address = p.Address,
                        PoliceStationId = p.PoliceStationId,
                        CityId = p.CityId,
                        PostalCode = p.PostalCode,
                        EmergencyContact = p.EmergencyContact,
                        MaritalStatus = p.MaritalStatus,
                        Profession = p.Profession,
                        IsExistingPatient = p.IsExistingPatient,
                        ExistingPatientId = p.ExistingPatientId,
                        ProfileProgress = p.ProfileProgress,
                        Relatives = _dbContext.Smartrx_PatientProfile
                            .AsNoTracking()
                            .Where(r => r.RelatedToPatientId == p.Id && r.IsRelative == true && !r.IsDeleted)
                            .Any() ? _dbContext.Smartrx_PatientProfile
                            .Where(r => r.RelatedToPatientId == p.Id && r.IsRelative == true && !r.IsDeleted)
                            .Select(r => new RelativeContract
                            {
                                Id = r.Id,
                                PatientCode = r.PatientCode,
                                FirstName = r.FirstName,
                                LastName = r.LastName,
                                NickName = r.NickName,
                                Age = r.Age,
                                AgeYear = r.AgeYear,
                                AgeMonth = r.AgeMonth,
                                DateOfBirth = r.DateOfBirth,
                                Gender = r.Gender,
                                BloodGroup = r.BloodGroup,
                                Height = r.Height,
                                PhoneNumber = r.PhoneNumber,
                                Email = r.Email,
                                ProfilePhotoName = r.ProfilePhotoName,
                                ProfilePhotoPath = r.ProfilePhotoPath,
                                Address = r.Address,
                                PoliceStationId = r.PoliceStationId,
                                CityId = r.CityId,
                                PostalCode = r.PostalCode,
                                EmergencyContact = r.EmergencyContact,
                                MaritalStatus = r.MaritalStatus,
                                Profession = r.Profession,
                                IsExistingPatient = r.IsExistingPatient,
                                ExistingPatientId = r.ExistingPatientId,
                                IsRelative = r.IsRelative,
                                RelatedToPatientId = r.RelatedToPatientId,
                                RelationToPatient = r.RelationToPatient,
                                ProfileProgress = r.ProfileProgress
                            })
                            .ToList() : null,
                        IsActive = p.IsActive,
                        TotalPrescriptions = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Count(),
                        RxType = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Any() ? "SmartRx" :
                            _dbContext.Prescription_UploadedPrescription
                            .AsNoTracking()
                            .Where(pr => pr.PatientId == p.Id && 
                                        pr.IsSmartRxRequested == true && 
                                        (pr.IsCompleted == null || pr.IsCompleted == false))
                            .Any() ? "Waiting" : "File Only"
                    })
                    .FirstOrDefaultAsync();
                //if (patientWithRelatives is not null)
                //{
                //    var vitalsWeight = await _dbContext.Smartrx_Vital.Where(v => v.Vital.Name == "Weight" && v.PatientId==id).ToListAsync(cancellationToken);
                //    var weight = vitalsWeight!.Where(v => v.Vital.ApplicableEntity == patientWithRelatives.GenderString).FirstOrDefault();
                //    if(weight is not null)patientWithRelatives.Weight = weight!.VitalValue;
                //    patientWithRelatives.WeightMeasurementUnit = weight.Vital.Unit.Name;
                //    var vitalsHeight = await _dbContext.Smartrx_Vital.Where(v => v.Vital.Name == "Height" && v.PatientId == id).ToListAsync(cancellationToken);
                //    var height = vitalsHeight!.Where(v => v.Vital.ApplicableEntity == patientWithRelatives.GenderString).FirstOrDefault();
                //    patientWithRelatives.Height = height!.VitalValue.ToString() + " "+ height.Vital.Unit.MeasurementUnit;
                //} 


                return patientWithRelatives;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load patient with relatives: " + ex.Message);
            }


        }

        public async Task<IList<PatientDropdown>> GetPatientDropdownInfoAsync(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var patientDropdown = await _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .Where(p => p.IsActive == true && !p.IsDeleted && p.UserId == userId)
                    .Select(p => new PatientDropdown()
                {
                    PatientId = p.Id,
                    PatientName = p.FirstName + " " + p.LastName + " " + p.NickName
                }).ToListAsync(cancellationToken);
                return patientDropdown;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> IsExistsPatientProfileDetails(long patientId)
        {
            var patient = await _dbContext.Smartrx_PatientProfile
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == patientId && !p.IsDeleted);

            if (patient == null)
            {
                return false;
            }
            return true;
        }

        public async Task<PatientWithRelativesContract> EditPatientDetailsAsync(long patientId, long loginUserId, PatientWithRelativesContract patientDetails, CancellationToken cancellationToken)
        {
            // Fetch the existing patient record
            var patient = await _dbContext.Smartrx_PatientProfile
                .FirstOrDefaultAsync(p => p.Id == patientId && !p.IsDeleted);

            var heightUnit = await _dbContext.Configuration_Unit.Where(u => u.Type == "Vital" && u.Name == "Height" && u.MeasurementUnit == patientDetails.HeightMeasurementUnit.ToLower()).FirstOrDefaultAsync(cancellationToken);
            var weightUnit = await _dbContext.Configuration_Unit.Where(u => u.Type == "Vital" && u.Name == "Weight" && u.MeasurementUnit == patientDetails.WeightMeasurementUnit.ToLower()).FirstOrDefaultAsync(cancellationToken);


            // Update fields using null coalescing operator
            if(patientDetails.FirstName!=null) patient!.FirstName = patientDetails.FirstName;
            if(patientDetails.LastName!=null) patient.LastName = patientDetails.LastName;
            if(patientDetails.NickName!=null) patient.NickName = patientDetails.NickName;
            if(patientDetails.Email!=null) patient.Email = patientDetails.Email;
            if(patientDetails.Age!=null) patient.Age = patientDetails.Age;
            if(patientDetails.AgeYear!=null) patient.AgeYear = patientDetails.AgeYear;
            if(patientDetails.AgeMonth!=null) patient.AgeMonth = patientDetails.AgeMonth;
            if(patientDetails.DateOfBirth!=null) patient.DateOfBirth = patientDetails.DateOfBirth;
            if(patientDetails.Gender!=null) patient.Gender = patientDetails.Gender ?? 0;
            if(patientDetails.Height!=null) patient.Height = patientDetails.Height;
            if(patientDetails.HeightFeet!=null && patientDetails.HeightFeet >= 0) patient.HeightFeet = patientDetails.HeightFeet;
            if(patientDetails.HeightInches!=null) patient.HeightInches = patientDetails.HeightInches;
            patient.HeightMeasurementUnitId = heightUnit!.Id;
            if(patientDetails.Weight!=null) patient.Weight = patientDetails.Weight??0;
            patient.WeightMeasurementUnitId = weightUnit!.Id;
            if(patientDetails.BloodGroup!=null) patient.BloodGroup = patientDetails.BloodGroup;
            if(patientDetails.PhoneNumber!=null) patient.PhoneNumber = patientDetails.PhoneNumber;
            // Save generated image name/path if provided; otherwise keep values from DTO (which may be existing)
            if(!string.IsNullOrWhiteSpace(patientDetails.ProfilePhotoName)) patient.ProfilePhotoName =patientDetails.ProfilePhotoName;
            if(!string.IsNullOrWhiteSpace(patientDetails.ProfilePhotoPath)) patient.ProfilePhotoPath = patientDetails.ProfilePhotoPath;
            if(!string.IsNullOrWhiteSpace(patientDetails.Address)) patient.Address = patientDetails.Address;
            if(patientDetails.CityId !=null) patient.CityId = patientDetails.CityId;
            if(patientDetails.PoliceStationId !=null) patient.PoliceStationId = patientDetails.PoliceStationId;
            if(patientDetails.PostalCode!=null) patient.PostalCode = patientDetails.PostalCode;
            if(patientDetails.EmergencyContact!=null) patient.EmergencyContact = patientDetails.EmergencyContact;
            if (patientDetails.MaritalStatus != null) patient.MaritalStatus = patientDetails.MaritalStatus;
            if(!string.IsNullOrEmpty(patientDetails.Profession)) patient.Profession = patientDetails.Profession;
            patient.IsExistingPatient = patientDetails.IsExistingPatient??false;
            patient.ExistingPatientId =(patientDetails.IsExistingPatient != null && patientDetails.IsExistingPatient==true) ? patientDetails.ExistingPatientId : null;
            patient.IsRelative = patientDetails.IsRelative??false;
            //if(patientDetails.IsRelative!=null && patientDetails.IsRelative==true) patient.RelationToPatient = patientDetails.RelationToPatient;
            //if(patientDetails.RelatedToPatientId!=null) patient.RelatedToPatientId = patientDetails.RelatedToPatientId;
            if(patientDetails.ProfileProgress!=null) patient.ProfileProgress = patientDetails.ProfileProgress??0;
            
            patient.ModifiedById = loginUserId;
            patient.ModifiedDate = DateTime.Now;


            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(patient).Reload();


            List<SmartRx_PatientProfileEntity> patientRelatives = new List<SmartRx_PatientProfileEntity>();
            patientRelatives = await _dbContext.Smartrx_PatientProfile
                                    .AsNoTracking()
                                    .Where(p => p.RelatedToPatientId.HasValue && p.RelatedToPatientId.Value == patientId && !p.IsDeleted)
                                    .ToListAsync();
            // Relatives: update only relation fields for existing relative rows
            if (patientDetails.Relatives!=null && patientDetails.Relatives.Count > 0)
            {
                if (patientRelatives != null && patientRelatives.Count > 0)
                {
                    foreach (var relative in patientRelatives)
                    {
                        var rel = patientDetails.Relatives.Where(p => p.RelatedToPatientId == relative.Id).FirstOrDefault();
                        if (rel != null)
                        {
                            relative.IsRelative = true;
                            relative.RelatedToPatientId = patientId;
                            relative.RelationToPatient = rel.RelationToPatient;
                            relative.ModifiedById = loginUserId;
                            relative.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            relative.IsRelative = false;
                            relative.RelatedToPatientId = null;
                            relative.RelationToPatient = null;
                            relative.ModifiedById = loginUserId;
                            relative.ModifiedDate = DateTime.Now;
                        }
                    }
                }
                else
                {
                    patientRelatives = new List<SmartRx_PatientProfileEntity>();
                    foreach (var relative in patientDetails.Relatives)
                    {
                        var rel = await _dbContext.Smartrx_PatientProfile
                                    .AsNoTracking()
                                    .Where(p => p.Id == relative.RelatedToPatientId && !p.IsDeleted).FirstOrDefaultAsync();
                        if (rel!=null)
                        {
                            rel.IsRelative = true;
                            rel.RelatedToPatientId =patientId;
                            rel.RelationToPatient = relative.RelationToPatient;
                            rel.ModifiedById = loginUserId;
                            rel.ModifiedDate = DateTime.Now;
                        }
                        patientRelatives.Add(rel);  
                    }

                }
            }
            else
            {
                foreach (var relative in patientRelatives)
                {
                    relative.IsRelative = false;
                    relative.RelatedToPatientId = null;
                    relative.RelationToPatient = null;
                    relative.ModifiedById = loginUserId;
                    relative.ModifiedDate = DateTime.Now;
                }               
            }
            if (patientRelatives.Count > 0)
            {
                await _dbContext.SaveChangesAsync();
                foreach (var relative in patientRelatives)
                {
                    _dbContext.Entry(relative).Reload();
                }
            }

            var pt = new PatientWithRelativesContract()
            {
                Id = patient.Id,
                PatientCode = patient.PatientCode,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                NickName = patient.NickName,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Gender = patient.Gender,

                Age = patient.Age,
                AgeYear = patient.AgeYear,
                AgeMonth = patient.AgeMonth,

                Height = patient.HeightFeet + "ft " + patient.HeightInches + "in",
                HeightFeet = patient.HeightFeet ?? 0,
                HeightInches = patient.HeightInches ?? 0,
                HeightMeasurementUnit = heightUnit.MeasurementUnit,
                HeightMeasurementUnitId = patient.HeightMeasurementUnitId,
                Weight = patient.Weight,
                WeightMeasurementUnit = weightUnit.MeasurementUnit,
                WeightMeasurementUnitId = patient.WeightMeasurementUnitId,

                DateOfBirth = patient.DateOfBirth,
                BloodGroup = patient.BloodGroup,
                ProfilePhotoName = patient.ProfilePhotoName,
                ProfilePhotoPath = patient.ProfilePhotoPath,
                Address = patient.Address,
                CityId = patient.CityId,
                PoliceStationId = patient.PoliceStationId,
                PostalCode = patient.PostalCode,
                EmergencyContact = patient.EmergencyContact,
                MaritalStatus = patient.MaritalStatus,
                Profession = patient.Profession,
                IsExistingPatient = patient.IsExistingPatient,
                ExistingPatientId = patient.ExistingPatientId,
                IsRelative = patient.IsRelative,
                RelatedToPatientId = patient.RelatedToPatientId,
                RelationToPatient = patient.RelationToPatient,
                ProfileProgress = patient.ProfileProgress,

                IsActive = patient.IsActive,
                TotalPrescriptions = _dbContext.Smartrx_Master
                    .Where(sm => sm.PatientId == patientId && 
                                sm.IsRecommended == true && 
                                sm.IsApproved == true && 
                                sm.IsCompleted == true)
                    .Count(),
                RxType = _dbContext.Smartrx_Master
                    .Where(sm => sm.PatientId == patientId && 
                                sm.IsRecommended == true && 
                                sm.IsApproved == true && 
                                sm.IsCompleted == true)
                    .Any() ? "SmartRx" :
                    _dbContext.Prescription_UploadedPrescription
                    .Where(pr => pr.PatientId == patientId && 
                                pr.IsSmartRxRequested == true && 
                                (pr.IsCompleted == null || pr.IsCompleted == false))
                    .Any() ? "Waiting" : "File Only"
            };

            var relatives = await _dbContext.Smartrx_PatientProfile
                .AsNoTracking()
                .Where(p => p.IsRelative == true && p.RelatedToPatientId == patientId && !p.IsDeleted)
                .ToListAsync();
            if(relatives != null)
            {
                var relativesContract = relatives!.Select(p => new RelativeContract()
                {
                    Id = p.Id,
                    RelatedToPatientId = p.RelatedToPatientId,
                    IsRelative = p.IsRelative,
                    RelationToPatient = p.RelationToPatient,

                }).ToList();
                pt.Relatives = relativesContract!;
            }
            return pt;
        }

        public async Task<IList<PatientWithRelativesContract>> GetAllPatientProfilesByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var patientProfiles = await _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .Where(p => p.UserId== userId && p.IsActive == true && !p.IsDeleted)
                    .Select(p => new PatientWithRelativesContract
                    {
                        Id = p.Id,
                        PatientCode = p.PatientCode,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        NickName = p.NickName,
                        Age = p.Age,
                        AgeYear = p.AgeYear,
                        AgeMonth = p.AgeMonth,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        GenderString = p.Gender == (int)Gender.Male ? Gender.Male.ToString() : Gender.Female.ToString(),
                        BloodGroup = p.BloodGroup,
                        Height = p.Height,
                        HeightFeet = p.HeightFeet ?? 0,
                        HeightInches = p.HeightInches ?? 0,
                        HeightMeasurementUnit = p.HeightUnit.Name,
                        Weight = p.Weight,
                        WeightMeasurementUnit = p.WeightUnit.Name,
                        PhoneNumber = p.PhoneNumber,
                        Email = p.Email,
                        ProfilePhotoName = p.ProfilePhotoName,
                        ProfilePhotoPath = p.ProfilePhotoPath,
                        Address = p.Address,
                        PoliceStationId = p.PoliceStationId,
                        CityId = p.CityId,
                        PostalCode = p.PostalCode,
                        EmergencyContact = p.EmergencyContact,
                        MaritalStatus = p.MaritalStatus,
                        Profession = p.Profession,
                        IsExistingPatient = p.IsExistingPatient,
                        ExistingPatientId = p.ExistingPatientId,
                        ProfileProgress = p.ProfileProgress,
                        Relatives = _dbContext.Smartrx_PatientProfile
                            .AsNoTracking()
                            .Where(r => r.RelatedToPatientId == p.Id && r.IsRelative == true && !r.IsDeleted)
                            .Select(r => new RelativeContract
                            {
                                Id = r.Id,
                                PatientCode = r.PatientCode,
                                FirstName = r.FirstName,
                                LastName = r.LastName,
                                NickName = r.NickName,
                                Age = r.Age,
                                AgeYear = r.AgeYear,
                                AgeMonth = r.AgeMonth,
                                DateOfBirth = r.DateOfBirth,
                                Gender = r.Gender,
                                BloodGroup = r.BloodGroup,
                                Height = r.Height,
                                PhoneNumber = r.PhoneNumber,
                                Email = r.Email,
                                ProfilePhotoName = r.ProfilePhotoName,
                                ProfilePhotoPath = r.ProfilePhotoPath,
                                Address = r.Address,
                                PoliceStationId = r.PoliceStationId,
                                CityId = r.CityId,
                                PostalCode = r.PostalCode,
                                EmergencyContact = r.EmergencyContact,
                                MaritalStatus = r.MaritalStatus,
                                Profession = r.Profession,
                                IsExistingPatient = r.IsExistingPatient,
                                ExistingPatientId = r.ExistingPatientId,
                                IsRelative = r.IsRelative,
                                RelatedToPatientId = r.RelatedToPatientId,
                                RelationToPatient = r.RelationToPatient,
                                ProfileProgress = r.ProfileProgress
                            })
                            .ToList(),
                        IsActive = p.IsActive,
                        TotalPrescriptions = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Count(),
                        RxType = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Any() ? "SmartRx" :
                            _dbContext.Prescription_UploadedPrescription
                            .AsNoTracking()
                            .Where(pr => pr.PatientId == p.Id && 
                                        pr.IsSmartRxRequested == true && 
                                        (pr.IsCompleted == null || pr.IsCompleted == false))
                            .Any() ? "Waiting" : "File Only"
                    })
                    .ToListAsync(cancellationToken);

                return patientProfiles;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load patient profiles for user: " + ex.Message);
            }
        }

        public async Task<PaginatedResult<PatientWithRelativesContract>> GetAllPatientProfilesByUserIdWithPagingAsync(long userId, string? RxType, string? searchKeyword, string? searchColumn, PagingSortingParams? pagingSorting, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (pagingSorting == null)
                {
                    pagingSorting = new PagingSortingParams();
                }

                if (pagingSorting.PageNumber <= 0) pagingSorting.PageNumber = 1;
                if (pagingSorting.PageSize <= 0) pagingSorting.PageSize = 10;

                var baseQuery = _dbContext.Smartrx_PatientProfile
                    .AsNoTracking()
                    .Where(p => p.UserId == userId && p.IsActive == true && !p.IsDeleted)
                    .Select(p => new PatientWithRelativesContract
                    {
                        Id = p.Id,
                        PatientCode = p.PatientCode,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        NickName = p.NickName,
                        Age = p.Age,
                        AgeYear = p.AgeYear,
                        AgeMonth = p.AgeMonth,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        GenderString = p.Gender == (int)Gender.Male ? Gender.Male.ToString() : Gender.Female.ToString(),
                        BloodGroup = p.BloodGroup,
                        Height = p.Height,
                        HeightFeet = p.HeightFeet ?? 0,
                        HeightInches = p.HeightInches ?? 0,
                        HeightMeasurementUnit = p.HeightUnit.Name,
                        Weight = p.Weight,
                        WeightMeasurementUnit = p.WeightUnit.Name,
                        PhoneNumber = p.PhoneNumber,
                        Email = p.Email,
                        ProfilePhotoName = p.ProfilePhotoName,
                        ProfilePhotoPath = p.ProfilePhotoPath,
                        Address = p.Address,
                        PoliceStationId = p.PoliceStationId,
                        CityId = p.CityId,
                        PostalCode = p.PostalCode,
                        EmergencyContact = p.EmergencyContact,
                        MaritalStatus = p.MaritalStatus,
                        Profession = p.Profession,
                        IsExistingPatient = p.IsExistingPatient,
                        ExistingPatientId = p.ExistingPatientId,
                        ProfileProgress = p.ProfileProgress,
                        Relatives = _dbContext.Smartrx_PatientProfile
                            .AsNoTracking()
                            .Where(r => r.RelatedToPatientId == p.Id && r.IsRelative == true && !r.IsDeleted)
                            .Select(r => new RelativeContract
                            {
                                Id = r.Id,
                                PatientCode = r.PatientCode,
                                FirstName = r.FirstName,
                                LastName = r.LastName,
                                NickName = r.NickName,
                                Age = r.Age,
                                AgeYear = r.AgeYear,
                                AgeMonth = r.AgeMonth,
                                DateOfBirth = r.DateOfBirth,
                                Gender = r.Gender,
                                BloodGroup = r.BloodGroup,
                                Height = r.Height,
                                PhoneNumber = r.PhoneNumber,
                                Email = r.Email,
                                ProfilePhotoName = r.ProfilePhotoName,
                                ProfilePhotoPath = r.ProfilePhotoPath,
                                Address = r.Address,
                                PoliceStationId = r.PoliceStationId,
                                CityId = r.CityId,
                                PostalCode = r.PostalCode,
                                EmergencyContact = r.EmergencyContact,
                                MaritalStatus = r.MaritalStatus,
                                Profession = r.Profession,
                                IsExistingPatient = r.IsExistingPatient,
                                ExistingPatientId = r.ExistingPatientId,
                                IsRelative = r.IsRelative,
                                RelatedToPatientId = r.RelatedToPatientId,
                                RelationToPatient = r.RelationToPatient,
                                ProfileProgress = r.ProfileProgress
                            })
                            .ToList(),
                        IsActive = p.IsActive,
                        TotalPrescriptions = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Count(),
                        RxType = _dbContext.Smartrx_Master
                            .AsNoTracking()
                            .Where(sm => sm.PatientId == p.Id && 
                                        sm.IsRecommended == true && 
                                        sm.IsApproved == true && 
                                        sm.IsCompleted == true)
                            .Any() ? "Smart Rx" :
                            _dbContext.Prescription_UploadedPrescription
                            .AsNoTracking()
                            .Where(pr => pr.PatientId == p.Id && 
                                        pr.IsSmartRxRequested == true && 
                                        (pr.IsCompleted == null || pr.IsCompleted == false))
                            .Any() ? "Waiting" : "File Only"
                    });

                // Apply search filter only if SearchKeyword is provided
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    var searchTerm = searchKeyword.Trim().ToLower();
                    
                    // Only apply search if the keyword is not empty after trimming
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        switch (searchColumn?.ToLower())
                        {
                            case "firstname":
                                baseQuery = baseQuery.Where(x => x.FirstName != null && x.FirstName.ToLower().Contains(searchTerm));
                                break;
                            case "lastname":
                                baseQuery = baseQuery.Where(x => x.LastName != null && x.LastName.ToLower().Contains(searchTerm));
                                break;
                            case "patientcode":
                                baseQuery = baseQuery.Where(x => x.PatientCode != null && x.PatientCode.ToLower().Contains(searchTerm));
                                break;
                            case "nickname":
                                baseQuery = baseQuery.Where(x => x.NickName != null && x.NickName.ToLower().Contains(searchTerm));
                                break;
                            case "phonenumber":
                                baseQuery = baseQuery.Where(x => x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(searchTerm));
                                break;
                            case "email":
                                baseQuery = baseQuery.Where(x => x.Email != null && x.Email.ToLower().Contains(searchTerm));
                                break;
                            case "address":
                                baseQuery = baseQuery.Where(x => x.Address != null && x.Address.ToLower().Contains(searchTerm));
                                break;
                            case "profession":
                                baseQuery = baseQuery.Where(x => x.Profession != null && x.Profession.ToLower().Contains(searchTerm));
                                break;
                            case "emergencycontact":
                                baseQuery = baseQuery.Where(x => x.EmergencyContact != null && x.EmergencyContact.ToLower().Contains(searchTerm));
                                break;
                            case "all":
                            default:
                                baseQuery = baseQuery.Where(x => 
                                    (x.FirstName != null && x.FirstName.ToLower().Contains(searchTerm)) ||
                                    (x.LastName != null && x.LastName.ToLower().Contains(searchTerm)) ||
                                    (x.PatientCode != null && x.PatientCode.ToLower().Contains(searchTerm)) ||
                                    (x.NickName != null && x.NickName.ToLower().Contains(searchTerm)) ||
                                    (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(searchTerm)) ||
                                    (x.Email != null && x.Email.ToLower().Contains(searchTerm)) ||
                                    (x.Address != null && x.Address.ToLower().Contains(searchTerm)) ||
                                    (x.Profession != null && x.Profession.ToLower().Contains(searchTerm)) ||
                                    (x.EmergencyContact != null && x.EmergencyContact.ToLower().Contains(searchTerm)));
                                break;
                        }
                    }
                }

                // Get total count - filter by RxType if provided, otherwise return all
                var countQuery = baseQuery;
                if (!string.IsNullOrWhiteSpace(RxType))
                {
                    countQuery = countQuery.Where(p => p.RxType == RxType);
                }
                var totalRecords = await countQuery.CountAsync();

                // Apply sorting
                IQueryable<PatientWithRelativesContract> sortedQuery;
                switch (pagingSorting.SortBy?.ToLower())
                {
                    case "firstname":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.FirstName)
                            : baseQuery.OrderBy(x => x.FirstName);
                        break;
                    case "lastname":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.LastName)
                            : baseQuery.OrderBy(x => x.LastName);
                        break;
                    case "patientcode":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.PatientCode)
                            : baseQuery.OrderBy(x => x.PatientCode);
                        break;
                    case "age":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.Age)
                            : baseQuery.OrderBy(x => x.Age);
                        break;
                    case "dateofbirth":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.DateOfBirth)
                            : baseQuery.OrderBy(x => x.DateOfBirth);
                        break;
                    case "createddate":
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.Id) // Using ID as proxy for created date
                            : baseQuery.OrderBy(x => x.Id);
                        break;
                    default:
                        sortedQuery = pagingSorting.SortDirection.ToLower() == "desc"
                            ? baseQuery.OrderByDescending(x => x.FirstName)
                            : baseQuery.OrderBy(x => x.FirstName);
                        break;
                }

                // Apply paging - filter by RxType if provided, otherwise return all
                var filteredQuery = sortedQuery;
                if (!string.IsNullOrWhiteSpace(RxType))
                {
                    filteredQuery = filteredQuery.Where(p => p.RxType == RxType);
                }
                
                var pagedData = await filteredQuery
                    .Skip((pagingSorting.PageNumber - 1) * pagingSorting.PageSize)
                    .Take(pagingSorting.PageSize)
                    .ToListAsync();

                return new PaginatedResult<PatientWithRelativesContract>(
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
                throw new Exception("Failed to load patient profiles with paging for user: " + ex.Message);
            }
        }

        public async Task<PatientWithRelativesContract> CreatePatientDetailsAsync(long userId, PatientWithRelativesContract patientDetails, CancellationToken cancellationToken)
        {
            try
            {
                // Generate patient code
                var patientCode = await _codeGenerationService.GeneratePatientCodeAsync();

                var heightUnit = await _dbContext.Configuration_Unit.Where(u => u.Type == "Vital" && u.Name == "Height" && u.MeasurementUnit == patientDetails.HeightMeasurementUnit.ToLower()).FirstOrDefaultAsync(cancellationToken);
                var weightUnit = await _dbContext.Configuration_Unit.Where(u => u.Type == "Vital" && u.Name == "Weight" && u.MeasurementUnit == patientDetails.WeightMeasurementUnit.ToLower()).FirstOrDefaultAsync(cancellationToken);

                // Create new patient entity
                    var newPatient = new SmartRx_PatientProfileEntity
                {
                    PatientCode = patientCode,
                    FirstName = patientDetails.FirstName ?? string.Empty,
                    LastName = patientDetails.LastName ?? string.Empty,
                    NickName = patientDetails.NickName,
                    Age = patientDetails.Age,
                    AgeYear = patientDetails.AgeYear,
                    AgeMonth = patientDetails.AgeMonth,
                    DateOfBirth = patientDetails.DateOfBirth ?? DateTime.Now,
                    Gender = patientDetails.Gender ?? 0,
                    BloodGroup = patientDetails.BloodGroup,
                    Height = patientDetails.Height ?? string.Empty,
                    HeightFeet = patientDetails.HeightFeet,
                    HeightInches = patientDetails.HeightInches,
                    HeightMeasurementUnitId = heightUnit?.Id,
                    Weight = patientDetails.Weight ?? 0,
                    WeightMeasurementUnitId = weightUnit?.Id,
                    PhoneNumber = patientDetails.PhoneNumber ?? string.Empty,
                    Email = patientDetails.Email ?? string.Empty,
                    ProfilePhotoName = patientDetails.ProfilePhotoName,
                    ProfilePhotoPath = patientDetails.ProfilePhotoPath,
                    Address = patientDetails.Address ?? string.Empty,
                    PoliceStationId = patientDetails.PoliceStationId,
                    CityId = patientDetails.CityId,
                    PostalCode = patientDetails.PostalCode,
                    EmergencyContact = patientDetails.EmergencyContact,
                    MaritalStatus = patientDetails.MaritalStatus,
                    Profession = patientDetails.Profession,
                    IsExistingPatient = patientDetails.IsExistingPatient ?? false,
                    ExistingPatientId = patientDetails.ExistingPatientId,
                    IsRelative = patientDetails.IsRelative ?? false,
                    RelationToPatient = patientDetails.RelationToPatient,
                    RelatedToPatientId = patientDetails.RelatedToPatientId,
                    ProfileProgress = patientDetails.ProfileProgress ?? 0,
                    IsActive = patientDetails.IsActive ?? true,
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    CreatedById = userId
                };

                _dbContext.Smartrx_PatientProfile.Add(newPatient);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Handle relatives if provided
                //if (patientDetails.Relatives != null && patientDetails.Relatives.Count > 0)
                //{
                //    var relatives = new List<SmartRx_PatientProfileEntity>();
                //    foreach (var r in patientDetails.Relatives)
                //    {
                //        //var relativeCode = await GeneratePatientCodeAsync();
                //        relatives.Add(new SmartRx_PatientProfileEntity
                //        {
                //            PatientCode = relativeCode,
                //            FirstName = r.FirstName ?? string.Empty,
                //            LastName = r.LastName ?? string.Empty,
                //            NickName = r.NickName,
                //            Age = r.Age,
                //            AgeYear = r.AgeYear,
                //            AgeMonth = r.AgeMonth,
                //            DateOfBirth = r.DateOfBirth ?? DateTime.Now,
                //            Gender = r.Gender,
                //            BloodGroup = r.BloodGroup,
                //            Height = r.Height ?? string.Empty,
                //            PhoneNumber = r.PhoneNumber ?? string.Empty,
                //            Email = r.Email ?? string.Empty,
                //            ProfilePhotoName = r.ProfilePhotoName,
                //            ProfilePhotoPath = r.ProfilePhotoPath,
                //            Address = r.Address ?? string.Empty,
                //            PoliceStationId = r.PoliceStationId,
                //            CityId = r.CityId,
                //            PostalCode = r.PostalCode,
                //            EmergencyContact = r.EmergencyContact,
                //            MaritalStatus = r.MaritalStatus,
                //            Profession = r.Profession,
                //            IsExistingPatient = r.IsExistingPatient ?? false,
                //            ExistingPatientId = r.ExistingPatientId,
                //            IsRelative = true,
                //            RelationToPatient = r.RelationToPatient,
                //            RelatedToPatientId = newPatient.Id,
                //            ProfileProgress = r.ProfileProgress ?? 0,
                //            IsActive = r.IsActive == 1,
                //            UserId = userId,
                //            CreatedDate = DateTime.Now,
                //            CreatedById = userId
                //        });
                //    }

                //    _dbContext.Smartrx_PatientProfile.AddRange(relatives);
                //    await _dbContext.SaveChangesAsync(cancellationToken);
                //}

                // Return the created patient with relatives
                return await GetPatientProfileWithRelativesById(newPatient.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create patient profile: " + ex.Message);
            }
        }

       
    }
}
