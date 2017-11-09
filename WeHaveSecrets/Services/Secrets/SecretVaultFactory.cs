using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Services.Secrets
{
    public class SecretVaultFactory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISecretsRepository _repository;

        public SecretVaultFactory(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ISecretsRepository repository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public ISecretVault CreateVault()
        {
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

            if (user == null)
            {
                return new AnonymousVault(_repository);
            }
            else
            {
                return new UserVault(user.Id, _repository);
            }
        }
    }
}
