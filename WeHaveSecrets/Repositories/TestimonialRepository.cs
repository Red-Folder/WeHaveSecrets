using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models.Testimonials;

namespace WeHaveSecrets.Repositories
{
    public class TestimonialRepository : ITestimonialRepository
    {
        private string _connectionString;

        public TestimonialRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        public void Save(Testimonial testimonial)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"insert into Testimonials (Id, UserId, Comment, Created) Values ('{Guid.NewGuid().ToString()}', '{testimonial.UserId}', '{testimonial.Comment}', '{testimonial.Created.ToString("yyyy-MM-dd HH:mm:ss")}')";
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new ApplicationException("Unable to save");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
