using PMSBackend.Application.CommonServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSBackend.Application.DTOs
{
    public class UserActivityDTO
    {
        public long Id { get; set; }
        public string ActivityCode { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public double ActivityPoint { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public long? CreatedById { get; set; }
        [NotMapped]
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public long? ModifiedById { get; set; }
    }

    public class UserActivityCreateDTO
    {
        public string ActivityName { get; set; } = string.Empty;
        public double ActivityPoint { get; set; }
        public string? Remarks { get; set; }
        public long UserId { get; set; }
    }

    public class UserActivitiesDTO
    {
        public ApiResponseResult? ApiResponseResult { get; set; }
    }
}

