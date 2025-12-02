using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.DoctorProfile
{
    public class GetDoctorProfileByIdQuery : IRequest<DoctorProfileDTO>
    {
        public long DoctorId { get; set; }
        
    }

    public class GetDoctorProfileByIdQueryHandler : IRequestHandler<GetDoctorProfileByIdQuery, DoctorProfileDTO>
    {
        private readonly IDoctorProfileRepository _doctorProfileRepository;
        public GetDoctorProfileByIdQueryHandler(IDoctorProfileRepository doctorProfileRepository)
        {
            _doctorProfileRepository = doctorProfileRepository;
        }
        public async Task<DoctorProfileDTO> Handle(GetDoctorProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                DoctorProfileDTO doctorDTO = new DoctorProfileDTO();
                var doctorProfile = await _doctorProfileRepository.GetDoctorProfileByIdAsync(request.DoctorId);
                if (doctorProfile is null)
                {
                    doctorDTO.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No doctor details found!"
                    };
                    return doctorDTO;
                }
                doctorDTO = new DoctorProfileDTO
                {
                    DoctorId = doctorProfile!.DoctorId,
                    DoctorCode = doctorProfile.DoctorCode,
                    DoctorTitle = doctorProfile.DoctorTitle,
                    DoctorFirstName = doctorProfile.DoctorFirstName,
                    DoctorLastName = doctorProfile.DoctorLastName,
                    DoctorEducationDegrees = doctorProfile.DoctorEducationDegrees!.Select(e => new EducationDTO()
                    {
                        EducationId = e.Id,
                        EducationDegreeName = e.DegreeName,
                        EducationCode = e.Code,
                        EducationDescription = e.Description,
                        EducationInstitutionName = e.InstitutionName
                    }).ToList(),
                    DoctorSpecializedArea = doctorProfile.DoctorSpecializedArea,
                    ProfilePhotoName = doctorProfile.ProfilePhotoName,
                    ProfilePhotoPath = doctorProfile.ProfilePhotoPath,
                    DoctorChambers = doctorProfile.DoctorChambers,
                    DoctorYearOfExperiences = doctorProfile.DoctorYearOfExperiences,
                    DoctorExperiences = doctorProfile.DoctorExperiences,
                    DoctorBMDCRegNo = doctorProfile.DoctorBMDCRegNo,
                    DoctorProfessionalSummary = doctorProfile.DoctorProfessionalSummary,
                    DoctorRating = doctorProfile.DoctorRating,
                    Comments = doctorProfile.Comments
                };
                doctorDTO.ApiResponseResult = null;
                await Task.CompletedTask;
                return doctorDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
