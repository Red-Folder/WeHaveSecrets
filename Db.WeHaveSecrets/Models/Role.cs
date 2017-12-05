using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Db.WeHaveSecrets.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
