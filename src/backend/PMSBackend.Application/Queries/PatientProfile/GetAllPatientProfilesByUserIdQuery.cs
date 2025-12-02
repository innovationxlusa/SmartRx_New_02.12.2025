using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.PatientProfile
{
    public class GetAllPatientProfilesByUserIdQuery : IRequest<PaginatedResult<PatientWithRelativesDTO>>
    {
        public long UserId { get; set; }
        public string? RxType { get; set; } = string.Empty; // Optional RxType filter
        public string? SearchKeyword { get; set; }
        public string? SearchColumn { get; set; }

        public PagingSortingParams? PagingSorting { get; set; } = new PagingSortingParams(); // Default initialization for Swagger
    }

    public class GetAllPatientProfilesByUserIdQueryHandler : IRequestHandler<GetAllPatientProfilesByUserIdQuery, PaginatedResult<PatientWithRelativesDTO>>
    {
        private readonly IPatientProfileRepository _patientProfileRepository;

        public GetAllPatientProfilesByUserIdQueryHandler(IPatientProfileRepository patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }

        public async Task<PaginatedResult<PatientWithRelativesDTO>> Handle(GetAllPatientProfilesByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (request.PagingSorting == null)
                {
                    request.PagingSorting = new PagingSortingParams();
                }

                var result = await _patientProfileRepository.GetAllPatientProfilesByUserIdWithPagingAsync(
                    request.UserId, 
                    request.RxType ?? string.Empty, 
                    request.SearchKeyword, 
                    request.SearchColumn, 
                    request.PagingSorting, 
                    cancellationToken);
                
                if (result is null || !result.Data.Any())
                {
                    return new PaginatedResult<PatientWithRelativesDTO>(
                        new List<PatientWithRelativesDTO>(),
                        0,
                        request.PagingSorting.PageNumber,
                        request.PagingSorting.PageSize,
                        request.PagingSorting.SortBy,
                        request.PagingSorting.SortDirection,
                        "No patient profiles found for this user!", null, null, null, null);
                }

                var patientProfiles = result.Data.Select(p => new PatientWithRelativesDTO
                {
                    Id = p.Id ?? 0,
                    PatientCode = p.PatientCode,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    NickName = p.NickName,
                    Age = p.Age,
                    AgeYear = p.AgeYear,
                    AgeMonth = p.AgeMonth,
                    Gender = p.Gender ?? 0,
                    DateOfBirth = p.DateOfBirth,
                    BloodGroup = p.BloodGroup,
                    Height = p.Height,
                    HeightFeet = p.HeightFeet ?? 0,
                    HeightInches = p.HeightInches ?? 0,
                    HeightMeasurementUnit = p.HeightMeasurementUnit,
                    Weight = p.Weight,
                    WeightMeasurementUnit = p.WeightMeasurementUnit,
                    MobileNo = p.PhoneNumber,
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
                    ProfileProgress = p.ProfileProgress ?? 0,
                    Relatives = p.Relatives?.Where(r => r.ExistingPatientId != p.Id).Select(r => new RelativeDTO()
                    {
                        Id = r.Id,
                        PatientCode = r.PatientCode,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        NickName = r.NickName,
                        Age = r.Age,
                        AgeYear = r.AgeYear,
                        AgeMonth = r.AgeMonth,
                        Gender = r.Gender,
                        DateOfBirth = r.DateOfBirth,
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
                        RelationToPatient = r.RelationToPatient,
                        ProfileProgress = r.ProfileProgress.Value,
                        IsActive = r.IsActive
                    }).ToList(),
                    IsActive = p.IsActive,
                    TotalPrescriptions = p.TotalPrescriptions,
                    RxType = p.RxType
                }).ToList();

                return new PaginatedResult<PatientWithRelativesDTO>(
                    patientProfiles,
                    result.TotalRecords,
                    result.PageNumber,
                    result.PageSize,
                    result.SortBy,
                    result.SortDirection,
                    null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
