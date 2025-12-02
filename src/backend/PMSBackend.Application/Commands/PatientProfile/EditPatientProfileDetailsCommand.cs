using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PMSBackend.Application.Commands.PrescriptionUpload;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Commands.PatientProfile
{
    public class EditPatientProfileDetailsCommand : IRequest<PatientWithRelativesDTO>
    {
        public long PatientId { get; set; }
        public PatientUpdateDTO? PatientDetails { get; set; }
        public long LoginUserId { get; set; }

    }

    public class EditPatientProfileDetailsCommandHandler : IRequestHandler<EditPatientProfileDetailsCommand, PatientWithRelativesDTO>
    {

        private readonly IPatientProfileRepository _repository;
        private readonly ILogger<EditForSmartRxRequestCommand> _logger;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public EditPatientProfileDetailsCommandHandler(
            IPatientProfileRepository repository,
            ILogger<EditForSmartRxRequestCommand> logger,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _repository = repository;
            _logger = logger;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PatientWithRelativesDTO> Handle(EditPatientProfileDetailsCommand request, CancellationToken cancellationtoken)
        {
            try
            {
                var responseResult = new PatientWithRelativesDTO();

                bool isExistsPatient = await _repository.IsExistsPatientProfileDetails(request.PatientId);
                if (!isExistsPatient)
                {
                    responseResult.ApiResponseResult = new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Status = "Failed",
                        Message = "Patient not found"
                    };
                    return responseResult;
                }



                PatientWithRelativesContract pt = new PatientWithRelativesContract();

                // Set PatientId from the request (required)
                pt.Id = request.PatientId;

                // Only set fields that are provided in the update DTO (all optional)
                if (request.PatientDetails != null)
                {
                    // Basic Information
                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.PatientCode))
                        pt.PatientCode = request.PatientDetails.PatientCode;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.FirstName))
                        pt.FirstName = request.PatientDetails.FirstName;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.LastName))
                        pt.LastName = request.PatientDetails.LastName;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.NickName))
                        pt.NickName = request.PatientDetails.NickName;

                    if (request.PatientDetails.Age.HasValue)
                        pt.Age = request.PatientDetails.Age.Value;

                    if (request.PatientDetails.AgeYear.HasValue)
                        pt.AgeYear = request.PatientDetails.AgeYear.Value;

                    if (request.PatientDetails.AgeMonth.HasValue)
                        pt.AgeMonth = request.PatientDetails.AgeMonth.Value;

                    if (request.PatientDetails.Gender.HasValue)
                        pt.Gender = request.PatientDetails.Gender.Value;

                    if (request.PatientDetails.DateOfBirth.HasValue)
                        pt.DateOfBirth = request.PatientDetails.DateOfBirth.Value;

                    if (request.PatientDetails.BloodGroup.HasValue)
                        pt.BloodGroup = request.PatientDetails.BloodGroup.Value;

                    // Physical Measurements
                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.Height))
                        pt.Height = request.PatientDetails.Height;

                    if (request.PatientDetails.HeightFeet.HasValue)
                        pt.HeightFeet = request.PatientDetails.HeightFeet.Value;

                    if (request.PatientDetails.HeightInches.HasValue)
                        pt.HeightInches = request.PatientDetails.HeightInches.Value;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.HeightMeasurementUnit))
                        pt.HeightMeasurementUnit = request.PatientDetails.HeightMeasurementUnit;

                    if (request.PatientDetails.HeightMeasurementUnitId.HasValue)
                        pt.HeightMeasurementUnitId = request.PatientDetails.HeightMeasurementUnitId.Value;

                    if (request.PatientDetails.Weight.HasValue)
                        pt.Weight = request.PatientDetails.Weight.Value;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.WeightMeasurementUnit))
                        pt.WeightMeasurementUnit = request.PatientDetails.WeightMeasurementUnit;

                    if (request.PatientDetails.WeightMeasurementUnitId.HasValue)
                        pt.WeightMeasurementUnitId = request.PatientDetails.WeightMeasurementUnitId.Value;

                    // Contact Information
                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.MobileNo))
                        pt.PhoneNumber = request.PatientDetails.MobileNo;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.Email))
                        pt.Email = request.PatientDetails.Email;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.Address))
                        pt.Address = request.PatientDetails.Address;

                    if (request.PatientDetails.PoliceStationId.HasValue)
                        pt.PoliceStationId = request.PatientDetails.PoliceStationId.Value;

                    if (request.PatientDetails.CityId.HasValue)
                        pt.CityId = request.PatientDetails.CityId.Value;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.PostalCode))
                        pt.PostalCode = request.PatientDetails.PostalCode;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.EmergencyContact))
                        pt.EmergencyContact = request.PatientDetails.EmergencyContact;

                    // Profile Information
                    if (request.PatientDetails.MaritalStatus.HasValue)
                        pt.MaritalStatus = request.PatientDetails.MaritalStatus.Value;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.Profession))
                        pt.Profession = request.PatientDetails.Profession;

                    if (request.PatientDetails.IsExistingPatient.HasValue)
                        pt.IsExistingPatient = request.PatientDetails.IsExistingPatient.Value;

                    if (request.PatientDetails.ExistingPatientId.HasValue)
                        pt.ExistingPatientId = request.PatientDetails.ExistingPatientId.Value;

                    if (request.PatientDetails.ProfileProgress.HasValue)
                        pt.ProfileProgress = request.PatientDetails.ProfileProgress.Value;

                    if (request.PatientDetails.IsRelative.HasValue)
                        pt.IsRelative = request.PatientDetails.IsRelative.Value;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.RelationToPatient))
                        pt.RelationToPatient = request.PatientDetails.RelationToPatient;

                    if (request.PatientDetails.RelatedToPatientId.HasValue)
                        pt.RelatedToPatientId = request.PatientDetails.RelatedToPatientId.Value;

                    if (request.PatientDetails.IsActive.HasValue)
                        pt.IsActive = request.PatientDetails.IsActive.Value;

                    // Profile Photo handling
                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.ProfilePhotoName))
                        pt.ProfilePhotoName = request.PatientDetails.ProfilePhotoName;

                    if (!string.IsNullOrWhiteSpace(request.PatientDetails.ProfilePhotoPath))
                        pt.ProfilePhotoPath = request.PatientDetails.ProfilePhotoPath;

                    // If a new file was uploaded, override with generated name/path
                    if (request.PatientDetails.ProfilePhoto != null && request.PatientDetails.ProfilePhoto.Length > 0)
                    {
                        // Get existing patient code for photo naming
                        var existingPatient = await _repository.GetPatientProfileWithRelativesById(request.PatientId, cancellationtoken);
                        string patientCode = existingPatient?.PatientCode;

                        pt.ProfilePhotoName = $"PatientProfilePhoto_{patientCode}_thumbnail.jpg";
                        pt.ProfilePhotoPath = $"photos\\PatientProfilePhoto_{patientCode}_thumbnail.jpg";
                    }
                    if (request.PatientDetails.Relatives != null) pt.Relatives = request.PatientDetails.Relatives;
                }


                var patientUpdatedInfo = await _repository.EditPatientDetailsAsync(request.PatientId, request.LoginUserId, pt, cancellationtoken);

                responseResult.Id = patientUpdatedInfo.Id ?? 0;
                responseResult.PatientCode = patientUpdatedInfo.PatientCode;
                responseResult.FirstName = patientUpdatedInfo.FirstName;
                responseResult.LastName = patientUpdatedInfo.LastName;
                responseResult.NickName = patientUpdatedInfo.NickName;
                responseResult.Age = patientUpdatedInfo.Age;
                responseResult.AgeYear = patientUpdatedInfo.AgeYear;
                responseResult.AgeMonth = patientUpdatedInfo.AgeMonth;
                responseResult.Gender = patientUpdatedInfo.Gender ?? 0;
                responseResult.DateOfBirth = patientUpdatedInfo.DateOfBirth;
                responseResult.BloodGroup = patientUpdatedInfo.BloodGroup;
                responseResult.Height = patientUpdatedInfo.Height;
                responseResult.HeightFeet = patientUpdatedInfo.HeightFeet ?? 0;
                responseResult.HeightInches = patientUpdatedInfo.HeightInches ?? 0;
                responseResult.HeightMeasurementUnit = patientUpdatedInfo.HeightMeasurementUnit;
                responseResult.HeightMeasurementUnitId = patientUpdatedInfo.HeightMeasurementUnitId;
                responseResult.Weight = patientUpdatedInfo.Weight;
                responseResult.WeightMeasurementUnit = patientUpdatedInfo.WeightMeasurementUnit;
                responseResult.WeightMeasurementUnitId = patientUpdatedInfo.WeightMeasurementUnitId;
                responseResult.MobileNo = patientUpdatedInfo.PhoneNumber;
                responseResult.Email = patientUpdatedInfo.Email;
                responseResult.Profession = patientUpdatedInfo.Profession;
                responseResult.ProfilePhotoName = patientUpdatedInfo.ProfilePhotoName;
                responseResult.ProfilePhotoPath = patientUpdatedInfo.ProfilePhotoPath;
                responseResult.Address = patientUpdatedInfo.Address;
                responseResult.PoliceStationId = patientUpdatedInfo.PoliceStationId;

                responseResult.CityId = patientUpdatedInfo.CityId;
                responseResult.PostalCode = patientUpdatedInfo.PostalCode;
                responseResult.EmergencyContact = patientUpdatedInfo.EmergencyContact;
                responseResult.MaritalStatus = patientUpdatedInfo.MaritalStatus;
                responseResult.Profession = patientUpdatedInfo.Profession;
                responseResult.IsExistingPatient = patientUpdatedInfo.IsExistingPatient;
                responseResult.ExistingPatientId = patientUpdatedInfo.ExistingPatientId;
                responseResult.ProfileProgress = patientUpdatedInfo.ProfileProgress ?? 0;

                if (patientUpdatedInfo.Relatives is not null)
                {
                    responseResult.RelativesDropdown = new List<PatientDropdown>();

                    foreach (var relative in patientUpdatedInfo.Relatives)
                    {
                        var rel = new PatientDropdown()
                        {
                            PatientId = relative.Id,
                            PatientName = relative.FirstName + " " + relative.LastName + " " + relative.NickName
                        };
                        responseResult.RelativesDropdown.Add(rel);
                    }
                    responseResult.Relatives = patientUpdatedInfo.Relatives!.Select(p => new RelativeDTO()
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
                        ProfileProgress = p.ProfileProgress ?? 0,
                        IsActive = p.IsActive
                    }).ToList();
                }

                // Save reward transaction for editing patient profile
                var rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                    request.LoginUserId,
                    0,                          // no prescription
                    null,                       // no SmartRx master
                    patientUpdatedInfo.Id,      // updated patient as context
                    "EDIT_PATIENT_PROFILE",
                    "editing a patient profile",
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
                throw;
            }
        }
    }
}


