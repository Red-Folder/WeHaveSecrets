using WeHaveSecrets.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Repositories
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
        
        public ApplicationUser FindByName(string name)
        {
            ApplicationUser results = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id, Username, PasswordHash from Users where Users.Username = '{name}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        results = new ApplicationUser();
                        results.Id = reader[0].ToString();
                        results.UserName = reader[1].ToString();
                        results.NormalizedUserName = results.UserName;
                        results.PasswordHash = reader[2].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public ApplicationUser FindById(string userId)
        {
            ApplicationUser results = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id, Username, PasswordHash from Users where Users.Id = '{userId}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        results = new ApplicationUser();
                        results.Id = reader[0].ToString();
                        results.UserName = reader[1].ToString();
                        results.NormalizedUserName = results.UserName;
                        results.PasswordHash = reader[2].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public bool Create(ApplicationUser user)
        {
            var result = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"insert into Users (Id, Username, PasswordHash) Values ('{user.Id}', '{user.UserName}', '{user.PasswordHash}')";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        public ApplicationRole FindRoleById(string roleId)
        {
            ApplicationRole results = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id from Roles where Roles.Id = '{roleId}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        results = new ApplicationRole();
                        results.Id = reader[0].ToString();
                        results.Name = reader[0].ToString();
                        results.NormalizedName = reader[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public bool IsInRole(string userId, string roleId)
        {
            bool results = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id, UserId, RoleId from UserRoles where UserRoles.UserId = '{userId}' and UserRoles.RoleId = '{roleId}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        results = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }

        public void AddToRole(string userId, string roleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"insert into UserRoles (Id, UserId, RoleId) Values ('{Guid.NewGuid().ToString()}', '{userId}', '{roleId}')";
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

        public IList<string> GetUserRoles(string userId)
        {
            var results = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"select Id, UserId, RoleId from UserRoles where UserRoles.UserId = '{userId}'";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(reader[2].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return results;
        }
    }
}
