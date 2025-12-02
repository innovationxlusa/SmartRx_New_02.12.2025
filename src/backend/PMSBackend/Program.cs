using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PMSBackend.API;
using PMSBackend.API.Common;
using PMSBackend.Application;
using PMSBackend.Databases;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Cryptography.X509Certificates;


var builder = WebApplication.CreateBuilder(args);

//var certPath = @"C:\\certs\\lan-test-dev.pfx"; //Path.Combine(builder.Environment.ContentRootPath, "certs", "lan-dev.pfx");
var certPath = @"C:\\certs\\smartrx-wan.pfx"; //Path.Combine(builder.Environment.ContentRootPath, "certs", "lan-dev.pfx");

var certPassword = "Abc@1234";
var certificate = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);

// Log environment information
Console.WriteLine($"========================================");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Loading appsettings.json and appsettings.{builder.Environment.EnvironmentName}.json");
Console.WriteLine($"========================================");

Console.WriteLine($"Connection string: {builder.Configuration.GetConnectionString("PMSDBConnection")}");

builder.WebHost.ConfigureKestrel(options =>
{
    // HTTP listener
    options.ListenAnyIP(7000); // Plain HTTP    
    // HTTPS listener
    options.ListenAnyIP(8443, listenOptions =>
    {
        listenOptions.UseHttps(certificate);
    });
});

// Configuration loading hierarchy:
// 1. appsettings.json (base configuration)
// 2. appsettings.{Environment}.json (environment-specific, overrides base)
// 3. Environment variables (highest priority)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Also load from environment variables if set

builder.Services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
});
// Add this to dependency injection
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartRx PMS API",
        Version = "v1",
        Description = "Comprehensive API for SmartRx Prescription Management System. This API provides endpoints for managing patients, prescriptions, rewards, user activities, and more.",
        Contact = new OpenApiContact
        {
            Name = "SmartRx Development Team",
            Email = "support@smartrx.com"
        },
        License = new OpenApiLicense
        {
            Name = "Proprietary",
            Url = new Uri("https://smartrx.com/license")
        },
        TermsOfService = new Uri("https://smartrx.com/terms")
    });

    // Add JWT Bearer Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'. Get your token from the /api/Auth/Login endpoint."
    });

    // Require Bearer token for all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments for better documentation
    try
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        }

        // Include XML comments from Application layer
        var applicationXmlFile = "PMSBackend.Application.xml";
        var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
        if (File.Exists(applicationXmlPath))
        {
            options.IncludeXmlComments(applicationXmlPath);
        }

        // Include XML comments from Domain layer
        var domainXmlFile = "PMSBackend.Domain.xml";
        var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
        if (File.Exists(domainXmlPath))
        {
            options.IncludeXmlComments(domainXmlPath);
        }
    }
    catch (Exception ex)
    {
        // Log but don't fail if XML files are missing
        Console.WriteLine($"Warning: Could not load XML documentation files: {ex.Message}");
    }

    // Use schema IDs that include generic type parameters to avoid conflicts
    options.CustomSchemaIds(type =>
    {
        if (type == null) return "Unknown";
        
        if (type.IsGenericType)
        {
            var name = type.Name;
            var index = name.IndexOf('`');
            var baseName = index > 0 ? name.Substring(0, index) : name;
            
            // Get generic type arguments and create a unique schema ID
            var genericArgs = type.GetGenericArguments();
            var argsString = string.Join("", genericArgs.Select(arg => 
            {
                // For nested generics, recursively process
                if (arg.IsGenericType)
                {
                    var argName = arg.Name;
                    var argIndex = argName.IndexOf('`');
                    var argBaseName = argIndex > 0 ? argName.Substring(0, argIndex) : argName;
                    var argGenericArgs = arg.GetGenericArguments();
                    var argArgsString = string.Join("", argGenericArgs.Select(a => a.Name.Replace("`", "").Replace(".", "")));
                    return argBaseName + argArgsString;
                }
                // Remove namespace and special characters for clean schema ID
                return arg.Name.Replace("`", "").Replace(".", "");
            }));
            
            return baseName + argsString;
        }
        return type.Name;
    });

    // Ignore properties that might cause issues
    options.IgnoreObsoleteProperties();

    // Add operation filter for security requirements
    // Temporarily disabled to debug 500 error - uncomment once working
    // try
    // {
    //     options.OperationFilter<SecurityRequirementsOperationFilter>();
    // }
    // catch (Exception ex)
    // {
    //     Console.WriteLine($"Warning: Could not add SecurityRequirementsOperationFilter: {ex.Message}");
    // }

    // Sort endpoints by tag
    try
    {
        options.TagActionsBy(api =>
        {
            try
            {
                var controller = api.ActionDescriptor?.RouteValues?["controller"] ?? "Default";
                return new[] { api.GroupName ?? controller };
            }
            catch
            {
                return new[] { "Default" };
            }
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not configure TagActionsBy: {ex.Message}");
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            //.AllowAnyOrigin()
            .WithOrigins("http://localhost:3000",
                        "https://localhost:3001",
                        "https://192.168.40.112:3000",
                        "https://localhost:3000",
                        "https://192.168.40.112:3000",
                        "https://192.168.40.112:3001",
                        "https://192.168.40.112:4000",
                        "http://192.168.40.112:3000",
                        "http://192.168.40.112:3001",
                        "http://192.168.40.112:4000",
                        "https://192.168.40.112:5001",
                        "https://182.160.111.50:5001"
                        ) // 👈 specify allowed origins
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders(
                "X-Total-Count",
                "X-Reward-Updated",
                "X-Reward-Title",
                "X-Reward-Points",
                "X-Reward-Message"); // expose reward headers for frontend
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorString = string.Join(" | ",
                context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value.Errors.Select(err => $"{kvp.Key.Replace("$.", "")}: {err.ErrorMessage}"))
            );

        var response = new
        {
            StatusCode = 400,
            Status = "Failed",
            Message = errorString
        };
        return new BadRequestObjectResult(response);
    };
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // 👈 log to console
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Optional: set log level

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

await DataSeeder.SeedAsync(app.Services);
await DataSeeder.SeedRoleDataAsync(app.Services);
await DataSeeder.SeedDataUserRoleAsync(app.Services);

app.UseCors("CorsPolicy");
app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Hello, this is a console log!");
    return "Hello World!";
});
var jwtSettings = app.Services.GetRequiredService<JwtSettings>();
JwtConfig.Initialize(jwtSettings);

