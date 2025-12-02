using PMSBackend.Application.CommonServices.Interfaces;

namespace PMSBackend.Databases.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}