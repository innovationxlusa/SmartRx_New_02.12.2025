using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PMSBackend.Application
{
    public static class DependencyInjection
    {
        //public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
        //    services.AddMediatR(ctg =>
        //    {
        //        ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

        //    });

        //    return services;
        //}
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(ctg =>
            {
                ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            });
            //services.AddMediatR(typeof(DependencyInjection).Assembly);
            //services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
