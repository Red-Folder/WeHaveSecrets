using SecretsRUs.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Repositories
{
    public class IdentityRepository: IIdentityRepository
    {
        private string _connectionString;

        public IdentityRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }
        
        public List<string> Read()
        {
            var results = new List<string>();
            using (var connection = new SqlConnection("Server=LT004447\\SQLEXPRESS;Database=SecretsRUs;Trusted_Connection=True;"))
            {
                var query = "select name from Test";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(reader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public void Add(string name)
        {
            using (var connection = new SqlConnection("Server=LT004447\\SQLEXPRESS;Database=SecretsRUs;Trusted_Connection=True;"))
            {
                var query = $"insert into Test (Name) Values ('{name}')";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ApplicationUser FindByName(string name)
        {
            return null;
        }

        public bool Create(ApplicationUser user)
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
