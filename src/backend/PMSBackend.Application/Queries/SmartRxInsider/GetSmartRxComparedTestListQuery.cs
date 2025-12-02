using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.SmartRxInsider
{
    public class GetSmartRxComparedTestListQuery : IRequest<InvestigationCompareDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }

        public string? TestCenterName { get; set; }
        public bool IsRecommended { get; set; }
        public List<long> SourceTestIds { get; set; }
        public PagingSortingParams PagingSorting { get; set; }
    }

    public class GetSmartRxComparedTestListQueryHandler : IRequestHandler<GetSmartRxComparedTestListQuery, InvestigationCompareDTO>
    {
        private readonly ISmartRxInsiderRepository _smartRxInsiderRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public GetSmartRxComparedTestListQueryHandler(
            ISmartRxInsiderRepository smartRxInsiderRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxInsiderRepository = smartRxInsiderRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }
        public async Task<InvestigationCompareDTO> Handle(GetSmartRxComparedTestListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                InvestigationCompareDTO responseResult = new InvestigationCompareDTO();
                List<InvestigationTestDTO> listOfTestWithDiagnosticCenter = new List<InvestigationTestDTO>();
                PaginatedResult<InvestigationTestDTO> pagedCompareList = new PaginatedResult<InvestigationTestDTO>();
                PaginatedResult<InvestigationTestDTO> pagedOnlyTestCenterList = new PaginatedResult<InvestigationTestDTO>();

                var testCentersToCompare = await _smartRxInsiderRepository.GetHospitalsWithBranchAndTestsAsync(request.TestCenterName, request.SmartRxMasterId, request.PrescriptionId, request.PagingSorting, cancellationToken);
                if (testCentersToCompare == null || testCentersToCompare.Data.Count <= 0)
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
                Parallel.ForEach(testCentersToCompare.Data, test =>
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
                        //DiagnosticTestCenterCityId = test.DiagnosticTestCenterCityId,
                        //DiagnosticTestCenterCityCode = test.DiagnosticTestCenterCityCode!,
                        //DiagnosticTestCenterCityName = test.DiagnosticTestCenterCityName!,
                        //DiagnosticTestCenterDistrictId = test.DiagnosticTestCenterDistrictId,
                        //DiagnosticTestCenterDistrictCode = test.DiagnosticTestCenterDistrictCode!,
                        //DiagnosticTestCenterDistrictName = test.DiagnosticTestCenterDistrictName!,
                        //DiagnosticTestCenterDivisionId = test.DiagnosticTestCenterDivisionId,
                        //DiagnosticTestCenterCountryId = test.DiagnosticTestCenterDistrictId,
                        //DiagnosticTestCenterCountryCode = test.DiagnosticTestCenterCountryCode!,
                        //DiagnosticTestCenterCountryName = test.DiagnosticTestCenterCountryName!,
                        DiagnosticTestCenterYearEstablished = test.DiagnosticTestCenterYearEstablished!,
                        DiagnosticTestCenterGoogleRating = test.DiagnosticTestCenterGoogleRating!,
                        DiagnosticTestCenterGoogleLocation = test.DiagnosticTestCenterGoogleLocation,
                        DiagnosticTestCenterIsActive = test.DiagnosticTestCenterIsActive,
                        //DiagnosticTestCenterWiseTestId = test.DiagnosticTestCenterWiseTestId ?? 0,
                        //DiagnosticTestCenterBranchWiseTestId = test.DiagnosticTestCenterBranchWiseTestId??0,
                        //DiagnosticeTestCenterTestName = test.DiagnosticeTestCenterTestName,
                        //DiagnosticeTestCenterTestDescription = test.DiagnosticeTestCenterTestDescription,
                        //DiagnosticeTestCenterTestFullName = test.DiagnosticeTestCenterTestFullName,
                        //DiagnosticeTestCenterTestShortName = test.DiagnosticeTestCenterTestShortName,
                        //TestNameByDiagnosticCenter = test.TestNameByDiagnosticCenter,
                        //DiagnosticeTestCenterWiseTestUnitPrice = test.DiagnosticeTestCenterWiseTestUnitPrice ?? 0,
                        //DiagnosticeTestCenterWiseTestUnitPriceMeasurementUnit = test.DiagnosticeTestCenterTestUnitPriceMeasurementUnit,
                        //TestPriceMeasurementUnit = test.TestPriceMeasurementUnit,
                        //TestPriceName = test.TestPriceName,
                        //TestPriceType = test.TestPriceType,
                        //TestUnitPrice = test.TestUnitPrice ?? 0,
                        IsUserSelected = test.IsUserSelected,
                        IsDoctorRecommended = test.IsDoctorRecommended,
                        Wished = test.Wished

                    };
                    listOfTestWithDiagnosticCenter.Add(testWithDiagnosticCenter);
                });
                pagedOnlyTestCenterList = new PaginatedResult<InvestigationTestDTO>(listOfTestWithDiagnosticCenter, testCentersToCompare.TotalRecords, testCentersToCompare.PageNumber, testCentersToCompare.PageSize, testCentersToCompare.SortBy, testCentersToCompare.SortDirection, null, null, null, null, null);
                responseResult.TestCentersListWithBranch = pagedOnlyTestCenterList;

                var smartRxInvestigationCompareList = await _smartRxInsiderRepository.GetAllTestCenterWithPatientTestList(request.SmartRxMasterId, request.PrescriptionId, request.TestCenterName, request.SourceTestIds, request.PagingSorting, cancellationToken);
                if (smartRxInvestigationCompareList == null || smartRxInvestigationCompareList.Data.Count <= 0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "No test investigation center found!"
                    };
                    return responseResult;
                }
                listOfTestWithDiagnosticCenter = new List<InvestigationTestDTO>();
                Parallel.ForEach(smartRxInvestigationCompareList.Data, test =>
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
                        IsUserSelected = test.IsUserSelected,
                        IsDoctorRecommended = test.IsDoctorRecommended,
                        Wished = test.Wished

                    };
                    listOfTestWithDiagnosticCenter.Add(testWithDiagnosticCenter);
                });
                pagedCompareList = new PaginatedResult<InvestigationTestDTO>(listOfTestWithDiagnosticCenter, smartRxInvestigationCompareList.TotalRecords, smartRxInvestigationCompareList.PageNumber, smartRxInvestigationCompareList.PageSize, smartRxInvestigationCompareList.SortBy, smartRxInvestigationCompareList.SortDirection, null, null, null, null, null);
                responseResult.ComparedTestList = pagedCompareList;
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