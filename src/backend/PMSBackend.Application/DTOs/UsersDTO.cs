using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class UsersDTO
    {
        public string? ConnectionString { get; set; }
        public List<UserDetailsResponseDTO> Users { get; set; }
        public ApiResponseResult ApiResponseResult { get; set; }
    }
}
