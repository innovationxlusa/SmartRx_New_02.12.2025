using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PatientProfile
{
    public class GetPatientProfileWithRelativesByIdQuery : IRequest<PatientWithRelativesDTO>
    {
        public long PatientId { get; set; }
    }

    public class GetPatientProfileWithRelativesByIdQueryHandler : IRequestHandler<GetPatientProfileWithRelativesByIdQuery, PatientWithRelativesDTO>
    {
        private readonly IPatientProfileRepository _patientProfileRepository;

        public GetPatientProfileWithRelativesByIdQueryHandler(IPatientProfileRepository patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }
        public async Task<PatientWithRelativesDTO> Handle(GetPatientProfileWithRelativesByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                PatientWithRelativesDTO responseResult = new PatientWithRelativesDTO();


                var patientProfile = await _patientProfileRepository.GetPatientProfileWithRelativesById(request.PatientId, cancellationToken);
                if (patientProfile is null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No patient details found!"
                    };
                    return responseResult;
                }

                responseResult.Id = patientProfile.Id ?? 0;
                responseResult.PatientCode = patientProfile.PatientCode;
                responseResult.FirstName = patientProfile.FirstName;
                responseResult.LastName = patientProfile.LastName;
                responseResult.NickName = patientProfile.NickName;
                responseResult.Age = patientProfile.Age;
                responseResult.AgeYear = patientProfile.AgeYear;
                responseResult.AgeMonth = patientProfile.AgeMonth;
                responseResult.Gender = patientProfile.Gender ?? 0;
                responseResult.DateOfBirth = patientProfile.DateOfBirth;
                responseResult.BloodGroup = patientProfile.BloodGroup;



                responseResult.Height = patientProfile.Height;
                responseResult.HeightFeet = patientProfile.HeightFeet ?? 0;
                responseResult.HeightInches = patientProfile.HeightInches ?? 0;
                responseResult.HeightMeasurementUnit = patientProfile.HeightMeasurementUnit;

                responseResult.Weight = patientProfile.Weight;
                responseResult.WeightMeasurementUnit= patientProfile.WeightMeasurementUnit;

                responseResult.MobileNo = patientProfile.PhoneNumber;
                responseResult.Email = patientProfile.Email;
                responseResult.ProfilePhotoName = patientProfile.ProfilePhotoName;
                responseResult.ProfilePhotoPath = patientProfile.ProfilePhotoPath;
                responseResult.Address = patientProfile.Address;
                responseResult.PoliceStationId = patientProfile.PoliceStationId;

                responseResult.CityId = patientProfile.CityId;
                responseResult.PostalCode = patientProfile.PostalCode;
                responseResult.EmergencyContact = patientProfile.EmergencyContact;
                responseResult.MaritalStatus = patientProfile.MaritalStatus;
                responseResult.Profession = patientProfile.Profession;
                responseResult.IsExistingPatient = patientProfile.IsExistingPatient;
                responseResult.ExistingPatientId = patientProfile.ExistingPatientId;
                responseResult.ProfileProgress = patientProfile.ProfileProgress??0;

                responseResult.Relatives = patientProfile.Relatives?.Where(p=> p.ExistingPatientId != patientProfile.Id).Select(p => new RelativeDTO()
                {
                    Id = p.Id,
                    PatientCode = p.PatientCode,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    NickName = p.NickName,
                    Age = p.Age,
                    AgeYear = p.AgeYear,
                    AgeMonth = p.AgeMonth,
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth,
                    BloodGroup = p.BloodGroup,
                    Height = p.Height,
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
                    IsRelative = p.IsRelative,
                    RelationToPatient = p.RelationToPatient,
                    ProfileProgress = p.ProfileProgress.Value,
                    IsActive = p.IsActive
                }).ToList();
                responseResult.ApiResponseResult = null;
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

