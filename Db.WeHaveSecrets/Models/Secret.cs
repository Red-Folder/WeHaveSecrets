using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets.Models
{
    public class Secret
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
