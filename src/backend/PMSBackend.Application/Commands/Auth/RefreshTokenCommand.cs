using MediatR;
using PMSBackend.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace PMSBackend.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<AuthTokenResponseDTO?>
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

