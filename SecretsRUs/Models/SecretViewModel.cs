using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Models
{
    public class SecretViewModel
    {
        public int Id;

        [Required]
        public string Key;

        [Required]
        public string Value;
    }
}
