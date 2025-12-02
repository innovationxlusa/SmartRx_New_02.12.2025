using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxRecommendedOrSelectedTestCenterListQuery : IRequest<InvestigationCompareDTO>
    {
        public string? TestCenterName { get; set; }
        public List<long> DoctorsTestList { get; set; }
        public long SmartrxId { get; set; }
        public long PrescriptionId { get; set; }
        public bool IsDoctorRecommended { get; set; } = false;
    }

    public class GetSmartRxRecommendedOrSelectedTestCenterListQueryHandler : IRequestHandler<GetSmartRxRecommendedOrSelectedTestCenterListQuery, InvestigationCompareDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;

        public GetSmartRxRecommendedOrSelectedTestCenterListQueryHandler(ISmartRxInsiderRepository smartRxInsiderRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
        }
        public async Task<InvestigationCompareDTO> Handle(GetSmartRxRecommendedOrSelectedTestCenterListQuery request, CancellationToken cancellationToken)
        {
            try
            {

                InvestigationCompareDTO responseResult = new InvestigationCompareDTO();
                List<InvestigationTestDTO> listOfTestWithDiagnosticCenter = new List<InvestigationTestDTO>();

                var smartRxInvestigationTestCenterList = await _smartRxInsiderRepository.GetAllTestWithPatientsSelectedCenterList(request.DoctorsTestList, request.SmartrxId, request.PrescriptionId, request.IsDoctorRecommended, cancellationToken);
                if (smartRxInvestigationTestCenterList == null || smartRxInvestigationTestCenterList.Count() <= 0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "No test center found!"
                    };
                    return responseResult;
                }

                Parallel.ForEach(smartRxInvestigationTestCenterList, test =>
                {
                    var testWithDiagnosticCenter = new InvestigationTestDTO()
                    {
                        InvestigationId = test.InvestigationId,
                        DiagnosticTestCenterId = test.DiagnosticTestCenterId,
                        DiagnosticTestCenterCode = test.DiagnosticTestCenterCode!,
                        DiagnosticTestCenterName = test.DiagnosticTestCenterName!,
                        DiagnosticTestCenterDescription = test.DiagnosticTestCenterDescription!,
                        DiagnosticTestCenterLocation = test.DiagnosticTestCenterLocation!,
                        DiagnosticTestCenterIcon = test.DiagnosticTestCenterIcon!,
                        //DiagnosticTestCenterBranchId = test.DiagnosticTestCenterBranchId,
                        DiagnosticTestCenterBranchName = test.DiagnosticTestCenterBranchName!,
                        //DiagnosticTestCenterBranchLocation = test.DiagnosticTestCenterBranchLocation!,
                        DiagnosticTestCenterAddress = test.DiagnosticTestCenterAddress!,
                        DiagnosticTestCenterCityId = test.DiagnosticTestCenterCityId,
                        DiagnosticTestCenterCityCode = test.DiagnosticTestCenterCityCode!,
                        DiagnosticTestCenterCityName = test.DiagnosticTestCenterCityName!,
                        DiagnosticTestCenterDistrictId = test.DiagnosticTestCenterDistrictId,
                        DiagnosticTestCenterDistrictCode = test.DiagnosticTestCenterDistrictCode!,
                        DiagnosticTestCenterDistrictName = test.DiagnosticTestCenterDistrictName!,
                        DiagnosticTestCenterDivisionId = test.DiagnosticTestCenterDivisionId,
                        DiagnosticTestCenterCountryId = test.DiagnosticTestCenterDistrictId,
                        DiagnosticTestCenterCountryCode = test.DiagnosticTestCenterCountryCode!,
                        DiagnosticTestCenterCountryName = test.DiagnosticTestCenterCountryName!,
                        DiagnosticTestCenterYearEstablished = test.DiagnosticTestCenterYearEstablished!,
                        DiagnosticTestCenterGoogleRating = test.DiagnosticTestCenterGoogleRating!,
                        DiagnosticTestCenterGoogleLocation = test.DiagnosticTestCenterGoogleLocation,
                        DiagnosticTestCenterIsActive = test.DiagnosticTestCenterIsActive,
                        DiagnosticTestCenterWiseTestId = test.DiagnosticTestCenterWiseTestId ?? 0,
                        //DiagnosticTestCenterBranchWiseTestId = test.DiagnosticTestCenterBranchWiseTestId??0,
                        DiagnosticeTestCenterTestName = test.DiagnosticeTestCenterTestName,

                        DiagnosticeTestCenterTestDescription = test.DiagnosticeTestCenterTestDescription,
                        DiagnosticeTestCenterTestFullName = test.DiagnosticeTestCenterTestFullName,
                        DiagnosticeTestCenterTestShortName = test.DiagnosticeTestCenterTestShortName,
                        TestNameByDiagnosticCenter = test.TestNameByDiagnosticCenter,
                        DiagnosticeTestCenterWiseTestUnitPrice = test.DiagnosticeTestCenterWiseTestUnitPrice ?? 0,
                        DiagnosticeTestCenterWiseTestUnitPriceMeasurementUnit = test.DiagnosticeTestCenterTestUnitPriceMeasurementUnit,
                        TestPriceMeasurementUnit = test.TestPriceMeasurementUnit,
                        TestPriceName = test.TestPriceName,
                        TestPriceType = test.TestPriceType,
                        TestUnitPrice = test.TestUnitPrice ?? 0,
                        IsDoctorRecommended = test.IsDoctorRecommended

                    };
                    listOfTestWithDiagnosticCenter.Add(testWithDiagnosticCenter);
                });
                responseResult.SelectedOrRecommendedTestList = listOfTestWithDiagnosticCenter;
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