using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetAllTestCentersQuery : IRequest<TestCentersDTO>
    {

    }

    public class GetAllTestCentersQueryHandler : IRequestHandler<GetAllTestCentersQuery, TestCentersDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetAllTestCentersQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }
        public async Task<TestCentersDTO> Handle(GetAllTestCentersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                TestCentersDTO responseResult = new TestCentersDTO();
                List<TestCenterDTO> listOfTestCenter = new List<TestCenterDTO>();


                var smartRxInvestigatioCenterList = await _smartRxInsiderRepository.GetAllTestCenter(cancellationToken);
                if (smartRxInvestigatioCenterList == null || smartRxInvestigatioCenterList.Count <= 0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No test center found!"
                    };
                    return responseResult;
                }

                Parallel.ForEach(smartRxInvestigatioCenterList, test =>
                {
                    var testWithDiagnosticCenter = new TestCenterDTO()
                    {
                        TestCenterId = test.TestCenterId,
                        TestCenterName = test.TestCenterName,
                        TestCenterBranch = test.TestCenterBranch,
                        TestCenterLocation = test.TestCenterLocation,
                        TestCenterIcon = test.TestCenterIcon,
                        TestUnitPrice = test.TestUnitPrice,
                        TestPriceMeasurementUnit = test.TestPriceMeasurementUnit,
                    };
                    listOfTestCenter.Add(testWithDiagnosticCenter);
                });
                responseResult.TestCenters = listOfTestCenter;
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