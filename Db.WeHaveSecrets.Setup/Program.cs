using System;
using Microsoft.EntityFrameworkCore;

namespace Db.WeHaveSecrets.Setup
{
    class Program
    {
        private const string ENV_CONNECTION_STRING = "CONNECTIONSTRING";
        private const int OK = 0;
        private const int ERROR = -1;

        static int Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable(ENV_CONNECTION_STRING);

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine($"[ERROR] Environment variable ${ENV_CONNECTION_STRING} not set");

                return ERROR;
            }
            else
            {
                try
                {
                    MigrateDatabase(args[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERROR] Error occurred during migration");
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.StackTrace);

                    return ERROR;
                }
            }

            return OK;
        }

        private static void MigrateDatabase(string connectionString)
        {
            Console.WriteLine("Creating database context");

            var optionsBuilder = new DbContextOptionsBuilder<WeHaveSecretsContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new WeHaveSecretsContext(optionsBuilder.Options))
            {
                Console.WriteLine("Running migration");
                context.Database.Migrate();
            }

            Console.WriteLine("Completed");
        }
    }
}
