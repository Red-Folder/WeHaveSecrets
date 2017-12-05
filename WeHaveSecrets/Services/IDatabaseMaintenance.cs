using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models;

namespace WeHaveSecrets.Services
{
    public interface IDatabaseMaintenance
    {
        bool Backup();

        List<Backup> Backups();
    }
}
