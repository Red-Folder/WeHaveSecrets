using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Models.Converters
{
    public class SecretViewModelFromDomain: SecretViewModel
    {
        public SecretViewModelFromDomain(Secret domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");

            Id = domain.Id;
            Key = domain.Key;
            Value = domain.Value;
        }
    }
}
