using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.Commands.Auth
{
    public class RevokeAllUserTokensCommand : IRequest<bool>
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, long.MaxValue, ErrorMessage = "User ID must be a positive number")]
        public long UserId { get; set; }
    }
}

