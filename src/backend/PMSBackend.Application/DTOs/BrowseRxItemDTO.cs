namespace PMSBackend.Application.DTOs
{
    public class BrowseRxItemDTO
    {
        public bool IsFolder { get; set; }
        public long? FolderId { get; set; }
        public long? FileId { get; set; }
        public string Name { get; set; }
        public string? PrescriptionCode { get; set; }
        public string? FileExtension { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedDateStr { get; set; }
        public long? ParentFolderId { get; set; }
        public long UserId { get; set; }
    }
}


