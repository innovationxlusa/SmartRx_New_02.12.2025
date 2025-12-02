namespace PMSBackend.Application.DTOs
{
    public class RoleResponseDTO
    {
        public long Id { get; set; }
        public string RoleName { get; set; } = default!;
        public string RoleDescription { get; set; } = default!;
    }
}
