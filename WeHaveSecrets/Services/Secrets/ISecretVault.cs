using WeHaveSecrets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Services.Secrets
{
    public interface ISecretVault
    {
        List<Secret> GetAll();
        void Save(Secret secret);
        Secret Get(int id);
    }
}
