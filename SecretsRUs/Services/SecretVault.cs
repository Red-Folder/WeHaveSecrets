using SecretsRUs.Models;
using SecretsRUs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Services
{
    public class SecretVault
    {
        private string _userId;
        private ISecretsRepository _repository;

        public SecretVault(string userId, ISecretsRepository repository)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (repository == null) throw new ArgumentNullException("repository");

            _userId = userId;
            _repository = repository;
        }

        public List<Secret> GetAll()
        {
            return _repository.GetAll(_userId);
        }

        public void Save(Secret secret)
        {
            if (secret == null) throw new ArgumentNullException("secret");

            _repository.Save(_userId, secret);
        }
    }
}
