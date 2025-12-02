using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class EditSmartRxInvestigationWishlistCommand : IRequest<SmartRxInvestigationWishlistsDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long InvestigationId { get; set; }
        public List<long> WishListIds { get; set; }
        public long LoginUserId { get; set; }
    }
    public class EditSmartRxInvestigationWishlistCommandHandler : IRequestHandler<EditSmartRxInvestigationWishlistCommand, SmartRxInvestigationWishlistsDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxInsiderRepository _smartRxRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public EditSmartRxInvestigationWishlistCommandHandler(
            ISmartRxInsiderRepository smartRxRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxRepository = smartRxRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<SmartRxInvestigationWishlistsDTO> Handle(EditSmartRxInvestigationWishlistCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                SmartRxInvestigationWishlistsDTO responseResult = new SmartRxInvestigationWishlistsDTO();

                //// Validation: Check input
                //if (request.WishListIds == null)
                //{
                //    responseResult.ApiResponseResult = new ApiResponseResult
                //    {
                //        Data = null,
                //        StatusCode = StatusCodes.Status404NotFound,
                //        Status = "Failed",
                //        Message = "Wishlist cannot be null.",
                //    };
                //    return responseResult;
                //}

                // Validation: Check for duplicates
                if (request.WishListIds is not null && request.WishListIds.Count != request.WishListIds.Distinct().Count())
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Failed",
                        Message = "Duplicate Wishlist IDs found.",
                    };
                    return responseResult;
                }

                //// Validation: Check for negative or zero IDs
                //if (request.WishListIds is not null && request.WishListIds.Any(id => id <= 0))
                //{
                //    var testCenters = await _smartRxRepository.GetAllTestCenter(cancellationtoken);
                //    var testCenterIds = testCenters?.Select(tc => tc.TestCenterId).ToList() ?? new List<long>();
                //    if (testCenterIds is not null)
                //    {
                //        if (request.WishListIds.Any(id => testCenterIds.Contains(id)))
                //        {
                //            responseResult.ApiResponseResult = new ApiResponseResult
                //            {
                //                Data = null,
                //                StatusCode = StatusCodes.Status400BadRequest,
                //                Status = "Failed",
                //                Message = "Wishlist contains invalid IDs.",
                //            };
                //            return responseResult;
                //        }
                //    }
                //}

                var investigations = await _smartRxRepository.GetPatientInvestigationListBySmartRxId(request.SmartRxMasterId, request.PrescriptionId, cancellationtoken);
                if (investigations == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "This investigation not found.",
                    };
                    return responseResult;
                }
                SmartRxInvestigationWishListDTO inv = new SmartRxInvestigationWishListDTO();
                List<SmartRxInvestigationWishListDTO> invs = new List<SmartRxInvestigationWishListDTO>();

                foreach (var investigation in investigations.Tests)
                {
                    investigation.Wishlist =request.WishListIds==null?null: string.Join(",", request.WishListIds!);
                    investigation.ModifiedById = request.LoginUserId;
                    investigation.ModifiedDate = DateTime.UtcNow;
                    var updatedData = await _smartRxRepository.UpdateInvestigationWishlistAsync(investigation, cancellationtoken);
                    if (updatedData is not null)
                    {
                        inv = new SmartRxInvestigationWishListDTO()
                        {
                            Id = updatedData.Id,
                            TestCode = updatedData.InvestigationTest.Code,
                            TestName = updatedData.InvestigationTest.TestName,
                            WishList = updatedData.Wishlist,
                            PrescriptionId = updatedData.PrescriptionId,
                            SmartRxMasterId = updatedData.SmartRxMasterId,
                            TestId = updatedData.TestId,
                            TestCenterIds = updatedData.UserSelectedTestCenterIds,
                            Wished = true,
                        };
                        invs.Add(inv);
                    }
                }
                responseResult.investigationWishlist = invs;

                responseResult.ApiResponseResult = null;

                // Save reward transaction for investigation wishlist (following InsertPrescriptionUploadCommand pattern)
                if (invs != null && invs.Any())
                {
                    var prescription = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.PrescriptionId);
                    if (prescription != null)
                    {
                        var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                            request.LoginUserId,
                            request.PrescriptionId,
                            request.SmartRxMasterId,
                            prescription.PatientId,
                            "INVESTIGATION_WISHLIST",
                            "updating investigation wishlist",
                            cancellationtoken);

                        if (rewardResult is not null && rewardResult.IsRewardUpdated)
                        {
                            responseResult.IsRewardUpdated = true;
                            responseResult.RewardTitle = rewardResult.RewardTitle;
                            responseResult.TotalRewardPoints = rewardResult.Points;
                            responseResult.RewardMessage = rewardResult.RewardMessage;
                        }
                    }
                }

                await Task.CompletedTask;

                return responseResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
