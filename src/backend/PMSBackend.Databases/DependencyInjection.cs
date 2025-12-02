using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PMSBackend.Application.CommonServices.Interfaces;
using PMSBackend.Databases.Data;
using PMSBackend.Databases.Repositories;
using PMSBackend.Databases.Services;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;



namespace PMSBackend.Databases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuth()
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
            services.AddScoped<IPrescriptionUploadRepository, PrescriptionUploadRepository>();
            services.AddScoped<IUserWiseFolderRepository, UserWiseFolderRepository>();
            services.AddScoped<IBrowseRxRepository, BrowseRxRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ISmartRxInsiderRepository, SmartRxInsiderRepository>();
            services.AddScoped<ISmartRxVitalRepository, SmartRxVitalRepository>();
            services.AddScoped<IVitalRepository, VitalRepository>();
            services.AddScoped<IMedicineCompareRepository, MedicineCompareRepository>();
            services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
            services.AddScoped<IDoctorProfileRepository, DoctorProfileRepository>();
            services.AddScoped<ISmartRxOtherExpenseRepository, SmartRxOtherExpenseRepository>();
            services.AddScoped<IPatientRewardRepository, PatientRewardRepository>();
            services.AddScoped<IRewardRepository, RewardRepository>();
            services.AddScoped<IRewardBadgeRepository, RewardBadgeRepository>();
            services.AddScoped<IUserRewardBadgeRepository, UserRewardBadgeRepository>();
            services.AddScoped<IRewardTransactionRepository, RewardTransactionRepository>();
            services.AddScoped<IRewardPointConversionsRepository, RewardPointConversionsRepository>();
            services.AddScoped<IRewardRuleRepository, RewardRuleRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<ICodeGenerationService, CodeGenerationService>();

            return services;
        }

        public static IServiceCollection AddContext(
            this IServiceCollection services, IConfiguration configuration)
        {
            //    IConfigurationRoot configurationr = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json")
            //.Build();

            //    var connectionString = configuration.GetConnectionString("PMSDBConnection");

            //    var optionsBuilder = new DbContextOptionsBuilder<PMSDbContext>();
            //    optionsBuilder.UseSqlServer(connectionString);


            services.AddSingleton<DbConnector>();
            var dbConnector = new DbConnector(configuration);
            var connectionString = dbConnector.GetConnectionString("PMSDBConnection");

            // Register DbContext with the retrieved connection string
            services.AddDbContext<PMSDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(PMSDbContext).Assembly.FullName)
                .EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                );

            return services;
        }

        public static IServiceCollection AddAuth(
        this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            // Configuration for token
            services.AddAuthentication(x =>
            {   
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = TokenHelper.BuildValidationParameters(validateLifetime: true);
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Extract token from Authorization header
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        
                        if (string.IsNullOrWhiteSpace(authHeader))
                        {
                            return Task.CompletedTask;
                        }

                        // Remove "Bearer " prefix if present (case-insensitive)
                        var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                            ? authHeader.Substring(7).Trim()
                            : authHeader.Trim();

                        // Trim whitespace and remove quotes that might have been added
                        token = token.Trim().Trim('"', '\'');

                        // Validate token format (JWT should have 3 parts separated by dots: header.payload.signature)
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            context.Fail("Token is empty after cleaning");
                            return Task.CompletedTask;
                        }

                        var parts = token.Split('.');
                        if (parts.Length != 3)
                        {
                            context.Fail($"Invalid JWT format. Expected 3 parts separated by dots, got {parts.Length} parts. Token length: {token.Length}");
                            return Task.CompletedTask;
                        }

                        // Check if all parts are non-empty
                        foreach (var part in parts)
                        {
                            if (string.IsNullOrWhiteSpace(part))
                            {
                                context.Fail("Invalid JWT format: One or more token parts are empty");
                                return Task.CompletedTask;
                            }
                        }

                        // Set the cleaned token - this will be used by the middleware for validation
                        context.Token = token;
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<DependencyInjection>>();
                        
                        // Extract token from request headers for logging
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        string tokenPreview = "No token provided";
                        
                        if (!string.IsNullOrWhiteSpace(authHeader))
                        {
                            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                                ? authHeader.Substring(7).Trim()
                                : authHeader.Trim();
                            
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                tokenPreview = token.Length > 20 
                                    ? $"{token.Substring(0, 20)}..." 
                                    : token;
                            }
                        }
                        
                        //logger.LogWarning("JWT Authentication failed. Error: {Error}, Token preview: {TokenPreview}", 
                        //    context.Exception?.Message ?? "Unknown error", tokenPreview);
                        
                        context.NoResult();
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken jwt &&
                            !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.Fail("Invalid token algorithm. Expected HS256.");
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        
                        var errorMessage = context.Error switch
                        {
                            "invalid_token" => "Invalid token format",
                            "invalid_signature" => "Invalid token signature",
                            _ => context.Error ?? "Authentication failed"
                        };

                        return context.Response.WriteAsync($"{{\"error\":\"{errorMessage}\"}}");
                    }
                };
            });
            return services;
        }
    }
}