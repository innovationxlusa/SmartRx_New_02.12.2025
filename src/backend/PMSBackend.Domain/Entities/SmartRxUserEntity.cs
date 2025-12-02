using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{

    [Table("Security_PMSUser")]
    public class SmartRxUserEntity : BaseEntity
    {
        [Column(TypeName = "nvarchar(10)")]
        public string UserCode { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string UserName { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? Password { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string MobileNo { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string? GoogleId { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string? FacebookId { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string? TwitterId { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string? Email { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string FirstName { get; set; } = string.Empty!;
        [Column(TypeName = "nvarchar(200)")]
        public string LastName { get; set; } = string.Empty!;
        public int AuthMethod { get; set; }
        public int? EmployeeId { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string? EmployeeCode { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual SmartRxUserEntity? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
        [ForeignKey("ModifiedById")]
        public virtual SmartRxUserEntity? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;


        public virtual IList<SmartRxUserEntity>? Users { get; set; }
        public virtual IList<SmartRxUserRoleEntity>? UserRoles { get; set; }
        //public virtual IList<Prescription_UserWiseFolderEntity>? UserWiseFolders { get; set; }
        //public virtual IList<Prescription_UploadEntity>? PrescriptionUploads { get; set; }
    }
}
