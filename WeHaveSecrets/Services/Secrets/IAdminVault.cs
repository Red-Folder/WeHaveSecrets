using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Services.Secrets
{
    public interface IAdminVault: ISecretVault
    {
        string UserId { set; get; }
    }
}
