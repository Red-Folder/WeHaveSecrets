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
            if (userManager == null) throw new ArgumentNullException("userManager");
            if (httpContextAccessor == null) throw new ArgumentNullException("httpContextAccessor");
            if (repository == null) throw new ArgumentNullException("repository");

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public ISecretVault CreateVault()
        {
            if (CurrentUser == null)
            {
                return new AnonymousVault(_repository);
            }
            else
            {
                return new UserVault(CurrentUser.Id, _repository);
            }
        }

        private ApplicationUser CurrentUser
        {
            get
            {
                var userContext = _httpContextAccessor?.HttpContext?.User;

                if (userContext == null)
                {
                    return null;
                }
                else
                {
                    return _userManager.GetUserAsync(userContext).Result;
                }
            }
        }
    }
}
