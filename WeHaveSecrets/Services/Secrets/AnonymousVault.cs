using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Repositories;

namespace WeHaveSecrets.Services.Secrets
{
    public class AnonymousVault : AbstractVault
    {
        public AnonymousVault(ISecretsRepository repository) : base(repository)
        {
        }
    }
}
