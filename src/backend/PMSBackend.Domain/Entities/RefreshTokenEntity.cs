using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Security_RefreshToken")]
    public class RefreshTokenEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Token { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string JwtId { get; set; } = string.Empty;

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity? User { get; set; }
    }
}

