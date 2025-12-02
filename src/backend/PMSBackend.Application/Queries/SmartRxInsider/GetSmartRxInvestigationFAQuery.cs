using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxInvestigationFAQuery : IRequest<InvestigationFAQListDTO>
    {
        public long InvestigationId { get; set; }
    }

    public class GetSmartRxInvestigationFAQueryHandler : IRequestHandler<GetSmartRxInvestigationFAQuery, InvestigationFAQListDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetSmartRxInvestigationFAQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }
        public async Task<InvestigationFAQListDTO> Handle(GetSmartRxInvestigationFAQuery request, CancellationToken cancellationToken)
        {
            try
            {
                InvestigationFAQListDTO responseResult = new InvestigationFAQListDTO();
                List<InvestigationFAQDTO> listFAQ = new List<InvestigationFAQDTO>();
                var smartRxInvestigationFAQs = await _smartRxInsiderRepository.GetPatientSingleTestFAQByIdAsync(request.InvestigationId);
                if (smartRxInvestigationFAQs == null || smartRxInvestigationFAQs.Count <= 0)
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


                Parallel.ForEach(smartRxInvestigationFAQs, faq =>
                {
                    var invFAQ = new InvestigationFAQDTO()
                    {
                        Id = faq.Id,
                        InvestigationId = faq.InvestigationId,
                        Question = faq.Question,
                        Answer = faq.Answer,
                    };
                    listFAQ.Add(invFAQ);
                });
                responseResult.ApiResponseResult = null;
                responseResult.investigationFAQs = listFAQ;

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