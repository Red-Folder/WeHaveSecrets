using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SecretsRUs.Models;
using SecretsRUs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Services.Secrets
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

            return new SecretVault(user.Id, _repository);
        }
    }
}
