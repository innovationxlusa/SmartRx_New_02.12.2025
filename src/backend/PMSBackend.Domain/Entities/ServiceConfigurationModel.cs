namespace PMSBackend.Domain.Entities
{
    public class ServiceConfigurationModel : BaseEntity
    {
        public string RedisConnectionString { get; set; } = default!;

        public string RedisDatabase { get; set; } = default!;

        public string APIDomain { get; set; } = default!;

        public string AppName { get; set; } = default!;

        public string LogApi { get; set; } = default!;


    }
}
