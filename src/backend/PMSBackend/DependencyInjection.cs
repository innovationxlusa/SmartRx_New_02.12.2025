using Mapster;
using PMSBackend.API.Common;

namespace PMSBackend.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
           
            services.AddControllers();
            services.AddMappings();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           // services.AddTransient<JwtService>();
            return services;
        }
    }
}
