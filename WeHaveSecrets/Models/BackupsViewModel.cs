using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Models
{
    public class BackupsViewModel
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }

        public List<BackupViewModel> AvailableBackups { get; set; }
    }
}
