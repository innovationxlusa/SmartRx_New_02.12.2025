using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxHistoryDTO
    {
        public long SmartRxMasterId { get; set; }

        public long PrescriptionId { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Details { get; set; }
    }
}
