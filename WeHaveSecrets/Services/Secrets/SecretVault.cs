using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Services.Secrets
{
    public class SecretVault : ISecretVault
    {
        private readonly string _userId;
        private readonly ISecretsRepository _repository;

        public SecretVault(string userId, ISecretsRepository repository)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (repository == null) throw new ArgumentNullException("repository");

            _userId = userId;
            _repository = repository;
        }

        public Secret Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Secret> GetAll()
        {
            return _repository.GetAll(_userId);
        }

        public void Save(Secret secret)
        {
            if (secret == null) throw new ArgumentNullException("secret");

            if (secret.Id <= 0)
            {
                _repository.Add(_userId, secret);
            }
            else
            {
                throw new NotImplementedException("Update has not been implemented yet");
            }
        }
    }
}
