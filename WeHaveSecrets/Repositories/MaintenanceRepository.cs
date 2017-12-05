using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private string _connectionString;

        public MaintenanceRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        public bool BackupTo(string filename)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"backup database WeHaveSecrets to disk = '{filename}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
