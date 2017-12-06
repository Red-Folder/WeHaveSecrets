using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public List<Testimonial> Testimonials { get; set; }
    }
}
