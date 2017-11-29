using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Repositories;

namespace WeHaveSecrets.Services.Secrets
{
    public class AdminVault : UserVault, IAdminVault
    {
        public AdminVault(ISecretsRepository repository): base("NotSet", repository)
        {
        }

        public string UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }
    }
}
