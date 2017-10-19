using SecretsRUs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Repositories
{
    public interface IIdentityRepository
    {
        ApplicationUser FindByName(string name);
        ApplicationUser FindById(string userId);
        bool Create(ApplicationUser user);
    }
}
