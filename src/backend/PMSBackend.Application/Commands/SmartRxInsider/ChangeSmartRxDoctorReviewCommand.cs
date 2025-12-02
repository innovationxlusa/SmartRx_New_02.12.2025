using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PMSBackend.Application.Commands.SmartRxInsider
{
    public class ChangeSmartRxDoctorReviewCommand:IRequest<SmartRxDoctorDTO>
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public long DoctorId { get; set; }
        //Time
        public int? TravelTimeMinute { get; set; }
        public int? WaitingTimeHour { get; set; }
        public int? WaitingTimeMinute { get; set; }
        public int? DoctorConsultingDuration { get; set; }// Doctor visit time
        //Cost
        public decimal? FeeCharged { get; set; }//doctor fee
        public long? FeeChargedMeasurementUnitId { get; set; } = 17;
        public string? FeeChargedMeasurementUnit { get; set; } = "৳";
        public decimal? TransportCost { get; set; }
        public decimal? OtherCost { get; set; }


        //Rating
        public decimal? Rating { get; set; }        
        public string Comments { get; set; }
        public long LoginUserId { get; set; }

        public bool IsDoctorReviewed { get; set; }
        public bool IsCostAdded { get; set; }
        public bool IsTimeAdded { get; set; }


    }
    public class ChangeSmartRxDoctorReviewCommandHandler : IRequestHandler<ChangeSmartRxDoctorReviewCommand, SmartRxDoctorDTO>
    {
        // private readonly ILogger<InsertPrescriptionUploadCommandHandler> _logger;
        private readonly ISmartRxInsiderRepository _smartRxRepository;
        private readonly IPrescriptionUploadRepository _prescriptionUploadRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public ChangeSmartRxDoctorReviewCommandHandler(
            ISmartRxInsiderRepository smartRxRepository,
            IPrescriptionUploadRepository prescriptionUploadRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _smartRxRepository = smartRxRepository;
            _prescriptionUploadRepository = prescriptionUploadRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<SmartRxDoctorDTO?> Handle(ChangeSmartRxDoctorReviewCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                SmartRxDoctorDTO responseResult = new SmartRxDoctorDTO();               
                var smartrxDoctors = await _smartRxRepository.GetPatientDoctorsListBySmartRxId(request.SmartRxMasterId, request.PrescriptionId,cancellationtoken);
                if(smartrxDoctors is null || !smartrxDoctors.Any())
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Doctor not found for this SmartRx.",
                    };
                    return responseResult;
                }
                var smartrxDoctor = await _smartRxRepository.UpdateSmartRxDoctorReviewByUser(request.SmartRxMasterId, request.PrescriptionId, request.DoctorId, 
                   request.TravelTimeMinute, request.WaitingTimeHour, request.WaitingTimeMinute, request.DoctorConsultingDuration, request.FeeCharged, request.FeeChargedMeasurementUnitId, 
                   request.TransportCost, request.OtherCost, request.Rating, request.Comments,request.LoginUserId, cancellationtoken);
                if (smartrxDoctor == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status404NotFound,
                        Status = "Failed",
                        Message = "Doctor review not updated.",
                    };
                    return responseResult;
                }               

                responseResult.SmartRxMasterId = smartrxDoctor.SmartRxMasterId;
                responseResult.PrescriptionId = smartrxDoctor.PrescriptionId;
                responseResult.DoctorId = smartrxDoctor.DoctorId;

                responseResult.TravelTimeMinute = smartrxDoctor.TravelTimeMinute;
                responseResult.ChamberWaitTimeMinute = smartrxDoctor.ChamberWaitTimeMinute;
                responseResult.ChamberWaitTimeHour = smartrxDoctor.ChamberWaitTimeHour;
                responseResult.ConsultingDurationInMinutes = smartrxDoctor.ConsultingDurationInMinutes;

                responseResult.ChamberFee = smartrxDoctor.ChamberFee;
                responseResult.ChamberFeeMeasurementUnit = smartrxDoctor.ChamberFeeMeasurementUnit;
                responseResult.TransportFee = smartrxDoctor.TransportFee;
                responseResult.OtherExpense = smartrxDoctor.OtherExpense;
               
                responseResult.DoctorRating = smartrxDoctor.DoctorRating;
                responseResult.Comments = smartrxDoctor.Comments;

                responseResult.PatientDoctor = new DoctorProfileDTO()
                {
                    DoctorId = smartrxDoctor.DoctorId,
                    DoctorFirstName = smartrxDoctor.PatientDoctor.DoctorFirstName,
                    DoctorLastName = smartrxDoctor.PatientDoctor.DoctorLastName,
                    ProfilePhotoName = smartrxDoctor.PatientDoctor.ProfilePhotoName,
                    ProfilePhotoPath = smartrxDoctor.PatientDoctor.ProfilePhotoPath,
                    DoctorSpecializedArea = smartrxDoctor.PatientDoctor.DoctorSpecializedArea,
                    DoctorBMDCRegNo = smartrxDoctor.PatientDoctor.DoctorBMDCRegNo,
                    DoctorCode = smartrxDoctor.PatientDoctor.DoctorCode,
                };
                if(smartrxDoctor.DoctorEducations == null || !smartrxDoctor.DoctorEducations.Any())
                {
                    responseResult.DoctorEducations = new List<EducationDTO>();
                }
                else
                {
                    responseResult.DoctorEducations = smartrxDoctor.DoctorEducations!.Select(e => new EducationDTO()
                    {
                        EducationCode = e.EducationCode,
                        EducationDegreeName = e.EducationDegreeName,
                        EducationDescription = e.EducationDescription,
                        EducationId = e.EducationId,
                        EducationInstitutionName = e.EducationInstitutionName
                    }).ToList();
                }
                    
                if(smartrxDoctor.DoctorChambers == null || !smartrxDoctor.DoctorChambers.Any())
                {
                    responseResult.Chambers = new List<SmartRxDoctorChamberDTO>();
                    
                }
                else
                {
                    responseResult.Chambers = smartrxDoctor.DoctorChambers.Select(c => new SmartRxDoctorChamberDTO()
                    {
                        ChamberName = c.ChamberName,
                        ChamberAddress = c.ChamberAddress,
                        ChamberGoogleAddress = c.ChamberGoogleAddress,
                        ChamberCityName = c.ChamberCityName,
                        ChamberCloseDay = c.ChamberCloseDay,
                        ChamberDescription = c.ChamberDescription,
                        ChamberDoctorBookingMobileNos = c.ChamberDoctorBookingMobileNos,
                        ChamberEmail = c.ChamberEmail,
                        ChamberEndTime = c.ChamberEndTime,
                        ChamberGoogleRating = c.ChamberGoogleRating,
                        ChamberHelpline = c.ChamberHelpline,
                        ChamberOpenDay = c.ChamberOpenDay,
                        ChamberOtherDoctorsId = c.ChamberOtherDoctorsId,
                        ChamberPostalCode = c.ChamberPostalCode,
                        ChamberStartTime = c.ChamberStartTime,
                        ChamberVisitingHour = c.ChamberVisitingHour,
                        DepartmentName = c.DepartmentName,
                        DepartmentSectionName = c.DepartmentSectionName,
                        DoctorDesignaitonInChamber = c.DoctorDesignaitonInChamber,
                        DoctorId = c.DoctorId,
                        DoctorSpecialization = c.DoctorSpecialization,
                        HospitalName = c.HospitalName,
                        IsMainChamber = c.IsMainChamber,
                        VisitingHour = c.VisitingHour,
                        Remarks = c.Remarks,
                        IsActive = c.IsActive,
                    }).ToList();
                }
                responseResult.ApiResponseResult = null;

                // Save reward transactions conditionally (following InsertPrescriptionUploadCommand pattern)
                if (smartrxDoctor != null)
                {
                    var prescription = await _prescriptionUploadRepository.GetDetailsByIdAsync(request.PrescriptionId);
                    if (prescription != null)
                    {
                        double totalPoints = 0;
                        var rewardMessages = new List<string>();
                        var rewardTitles = new List<string>();

                        // Save reward for reviewing doctor if IsDoctorReviewed is true
                        if (request.IsDoctorReviewed)
                        {
                            var reviewRewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                                request.LoginUserId,
                                request.PrescriptionId,
                                request.SmartRxMasterId,
                                prescription.PatientId,
                                "REVIEW_DOCTOR",
                                "reviewing a doctor",
                                cancellationtoken);

                            if (reviewRewardResult is not null && reviewRewardResult.IsRewardUpdated)
                            {
                                totalPoints += reviewRewardResult.Points;
                                rewardMessages.Add(reviewRewardResult.RewardMessage);
                                rewardTitles.Add(reviewRewardResult.RewardTitle);
                            }
                        }

                        // Save reward for adding cost if IsCostAdded is true
                        if (request.IsCostAdded)
                        {
                            var costRewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                                request.LoginUserId,
                                request.PrescriptionId,
                                request.SmartRxMasterId,
                                prescription.PatientId,
                                "ADD_COST_ON_DOCTOR_VISIT",
                                "adding cost for doctor visit",
                                cancellationtoken);

                            if (costRewardResult is not null && costRewardResult.IsRewardUpdated)
                            {
                                totalPoints += costRewardResult.Points;
                                rewardMessages.Add(costRewardResult.RewardMessage);
                                rewardTitles.Add(costRewardResult.RewardTitle);
                            }
                        }

                        // Save reward for adding time if IsTimeAdded is true
                        if (request.IsTimeAdded)
                        {
                            var timeRewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                                request.LoginUserId,
                                request.PrescriptionId,
                                request.SmartRxMasterId,
                                prescription.PatientId,
                                "ADD_TIME_ON_DOCTOR_VISIT",
                                "adding time for doctor visit",
                                cancellationtoken);

                            if (timeRewardResult is not null && timeRewardResult.IsRewardUpdated)
                            {
                                totalPoints += timeRewardResult.Points;
                                rewardMessages.Add(timeRewardResult.RewardMessage);
                                rewardTitles.Add(timeRewardResult.RewardTitle);
                            }
                        }

                        // Update response with reward information if any rewards were saved
                        if (totalPoints > 0)
                        {
                            responseResult.IsRewardUpdated = true;
                            responseResult.RewardTitle = string.Join(", ", rewardTitles);
                            responseResult.TotalRewardPoints = totalPoints;
                            responseResult.RewardMessage = string.Join(" ", rewardMessages);
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
