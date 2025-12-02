using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{

    [Table("Prescription_UserWiseFolder")]
    public class Prescription_UserWiseFolderEntity : BaseEntity
    {
        public long? ParentFolderId { get; set; } // for only parent folder

        [ForeignKey("ParentFolderId")]
        public virtual Prescription_UserWiseFolderEntity? ParentFolder { get; set; }
        [Column(TypeName = "varchar(50)")]
        public long FolderHierarchy { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FolderName { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual SmartRxUserEntity User { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual SmartRx_PatientProfileEntity? PatientProfile { get; set; }


        //public virtual IList<Prescription_UploadEntity>? PrescriptionUploads { get; set; }
        //public virtual IList<Prescription_UserWiseFolderEntity>? SubFolders { get; set; } = new List<Prescription_UserWiseFolderEntity>();
    }
}
