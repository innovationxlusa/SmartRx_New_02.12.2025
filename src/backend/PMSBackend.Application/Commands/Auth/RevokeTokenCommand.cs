using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.Commands.Auth
{
    public class RevokeTokenCommand : IRequest<bool>
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

