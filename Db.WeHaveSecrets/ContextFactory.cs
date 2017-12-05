using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets
{
    class ContextFactory : IDesignTimeDbContextFactory<WeHaveSecretsContext>
    {
        public WeHaveSecretsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeHaveSecretsContext>();
            optionsBuilder.UseSqlServer("Server=LT004447\\SQLEXPRESS;Database=WeHaveSecrets;Trusted_Connection=True;");
            return new WeHaveSecretsContext(optionsBuilder.Options);
        }
    }
}
