using MediatR;
using Microsoft.AspNetCore.Http;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;

namespace PMSBackend.Application.Queries.DoctorProfile
{
    public class GetDoctorProfilesByUserIdQuery : IRequest<PaginatedResult<DoctorProfileListItemDTO>>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }

        public long? DoctorId { get; set; }

        public string? SearchKeyword { get; set; }
        public string? SearchColumn { get; set; }

        public PagingSortingParams? PagingSorting { get; set; } = new PagingSortingParams(); // Default initialization for Swagger

    }

    public class GetDoctorProfilesByUserIdQueryHandler : IRequestHandler<GetDoctorProfilesByUserIdQuery, PaginatedResult<DoctorProfileListItemDTO>>
    {
        private readonly IDoctorProfileRepository _doctorProfileRepository;
        private readonly IRewardTransactionRepository _rewardTransactionRepository;

        public GetDoctorProfilesByUserIdQueryHandler(
            IDoctorProfileRepository doctorProfileRepository,
            IRewardTransactionRepository rewardTransactionRepository)
        {
            _doctorProfileRepository = doctorProfileRepository;
            _rewardTransactionRepository = rewardTransactionRepository;
        }

        public async Task<PaginatedResult<DoctorProfileListItemDTO>> Handle(GetDoctorProfilesByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Ensure PagingSorting is initialized
                if (request.PagingSorting == null)
                {
                    request.PagingSorting = new PagingSortingParams();
                }

                var result = await _doctorProfileRepository.GetDoctorProfilesByUserIdWithPagingAsync(
                    request.UserId,
                    request.PatientId,
                    request.DoctorId,
                    request.SearchKeyword,
                    request.SearchColumn,
                    request.PagingSorting,
                    cancellationToken);

                var mapped = result.Data.Select(p => new DoctorProfileListItemDTO
                {
                    DoctorId = p.DoctorId,
                    DoctorCode = p.DoctorCode,
                    DoctorTitle = p.DoctorTitle,
                    DoctorFirstName = p.DoctorFirstName,
                    DoctorLastName = p.DoctorLastName,
                    ProfilePhotoName = p.ProfilePhotoName,
                    ProfilePhotoPath = p.ProfilePhotoPath,
                    RegistrationNumber = p.RegistrationNumber,
                    DoctorRating = p.DoctorRating,
                    SmartRxCount = p.SmartRxCount ?? 0
                }).ToList();

                //var rewardResult = new RewardTransactionResult();
                //// Save reward for browsing doctors (similar pattern to CreateFolderCommand)
                //if (mapped.Count > 0)
                //{
                //    var referenceDoctorId = mapped.First().DoctorId;
                //    rewardResult = await _rewardTransactionRepository.CreateRewardTransactionForPrescriptionAsync(
                //        request.UserId,
                //        null,
                //        null,
                //        request.PatientId,
                //        "BROWSE_DOCTOR",
                //        "browsing doctor profiles",
                //        cancellationToken);
                //}

                return new PaginatedResult<DoctorProfileListItemDTO>(
                    mapped,
                    result.TotalRecords,
                    result.PageNumber,
                    result.PageSize,
                    result.SortBy,
                    result.SortDirection,
                    result.message,
                   null,
                   null,
                   null,
                   null
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


