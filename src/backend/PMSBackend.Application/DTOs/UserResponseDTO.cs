namespace PMSBackend.Application.DTOs
{
    public class UserResponseDTO
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
