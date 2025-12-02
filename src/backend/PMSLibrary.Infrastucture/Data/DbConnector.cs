using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PMSBackend.Infrastucture.Data
{
    public class DbConnector
    {
        private readonly IConfiguration _configuration;
        private string? v;

        public DbConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       

        public string GetConnectionString(string connectionName)
        {
            return _configuration.GetConnectionString(connectionName);
        }
        public IDbConnection CreateConnection(string connectionName)
        {
           string connectionString = _configuration.GetConnectionString(connectionName);
            return new SqlConnection(connectionString);
        }
    }
}
