using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PMSBackend.Databases.Data
{
    public class DbConnector
    {
        private readonly IConfiguration _configuration;
        private string? v;

        public DbConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //    IConfigurationRoot configurationr = new ConfigurationBuilder()
        //.SetBasePath(Directory.GetCurrentDirectory())
        //.AddJsonFile("appsettings.json")
        //.Build();

        //    var connectionString = configuration.GetConnectionString("PMSDBConnection");

        //    var optionsBuilder = new DbContextOptionsBuilder<PMSDbContext>();
        //    optionsBuilder.UseSqlServer(connectionString);

        //    builder.Configuration
        //.SetBasePath(Directory.GetCurrentDirectory())
        //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //.AddEnvironmentVariables(); // Also load from environment variables if set
        public string GetConnectionString(string connectionName)
        {
            //    IConfigurationRoot configurationr = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json")
            //.Build();

            //    var connectionString = configuration.GetConnectionString("PMSDBConnection");

            //    var optionsBuilder = new DbContextOptionsBuilder<PMSDbContext>();
            //    optionsBuilder.UseSqlServer(connectionString);

            return _configuration.GetConnectionString(connectionName);
        }
        public IDbConnection CreateConnection(string connectionName)
        {
            string connectionString = _configuration.GetConnectionString(connectionName);
            return new SqlConnection(connectionString);
        }
    }
}
