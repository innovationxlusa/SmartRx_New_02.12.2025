using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMSBackend.Domain.Repositories;
using PMSBackend.Infrastructure.Repositories;
using PMSBackend.Infrastructure.Services;
using PMSBackend.Infrastucture.Data;
using PMSBackend.Application.CommonServices.Exceptions;
using PMSBackend.Application.CommonServices.Interfaces;


namespace PMSBackend.Infrastructure
{
    public static class DependencyInjection
    {
        //    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        //        IConfiguration configuration)
        //    {

        //        //services.AddDbContext<PMSDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("PMSDBConnection"),
        //        //    b => b.MigrationsAssembly(typeof(PMSDbContext).Assembly.FullName)
        //        //));

        //        services.AddDbContext<PMSDbContext>(options =>
        //options.UseSqlServer(configuration.GetConnectionString("PMSDBConnection"), b => b.MigrationsAssembly(typeof(PMSDbContext).Assembly.FullName)), ServiceLifetime.Singleton);

        //        services.AddSingleton<IDBContext>(provider => provider.GetRequiredService<PMSDbContext>());

        //        services.AddIdentity<PMSUser, IdentityRole>()
        //        .AddEntityFrameworkStores<PMSDbContext>()
        //        .AddDefaultTokenProviders();

        //        services.Configure<IdentityOptions>(options =>
        //        {
        //            // Default Lockout settings.
        //            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //            options.Lockout.MaxFailedAccessAttempts = 5;
        //            options.Lockout.AllowedForNewUsers = true;
        //            // Default Password settings.
        //            options.Password.RequireDigit = false;
        //            options.Password.RequireLowercase = true;
        //            options.Password.RequireNonAlphanumeric = false; // For special character
        //            options.Password.RequireUppercase = false;
        //            options.Password.RequiredLength = 6;
        //            options.Password.RequiredUniqueChars = 1;
        //            // Default SignIn settings.
        //            options.SignIn.RequireConfirmedEmail = false;
        //            options.SignIn.RequireConfirmedPhoneNumber = false;
        //            options.User.RequireUniqueEmail = true;
        //        });

        //        services.AddScoped<IIdentityService, IdentityService>();           
        //        services.AddSingleton<IDBContext>(provider => provider.GetRequiredService<PMSDbContext>());

        //        services.AddScoped<IRequestHandler<CreateUserCommand, int>, CreateUserCommandHandler>();
        //        services.AddScoped<IRequestHandler<GetUserQuery, List<UserResponseDTO>>, GetUserQueryHandler>();
        //        //services.AddScoped<IIdentityService, IdentityService>();
        //        services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        //        services.AddSingleton(typeof(IPrescriptionUploadRepository), typeof(PrescriptionUploadRepository));

        //        services.AddScoped(typeof(IRequestHandler<GetAllUsersDetailsQuery, List<UserDetailsResponseDTO>>), typeof(GetAllUsersDetailsQueryHandler));

        //        services.AddTransient(typeof(IRequestHandler<InsertPrescriptionUploadCommand, PrescriptionUploadEntity>), typeof(InsertPrescriptionUploadCommandHandler));



        //        // services.AddScoped<IPrescriptionService, PrescriptionService>();
        //        //services.AddTransient<IBaseRepository<PrescriptionUploadEntity>, PrescriptionUploadRepository>();
        //        //services.AddTransient(typeof(PrescriptionSequenceEntity), typeof(PrescriptionUploadRepository));


        //        //services.AddTransient<IRequestHandler<CreateUserCommand, int>, CreateUserCommandHandler>();

        //        //services.AddTransient<IRequestHandler<GetAllUsersQuery, List<User>>, GetAllUsersQueryHandler>();

        //        // services.AddDbContext<PMSDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));



        //        return services;
        //    }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {

            services
                .AddAuth(configuration)
                .AddContext(configuration)
                .AddPersistence();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }


        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();


            //services.AddScoped(typeof(IPrescriptionUploadRepository), typeof(PrescriptionUploadRepository));
            // services.AddScoped(typeof(IPrescriptionUploadRepository), typeof(PrescriptionUploadRepository));
            // services.AddScoped(typeof(IRequestHandler<GetAllUsersDetailsQuery, List<UserDetailsResponseDTO>>), typeof(GetAllUsersDetailsQueryHandler));
            //services.AddTransient(typeof(IRequestHandler<InsertPrescriptionUploadCommand, PrescriptionUploadEntity>), typeof(InsertPrescriptionUploadCommandHandler));

            //services.AddScoped<IAdminstratorRepository, AdminstratorRepository>();
            //services.AddScoped<IAccountsRepository, AccountsRepository>();
            //services.AddScoped<ICustomerRepository, CustomersRepository>();
            //services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            //services.AddScoped<IDispositionRepository, DispositionRepository>();

            return services;
        }

        public static IServiceCollection AddContext(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DbConnector>();
            // Get connection string via DbConnector
            var dbConnector = new DbConnector(configuration);
            var connectionString = dbConnector.GetConnectionString("PMSDBConnection");

            // Register DbContext with the retrieved connection string
            services.AddDbContext<PMSDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(PMSDbContext).Assembly.FullName)
            ));
            //services.AddTransient<DbInitializer>();

            //services.AddDbContext<PMSDbContext>(options => options.UseSqlServer(connectionString,
            //    b => b.MigrationsAssembly(typeof(PMSDbContext).Assembly.FullName)
            //));
            //var dbSettings = new PMSDBSettings();
            // var config= configuration.GetSection(key: PMSDBSettings.SectionName);
            //var config=configuration.GetSection(PMSDBSettings.SectionName);

            // configuration.GetConnectionString(dbSettings.SqlServer);
            //services.AddSingleton(Options.Create(dbSettings));



            return services;
        }

        public static IServiceCollection AddAuth(
        this IServiceCollection services, ConfigurationManager configuration)
        {
            //var JwtSettings = new JwtSettings();
            //configuration.Bind(JwtSettings.SectionName, JwtSettings);

            //services.AddSingleton(Options.Create(JwtSettings));
            //services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            //services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = JwtSettings.Issuer,
            //        ValidAudience = JwtSettings.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(JwtSettings.Secret))
            //    });
            return services;
        }
    }
}