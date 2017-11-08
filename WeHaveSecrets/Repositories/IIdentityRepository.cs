using WeHaveSecrets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Repositories
{
    public interface IIdentityRepository
    {
        ApplicationUser FindByName(string name);
        ApplicationUser FindById(string userId);
        bool Create(ApplicationUser user);

        ApplicationRole FindRoleById(string roleId);
        bool IsInRole(string userId, string roleId);
        void AddToRole(string userId, string roleId);
        IList<string> GetUserRoles(string userId);
    }
}
