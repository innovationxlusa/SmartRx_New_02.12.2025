namespace PMSBackend.Domain.Entities
{
    public class FolderNode
    {
        public long FolderId { get; set; }
        public long UserId { get; set; }
        public long FolderHeirarchy { get; set; }
        public string FolderName { get; set; }
        public long? ParentFolderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
    }
}
