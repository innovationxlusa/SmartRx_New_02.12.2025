using System.Data;

namespace PMSBackend.Databases.Data
{
    public interface IDBContext
    {
        public IDbConnection CreateConnection();
    }
}
