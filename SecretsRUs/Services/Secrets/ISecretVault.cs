using SecretsRUs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Services.Secrets
{
    public interface ISecretVault
    {
        List<Secret> GetAll();
        void Save(Secret secret);
    }
}
