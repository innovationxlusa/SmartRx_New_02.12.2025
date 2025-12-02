using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class PrescriptionDTO
    {
        public long FileId { get; set; } // FileRx, SmartRx, Waiting List
        public string PrescriptionCode { get; set; }
        public string PrescriptionDate { get; set; }
        public long? SmartRxMasterId { get; set; }

        public bool IsExistingPatient { get; set; }
        public long PatientId { get; set; }
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public long? ParentFolderId { get; set; }
        public long FolderHeirarchy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public List<string> FilePathList { get; set; }
        public string? FileExtension { get; set; }
        public int? filStoredForThisPrescriptionCount { get; set; }
        public bool IsSmartRxRequested { get; set; }
        public bool? HasRelative { get; set; }
        public string? PatientRelativesId { get; set; }

        // both false ==only file uploaded
        // IsSmarted false & IsWaiting true== waiting
        // IsSmarted true == smarted
        public bool IsSmarted { get; set; }
        public bool IsWaiting { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public bool IsFile { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Tag1 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Tag2 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Tag3 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Tag4 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Tag5 { get; set; }

    }
}
