using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class EditSmartRxMedicineWishlistCommand : IRequest<SmartRxMedicineWishListsDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long SourceMedicineId { get; set; }
        public List<long> WishListIds { get; set; }
        public long LoginUserId { get; set; }
    }
    public class EditSmartRxMedicineWishlistCommandHandler : IRequestHandler<EditSmartRxMedicineWishlistCommand, SmartRxMedicineWishListsDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxInsiderRepository _smartRxRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public EditSmartRxMedicineWishlistCommandHandler(
            ISmartRxInsiderRepository smartRxRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxRepository = smartRxRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<SmartRxMedicineWishListsDTO?> Handle(EditSmartRxMedicineWishlistCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                SmartRxMedicineWishListsDTO responseResult = new SmartRxMedicineWishListsDTO();

                // Validation: Check input
                if (request.WishListIds == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Wishlist cannot be null.",
                    };
                    return responseResult;
                }

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

                // Validation: Check for negative or zero IDs
                if (request.WishListIds is not null && request.WishListIds.Any(id => id <= 0))
                {
                    var allMedicines = await _smartRxRepository.GetAllMedicine(cancellationtoken);
                    var medicineIds = allMedicines?.Select(tc => tc.Id).ToList();
                    if (medicineIds is not null)
                    {
                        if (request.WishListIds.Any(id => !medicineIds.Contains(id)))
                        {
                            responseResult.ApiResponseResult = new ApiResponseResult
                            {
                                Data = null,
                                StatusCode = StatusCodes.Status400BadRequest,
                                Status = "Failed",
                                Message = "Wishlist contains invalid IDs.",
                            };
                            return responseResult;
                        }
                    }
                }
                var sourceMedicine = await _smartRxRepository.GetPatientSingleMedicineBySmartRxId(request.SmartRxMasterId, request.PrescriptionId, request.SourceMedicineId, cancellationtoken);
                if (sourceMedicine == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Medicines not found.",
                    };
                    return responseResult;
                }
                SmartRxMedicineWishListDTO med = new SmartRxMedicineWishListDTO();
                List<SmartRxMedicineWishListDTO> meds = new List<SmartRxMedicineWishListDTO>();
                sourceMedicine.Wishlist = string.Join(",", request.WishListIds);
                sourceMedicine.ModifiedById = request.LoginUserId;
                sourceMedicine.ModifiedDate = DateTime.UtcNow;
                var updatedData = await _smartRxRepository.UpdateMedicineWishlistAsync(sourceMedicine, cancellationtoken);
                if (updatedData is not null)
                {
                    med = new SmartRxMedicineWishListDTO()
                    {
                        Id = updatedData.Id,
                        MedicineId = updatedData.MedicineId,
                        PrescriptionId = updatedData.PrescriptionId,
                        SmartRxMasterId = updatedData.SmartRxMasterId,
                        Wished = true,
                    };
                    meds.Add(med);
                }

                responseResult.MedicineWishlist = meds;
                responseResult.ApiResponseResult = null;

                // Save reward transaction for medicine wishlist (following InsertPrescriptionUploadCommand pattern)
                if (updatedData != null)
                {
                    var prescription = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.PrescriptionId);
                    if (prescription != null)
                    {
                        var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                            request.LoginUserId,
                            request.PrescriptionId,
                            request.SmartRxMasterId,
                            prescription.PatientId,
                            "MEDICINE_WISHLIST",
                            "updating medicine wishlist",
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
