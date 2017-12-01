using Db.WeHaveSecrets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets
{
    public class WeHaveSecretsContext: DbContext
    {
        public WeHaveSecretsContext(DbContextOptions<WeHaveSecretsContext> options): base(options)
        {

        }

        public DbSet<Secret> Secrets { get; set; }
    }
}
