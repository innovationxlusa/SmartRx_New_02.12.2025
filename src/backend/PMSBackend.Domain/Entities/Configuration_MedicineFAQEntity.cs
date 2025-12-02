using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Domain.Entities
{
    [Table("Configuration_MedicineFAQ")]
    public class Configuration_MedicineFAQEntity : BaseEntity
    {
        public long MedicineId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
