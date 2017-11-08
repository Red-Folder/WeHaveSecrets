using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models;
using System.Data.SqlClient;

namespace WeHaveSecrets.Repositories
{
    public class SecretsRepository : ISecretsRepository
    {
        private string _connectionString;

        public SecretsRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        public List<Secret> GetAll(string userId)
        {
            var results = new List<Secret>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id, [Key], [Value] from Secrets where Secrets.UserId = '{userId}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var secret = new Secret();
                        secret.Id = (int)reader[0];
                        secret.Key = reader[1].ToString();
                        secret.Value = reader[2].ToString();
                        results.Add(secret);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public Secret Add(string userId, Secret secret)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"insert into Secrets (UserId, [Key], [Value]) Values ('{userId}', '{secret.Key}', '{secret.Value}')";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new ApplicationException("Unable to save");
                    }

                    secret.Id = GetLastIdentity(connection);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return secret;
        }

        private int GetLastIdentity(SqlConnection connection)
        {
            var id = -1;
            var query = $"select @@IDENTITY";
            var command = new SqlCommand(query, connection);

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                id = Convert.ToInt32((Decimal)reader[0]);
            }

            return id;
        }
    }
}
