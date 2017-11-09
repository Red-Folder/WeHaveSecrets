using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace WeHaveSecrets.Services.Secrets
{
    public class UserVault : AbstractVault
    {
        private readonly string _userId;

        public UserVault(string userId, ISecretsRepository repository): base(repository)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            _userId = userId;
        }

        public override List<Secret> GetAll()
        {
            return _repository.GetAll(_userId);
        }

        public override void Save(Secret secret)
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
