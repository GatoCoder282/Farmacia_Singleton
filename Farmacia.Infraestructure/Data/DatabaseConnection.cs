using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Farmacia.Infraestructure.Data
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private static DatabaseConnection? _instance;
        private readonly string _connectionString;

        private DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void Initialize(string connectionString)
        {
            if (_instance == null)
                _instance = new DatabaseConnection(connectionString);
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("DatabaseConnection no inicializado. Llama Initialize() en Program.cs");
                return _instance;
            }
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
