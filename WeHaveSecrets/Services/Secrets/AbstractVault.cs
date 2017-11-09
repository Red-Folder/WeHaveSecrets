using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;

namespace WeHaveSecrets.Services.Secrets
{
    public abstract class AbstractVault: ISecretVault
    {
        protected readonly ISecretsRepository _repository;

        public AbstractVault(ISecretsRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            _repository = repository;
        }

        public virtual Secret Get(int id)
        {
            return _repository.Get(id);
        }

        public virtual List<Secret> GetAll()
        {
            throw new AuthenticationException("Anonymous access not allowed for this method");
        }

        public virtual void Save(Secret secret)
        {
            throw new AuthenticationException("Anonymous access not allowed for this method");
        }
    }
}
