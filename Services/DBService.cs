using Microsoft.Data.Sqlite;

namespace BhyatPos.Services
{
    public abstract class DBService
    {
        protected readonly string DBPath;

        protected DBService()
        {
            DBPath = "C:\\Users\\ESA\\Documents\\BhyatPos\\Data\\bhyatposdb.db";
        }

        protected SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={DBPath}");
        }
    }
}