// Log JWT configuration being used
Console.WriteLine($"========================================");
Console.WriteLine($"JWT Configuration Loaded:");
Console.WriteLine($"  Issuer: {jwtSettings.Issuer}");
Console.WriteLine($"  Audience: {jwtSettings.Audience}");
Console.WriteLine($"  Access Token Expiry: {jwtSettings.ExpiryMinutes} minutes");
Console.WriteLine($"  Refresh Token Expiry: {jwtSettings.RefreshTokenExpiryDays} days");
Console.WriteLine($"========================================");


app.Use(async (context, next) =>
{
    if (context.Request.Path.HasValue)
    {
        var originalPath = context.Request.Path.Value!;
        var normalizedPath = originalPath;

        while (normalizedPath.Contains("//", StringComparison.Ordinal))
        {
            normalizedPath = normalizedPath.Replace("//", "/");
        }

        if (!string.Equals(normalizedPath, originalPath, StringComparison.Ordinal))
        {
            context.Request.Path = new PathString(normalizedPath);
        }
    }

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/files"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Photos")),
    RequestPath = "/photos"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Logos")),
    RequestPath = "/logos"
});
app.UseDefaultFiles(new DefaultFilesOptions
{
    RequestPath = "/smartrx"
});
app.MapFallbackToFile("/smartrx/{*path:nonfile}", "smartrx/index.html");

// Configure Swagger/OpenAPI UI
// Enable Swagger in Development and Staging environments
// Note: In Production, you may want to restrict this
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
        c.SerializeAsV2 = false; // Use OpenAPI 3.0
    });
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartRx PMS API v1");
        options.RoutePrefix = "swagger"; // Swagger UI will be available at /swagger
        options.DocumentTitle = "SmartRx PMS API Documentation";
        options.DefaultModelsExpandDepth(-1); // Hide schemas by default
        options.DisplayRequestDuration(); // Show request duration
        options.EnableDeepLinking(); // Enable deep linking for tags and operations
        options.EnableFilter(); // Enable filter box
        options.ShowExtensions(); // Show extensions (auth, etc.)
        options.EnableValidator(); // Enable validator badge
        options.SupportedSubmitMethods(new[] { 
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get, 
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post, 
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put, 
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete, 
            Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Patch 
        });
    });
    
    Console.WriteLine("Swagger UI enabled and available at /swagger");
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
