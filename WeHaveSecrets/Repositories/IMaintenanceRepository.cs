using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Repositories
{
    public interface IMaintenanceRepository
    {
        bool BackupTo(string filename);
    }
}
