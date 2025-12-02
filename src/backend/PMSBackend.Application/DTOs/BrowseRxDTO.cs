namespace PMSBackend.Application.DTOs
{
    public class BrowseRxDTO
    {
        public long FolderId { get; set; }
        public long UserId { get; set; }
        public long FolderHeirarchy { get; set; }
        public string FolderName { get; set; }
        public long? ParentFolderId { get; set; }
        public string CreatedDateStr { get; set; }
        public List<FolderNodeDTO> Children { get; set; }
        public List<UserWiseFolderDTO> FolderListChildren { get; set; } = new();
        public List<PrescriptionDTO> PrescriptionList { get; set; } = new();
    }
}
