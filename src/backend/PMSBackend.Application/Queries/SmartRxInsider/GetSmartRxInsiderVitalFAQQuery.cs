using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxInsiderVitalFAQQuery : IRequest<VitalFAQListDTO>
    {
        public long VitalId { get; set; }
    }

    public class GetSmartRxInsiderVitalFAQQueryHandler : IRequestHandler<GetSmartRxInsiderVitalFAQQuery, VitalFAQListDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetSmartRxInsiderVitalFAQQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }
        public async Task<VitalFAQListDTO> Handle(GetSmartRxInsiderVitalFAQQuery request, CancellationToken cancellationToken)
        {
            try
            {
                VitalFAQListDTO responseResult = new VitalFAQListDTO();
                List<VitalFAQDTO> listFAQ = new List<VitalFAQDTO>();
                var smartRxVitalFAQs = await _smartRxInsiderRepository.GetPatientSingleVitalFAQByVitalId(request.VitalId);
                if (smartRxVitalFAQs == null || smartRxVitalFAQs.Count <= 0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No medicine FAQ found!"
                    };
                    return responseResult;
                }


                Parallel.ForEach(smartRxVitalFAQs, faq =>
                {
                    var vitalFAQ = new VitalFAQDTO()
                    {
                        Id = faq.Id,
                        VitalId = faq.VitalId,
                        Question = faq.Question,
                        Answer = faq.Answer,
                    };
                    listFAQ.Add(vitalFAQ);
                });
                responseResult.ApiResponseResult = null;
                responseResult.vitalFAQDTOs = listFAQ;

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