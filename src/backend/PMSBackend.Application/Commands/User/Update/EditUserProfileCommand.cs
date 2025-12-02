using MediatR;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.User.Update
{
    public class EditUserProfileCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; } = string.Empty!;
        public string? Email { get; set; } = string.Empty!;
        public string? GoogleId { get; set; } = string.Empty!;
        public string? FacebookId { get; set; } = string.Empty!;
        public string? TwitterId { get; set; } = string.Empty!;
        public string? FirstName { get; set; } = string.Empty!;
        public string? LastName { get; set; } = string.Empty!;
        public int? AuthMethod { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        // public IList<UserRole> Roles { get; set; } = default!;

    }

    public class EditUserProfileCommandHandler : IRequestHandler<EditUserProfileCommand, long>
    {
        private readonly IUserRepository _userRepository;

        public EditUserProfileCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<long> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _userRepository.GetDetailsByIdAsync(request.Id);
                if (entity is not null)
                {
                    if (!string.IsNullOrWhiteSpace(request.MobileNo)) entity.MobileNo = request.MobileNo!;
                    if (!string.IsNullOrWhiteSpace(request.Email)) entity.Email = request.Email!;
                    if (!string.IsNullOrWhiteSpace(request.GoogleId)) entity.GoogleId = request.GoogleId!;
                    if (!string.IsNullOrWhiteSpace(request.FacebookId)) entity.FacebookId = request.FacebookId!;
                    if (!string.IsNullOrWhiteSpace(request.TwitterId)) entity.TwitterId = request.TwitterId!;
                    if (!string.IsNullOrWhiteSpace(request.FirstName)) entity.FirstName = request.FirstName!;
                    if (!string.IsNullOrWhiteSpace(request.LastName)) entity.LastName = request.LastName!;
                    if (request.AuthMethod is not null && request.AuthMethod > 0) entity.AuthMethod = request.AuthMethod.Value;
                    if (request.EmployeeId is not null && request.EmployeeId > 0) entity.EmployeeId = request.EmployeeId.Value;
                    if (!string.IsNullOrWhiteSpace(request.EmployeeCode)) entity.EmployeeCode = request.EmployeeCode!;
                    if (request.Gender is not null && request.Gender > 0) entity.Gender = request.Gender.Value;
                    if (request.DateOfBirth is not null) entity.DateOfBirth = request.DateOfBirth.Value;
                    if (request.Status is not null && request.Status > 0) entity.Status = request.Status.Value;

                    await _userRepository.UpdateAsync(entity);
                }
                return request.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
