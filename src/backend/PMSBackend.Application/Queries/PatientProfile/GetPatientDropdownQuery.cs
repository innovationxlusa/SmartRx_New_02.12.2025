using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PatientProfile
{
    public class GetPatientDropdownQuery : IRequest<PatientDropdownDTO>
    {
        public long UserId { get; set; }
    }

    public class GetPatientDropdownQueryHandler : IRequestHandler<GetPatientDropdownQuery, PatientDropdownDTO>
    {
        private readonly IPatientProfileRepository _patientProfileRepository;

        public GetPatientDropdownQueryHandler(IPatientProfileRepository patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }
        public async Task<PatientDropdownDTO> Handle(GetPatientDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                PatientDropdownDTO responseResult = new PatientDropdownDTO();
                var pts = await _patientProfileRepository.GetPatientDropdownInfoAsync(request.UserId, cancellationToken);
                if (pts is null)
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
                responseResult.patientDropdowns = pts;
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

