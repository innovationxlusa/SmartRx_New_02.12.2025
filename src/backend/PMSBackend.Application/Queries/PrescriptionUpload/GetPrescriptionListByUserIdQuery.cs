using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.PrescriptionUpload
{
    public class GetPrescriptionListByUserIdQuery : IRequest<PrescriptionDTO>
    {
        public long UserId { get; set; }
    }

    public class GetPrescriptionListByUserIdQueryHandler : IRequestHandler<GetPrescriptionListByUserIdQuery, PrescriptionDTO>
    {
        private readonly IUserRepository _userRepository;
        public GetPrescriptionListByUserIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PrescriptionDTO> Handle(GetPrescriptionListByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // var role = await _roleRepository.GetDetailsByIdAsync(request.RoleId);
                await Task.CompletedTask;
                return new PrescriptionDTO();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}