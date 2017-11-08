using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Models
{
    public class NewSecretViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Key { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Value { get; set; }

    }
}
