using PMSBackend.Application.CommonServices.Interfaces;

namespace PMSBackend.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
