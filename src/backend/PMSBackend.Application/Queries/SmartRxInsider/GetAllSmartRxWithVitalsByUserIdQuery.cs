using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetAllSmartRxWithVitalsByUserIdQuery : IRequest<PaginatedResult<SmartRxWithVitalsDTO>>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
        public string? VitalName { get; set; }
        public string? SearchKeyword { get; set; }
        public string? SearchColumn { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public PagingSortingParams PagingSorting { get; set; }
    }

    public class GetAllSmartRxWithVitalsByUserIdQueryHandler : IRequestHandler<GetAllSmartRxWithVitalsByUserIdQuery, PaginatedResult<SmartRxWithVitalsDTO>>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetAllSmartRxWithVitalsByUserIdQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }

        public async Task<PaginatedResult<SmartRxWithVitalsDTO>> Handle(GetAllSmartRxWithVitalsByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _smartRxInsiderRepository.GetAllSmartRxWithVitalsByUserIdWithPagingAsync(
                    request.UserId,
                    request.PatientId,
                    request.VitalName,
                    request.SearchKeyword, 
                    request.SearchColumn, 
                    request.FromDate,
                    request.ToDate,
                    request.PagingSorting, 
                    cancellationToken);

                if (result is null || !result.Data.Any())
                {
                    return new PaginatedResult<SmartRxWithVitalsDTO>(
                        new List<SmartRxWithVitalsDTO>(),
                        0,
                        request.PagingSorting.PageNumber,
                        request.PagingSorting.PageSize,
                        request.PagingSorting.SortBy,
                        request.PagingSorting.SortDirection,
                        "No SmartRx with vitals found for this user!", null, null, null, null);
                }

                var smartRxWithVitals = result.Data.Select(s => new SmartRxWithVitalsDTO
                {
                    SmartRxId = s.SmartRxId,
                    PatientId = s.PatientId,
                    PrescriptionId = s.PrescriptionId,
                    PrescriptionDate = s.PrescriptionDate,
                    Remarks = s.Remarks,
                    IsRecommended = s.IsRecommended,
                    IsApproved = s.IsApproved,
                    IsCompleted = s.IsCompleted,
                    RecommendedDate = s.RecommendedDate,
                    ApprovedDate = s.ApprovedDate,
                    CompletedDate = s.CompletedDate,
                    Tag1 = s.Tag1,
                    Tag2 = s.Tag2,
                    Tag3 = s.Tag3,
                    Tag4 = s.Tag4,
                    Tag5 = s.Tag5,
                    PatientInfo = s.PatientInfo != null ? new SmartRxPatientProfileWithVitalsDTO
                    {
                        Id = s.PatientInfo.Id,
                        PatientCode = s.PatientInfo.PatientCode,
                        FirstName = s.PatientInfo.FirstName,
                        LastName = s.PatientInfo.LastName,
                        NickName = s.PatientInfo.NickName,
                        Age = s.PatientInfo.Age,
                        AgeYear = s.PatientInfo.AgeYear,
                        AgeMonth = s.PatientInfo.AgeMonth,
                        DateOfBirth = s.PatientInfo.DateOfBirth,
                        Gender = s.PatientInfo.Gender,
                        GenderString = s.PatientInfo.GenderString,
                        BloodGroup = s.PatientInfo.BloodGroup,
                        Height = s.PatientInfo.Height,
                        HeightFeet = s.PatientInfo.HeightFeet,
                        HeightInches = s.PatientInfo.HeightInches,
                        HeightMeasurementUnit = s.PatientInfo.HeightMeasurementUnit,
                        Weight = s.PatientInfo.Weight,
                        WeightMeasurementUnit = s.PatientInfo.WeightMeasurementUnit,
                        PhoneNumber = s.PatientInfo.PhoneNumber,
                        Email = s.PatientInfo.Email,
                        ProfilePhotoName = s.PatientInfo.ProfilePhotoName,
                        ProfilePhotoPath = s.PatientInfo.ProfilePhotoPath,
                        Address = s.PatientInfo.Address,
                        PoliceStationId = s.PatientInfo.PoliceStationId,
                        CityId = s.PatientInfo.CityId,
                        PostalCode = s.PatientInfo.PostalCode,
                        EmergencyContact = s.PatientInfo.EmergencyContact,
                        MaritalStatus = s.PatientInfo.MaritalStatus,
                        Profession = s.PatientInfo.Profession,
                        IsExistingPatient = s.PatientInfo.IsExistingPatient,
                        ExistingPatientId = s.PatientInfo.ExistingPatientId,
                        ProfileProgress = s.PatientInfo.ProfileProgress,
                        IsActive = s.PatientInfo.IsActive,
                        TotalPrescriptions = s.PatientInfo.TotalPrescriptions,
                        RxType = s.PatientInfo.RxType
                    } : null,
                    Vitals = s.Vitals?.Select(v => new SmartRxVitalDTO
                    {
                        Id = v.Id,
                        SmartRxMasterId = v.SmartRxMasterId,
                        PrescriptionId = v.PrescriptionId,
                        VitalId = v.VitalId,
                        VitalName = v.VitalName,
                        ApplicableEntity=v.ApplicableEntity,
                        VitalValue = v.VitalValue,
                        VitalUnit = v.VitalUnit,
                        VitalStatus = v.VitalStatus ?? "",
                        HeightFeet = v.HeightFeet,
                        HeightInches = v.HeightInches,
                        CreatedById = v.CreatedById,
                        CreatedDate = v.CreatedDate,
                        CreatedDateStr = v.CreatedDateStr ?? ""
                    }).ToList()
                }).ToList();

                return new PaginatedResult<SmartRxWithVitalsDTO>(
                    smartRxWithVitals,
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