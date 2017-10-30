using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Models.Converters
{
    public class SecretDomainFromViewModel: Secret
    {
        public SecretDomainFromViewModel(SecretViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException("vm");

            Id = vm.Id;
            Key = vm.Key;
            Value = vm.Value;
        }
    }
}
