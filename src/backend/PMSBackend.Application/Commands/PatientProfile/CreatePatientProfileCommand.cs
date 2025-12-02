using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PMSBackend.Application.Commands.PrescriptionUpload;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PMSBackend.Application.Commands.PatientProfile
{
    public class CreatePatientProfileCommand : IRequest<PatientWithRelativesDTO>
    {       
        public PatientWithRelativesDTO? PatientDetails { get; set; }        
        public long LoginUserId { get; set; }
    }

    public class CreatePatientProfileCommandHandler : IRequestHandler<CreatePatientProfileCommand, PatientWithRelativesDTO>
    {
        private readonly IPatientProfileRepository _repository;
        private readonly ILogger<CreatePatientProfileCommandHandler> _logger;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;
        
        public CreatePatientProfileCommandHandler(
            IPatientProfileRepository repository,
            ILogger<CreatePatientProfileCommandHandler> logger,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _logger = logger;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PatientWithRelativesDTO> Handle(CreatePatientProfileCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new PatientWithRelativesDTO();

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.PatientDetails?.FirstName))
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "First name is required"
                    };
                    return responseResult;
                }

                if (string.IsNullOrWhiteSpace(request.PatientDetails?.LastName))
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Last name is required"
                    };
                    return responseResult;
                }
                if (request.PatientDetails?.Age == null || request.PatientDetails.Age < 0|| request.PatientDetails.AgeYear ==null ||request.PatientDetails.AgeYear<=0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Valid age is required"
                    };
                    return responseResult;
                }

                if (request.PatientDetails?.Gender==null|| request.PatientDetails?.Gender<=0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Gender is required"
                    };
                    return responseResult;
                }


                if (request.PatientDetails?.HeightFeet == null || request.PatientDetails?.HeightFeet <= 0)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Height is required"
                    };
                    return responseResult;
                }

                PatientWithRelativesContract pt = new PatientWithRelativesContract();
                pt.FirstName = request.PatientDetails!.FirstName;
                pt.LastName = request.PatientDetails.LastName;
                pt.NickName = request.PatientDetails.NickName;
                pt.Age = request.PatientDetails.Age;
                pt.AgeYear = request.PatientDetails.AgeYear;
                pt.AgeMonth = request.PatientDetails.AgeMonth;
                pt.Gender = request.PatientDetails.Gender ?? 0;
                pt.DateOfBirth = request.PatientDetails.DateOfBirth;
                pt.BloodGroup = request.PatientDetails.BloodGroup;
                pt.Height = request.PatientDetails.Height;
                pt.HeightFeet = request.PatientDetails.HeightFeet;
                pt.HeightInches = request.PatientDetails.HeightInches;
                pt.HeightMeasurementUnit = request.PatientDetails.HeightMeasurementUnit;

                pt.Weight = request.PatientDetails.Weight;
                pt.WeightMeasurementUnit = request.PatientDetails.WeightMeasurementUnit;

                // Handle profile photo
                pt.ProfilePhotoName = request.PatientDetails.ProfilePhotoName;
                pt.ProfilePhotoPath = request.PatientDetails.ProfilePhotoPath;

                // If a new file was uploaded, override with generated name/path
                if (request.PatientDetails.ProfilePhoto != null && request.PatientDetails.ProfilePhoto.Length > 0)
                {
                    pt.ProfilePhotoName = $"PatientProfilePhoto_{request.PatientDetails.PatientCode}_thumbnail.jpg";
                    pt.ProfilePhotoPath = $"photos\\PatientProfilePhoto_{request.PatientDetails.PatientCode}_thumbnail.jpg";
                }

                pt.PhoneNumber = request.PatientDetails.MobileNo;
                pt.Email = request.PatientDetails.Email;
                pt.Address = request.PatientDetails.Address;
                pt.PoliceStationId = request.PatientDetails.PoliceStationId;
                pt.CityId = request.PatientDetails.CityId;
                pt.PostalCode = request.PatientDetails.PostalCode;
                pt.EmergencyContact = request.PatientDetails.EmergencyContact;
                pt.MaritalStatus = request.PatientDetails.MaritalStatus;
                pt.Profession = request.PatientDetails.Profession;
                pt.IsExistingPatient = request.PatientDetails.IsExistingPatient ?? false;
                pt.ExistingPatientId = request.PatientDetails.ExistingPatientId;
                pt.ProfileProgress = request.PatientDetails.ProfileProgress ?? 0;
                pt.IsActive = true; // Set as active by default for new patients

                // Handle relatives if provided
                if(request.PatientDetails.Relatives is not null)
                {
                    pt.Relatives = request!.PatientDetails!.Relatives!.Select(r => new RelativeContract()
                    {
                        FirstName = r.FirstName ?? string.Empty,
                        LastName = r.LastName ?? string.Empty,
                        NickName = r.NickName,
                        Age = r.Age,
                        AgeYear = r.AgeYear,
                        AgeMonth = r.AgeMonth,
                        DateOfBirth = r.DateOfBirth,
                        Gender = r.Gender ?? 0,
                        BloodGroup = r.BloodGroup,
                        Height = r.Height ?? string.Empty,
                        PhoneNumber = r.PhoneNumber ?? string.Empty,
                        Email = r.Email ?? string.Empty,
                        ProfilePhotoName = r.ProfilePhotoName,
                        ProfilePhotoPath = r.ProfilePhotoPath,
                        Address = r.Address,
                        PoliceStationId = r.PoliceStationId,
                        CityId = r.CityId,
                        PostalCode = r.PostalCode,
                        EmergencyContact = r.EmergencyContact,
                        MaritalStatus = r.MaritalStatus,
                        Profession = r.Profession,
                        IsExistingPatient = r.IsExistingPatient ?? false,
                        ExistingPatientId = r.ExistingPatientId,
                        RelationToPatient = r.RelationToPatient,
                        ProfileProgress = r.ProfileProgress,
                        IsRelative = true,
                        IsActive = r.IsActive ?? 1
                    }).ToList();
                }
                
                var createdPatientInfo = await _repository.CreatePatientDetailsAsync(request.LoginUserId, pt, cancellationtoken);

                if (createdPatientInfo == null)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Status = "Failed",
                        Message = "Failed to create patient profile"
                    };
                    return responseResult;
                }

                // Map the created patient info to response DTO
                responseResult.Id = createdPatientInfo.Id ?? 0;
                responseResult.PatientCode = createdPatientInfo.PatientCode;
                responseResult.FirstName = createdPatientInfo.FirstName;
                responseResult.LastName = createdPatientInfo.LastName;
                responseResult.NickName = createdPatientInfo.NickName;
                responseResult.Age = createdPatientInfo.Age;
                responseResult.AgeYear = createdPatientInfo.AgeYear;
                responseResult.AgeMonth = createdPatientInfo.AgeMonth;
                responseResult.Gender = createdPatientInfo.Gender ?? 0;
                responseResult.DateOfBirth = createdPatientInfo.DateOfBirth;
                responseResult.BloodGroup = createdPatientInfo.BloodGroup;
                responseResult.Height = createdPatientInfo.Height;
                responseResult.HeightFeet = createdPatientInfo.HeightFeet ?? 0;
                responseResult.HeightInches = createdPatientInfo.HeightInches ?? 0;
                responseResult.HeightMeasurementUnit = createdPatientInfo.HeightMeasurementUnit;
                responseResult.HeightMeasurementUnitId = createdPatientInfo.HeightMeasurementUnitId;
                responseResult.Weight = createdPatientInfo.Weight;
                responseResult.WeightMeasurementUnit = createdPatientInfo.WeightMeasurementUnit;
                responseResult.WeightMeasurementUnitId = createdPatientInfo.WeightMeasurementUnitId;
                responseResult.MobileNo = createdPatientInfo.PhoneNumber;
                responseResult.Email = createdPatientInfo.Email;
                responseResult.Profession = createdPatientInfo.Profession;
                responseResult.ProfilePhotoName = createdPatientInfo.ProfilePhotoName;
                responseResult.ProfilePhotoPath = createdPatientInfo.ProfilePhotoPath;
                responseResult.Address = createdPatientInfo.Address;
                responseResult.PoliceStationId = createdPatientInfo.PoliceStationId;
                responseResult.CityId = createdPatientInfo.CityId;
                responseResult.PostalCode = createdPatientInfo.PostalCode;
                responseResult.EmergencyContact = createdPatientInfo.EmergencyContact;
                responseResult.MaritalStatus = createdPatientInfo.MaritalStatus;
                responseResult.Profession = createdPatientInfo.Profession;
                responseResult.IsExistingPatient = createdPatientInfo.IsExistingPatient;
                responseResult.ExistingPatientId = createdPatientInfo.ExistingPatientId;
                responseResult.ProfileProgress = createdPatientInfo.ProfileProgress ?? 0;
                responseResult.IsActive = createdPatientInfo.IsActive;

                // Handle relatives in response
                if(createdPatientInfo.Relatives is not null)
                {
                    responseResult.RelativesDropdown = new List<PatientDropdown>();

                    foreach (var relative in createdPatientInfo.Relatives)
                    {
                        var rel = new PatientDropdown()
                        {
                            PatientId = relative.Id,
                            PatientName = relative.FirstName + " " + relative.LastName + " " + relative.NickName
                        };
                        responseResult.RelativesDropdown.Add(rel);
                    }
                    
                    responseResult.Relatives = createdPatientInfo.Relatives!.Select(p => new RelativeDTO()
                    {
                        Id = p.Id,
                        PatientCode = p.PatientCode,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        NickName = p.NickName,
                        Age = p.Age,
                        AgeYear = p.AgeYear,
                        AgeMonth = p.AgeMonth,
                        Gender = p.Gender,
                        DateOfBirth = p.DateOfBirth,
                        BloodGroup = p.BloodGroup,
                        Height = p.Height,
                        PhoneNumber = p.PhoneNumber,
                        Email = p.Email,
                        ProfilePhotoName = p.ProfilePhotoName,
                        ProfilePhotoPath = p.ProfilePhotoPath,
                        Address = p.Address,
                        PoliceStationId = p.PoliceStationId,
                        CityId = p.CityId,
                        PostalCode = p.PostalCode,
                        EmergencyContact = p.EmergencyContact,
                        MaritalStatus = p.MaritalStatus,
                        Profession = p.Profession,
                        IsExistingPatient = p.IsExistingPatient,
                        ExistingPatientId = p.ExistingPatientId,
                        IsRelative = p.IsRelative,
                        RelationToPatient = p.RelationToPatient,
                        ProfileProgress = p.ProfileProgress.Value,
                        IsActive = p.IsActive
                    }).ToList();
                }

                // Save reward transaction for creating patient profile
                var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                    request.LoginUserId,
                    0,                                 // no prescription
                    null,                              // no SmartRx master
                    createdPatientInfo.Id,             // created patient as context
                    "ADD_NEW_PATIENT",
                    "creating a patient profile",
                    cancellationtoken);

                if (rewardResult is not null && rewardResult.IsRewardUpdated)
                {
                    responseResult.IsRewardUpdated = true;
                    responseResult.RewardTitle = rewardResult.RewardTitle;
                    responseResult.TotalRewardPoints = rewardResult.Points;
                    responseResult.RewardMessage = rewardResult.RewardMessage;
                }
               
                return responseResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient profile");
                throw;
            }
        }
    }
}
