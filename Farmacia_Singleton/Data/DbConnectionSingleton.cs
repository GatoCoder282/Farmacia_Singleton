
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient; 

namespace Farmacia_Singleton.Data
{
    public sealed class DbConnectionSingleton
    {
        private static readonly Lazy<DbConnectionSingleton> _instance =
            new(() => new DbConnectionSingleton());

        private readonly string _connectionString;

        private DbConnectionSingleton()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            _connectionString = config.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
        }

        public static DbConnectionSingleton Instance => _instance.Value;

        public MySqlConnection GetConnection() => new MySqlConnection(_connectionString);
    }
}
