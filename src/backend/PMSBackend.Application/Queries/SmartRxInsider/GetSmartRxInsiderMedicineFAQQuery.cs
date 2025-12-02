using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxInsiderMedicineFAQQuery : IRequest<MedicineFAQListDTO>
    {
        public long MedicineId { get; set; }


    }

    public class GetSmartRxInsiderMedicineFAQQueryHandler : IRequestHandler<GetSmartRxInsiderMedicineFAQQuery, MedicineFAQListDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetSmartRxInsiderMedicineFAQQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }
        public async Task<MedicineFAQListDTO> Handle(GetSmartRxInsiderMedicineFAQQuery request, CancellationToken cancellationToken)
        {
            try
            {
                MedicineFAQListDTO responseResult = new MedicineFAQListDTO();
                List<MedicineFAQDTO> listFAQ = new List<MedicineFAQDTO>();
                var smartRxMedicineFAQ = await _smartRxInsiderRepository.GetPatientSingleFAQDrugInformationByIdAsync(request.MedicineId);
                if (smartRxMedicineFAQ == null || smartRxMedicineFAQ.Count <= 0)
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


                Parallel.ForEach(smartRxMedicineFAQ, faq =>
                {
                    var medicineFAQ = new MedicineFAQDTO()
                    {
                        Id = faq.Id,
                        MedicineId = faq.MedicineId,
                        Question = faq.Question,
                        Answer = faq.Answer,
                    };
                    listFAQ.Add(medicineFAQ);
                });
                responseResult.ApiResponseResult = null;
                responseResult.medicineFAQList = listFAQ;

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