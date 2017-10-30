using SecretsRUs.Models;
using SecretsRUs.Models.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecretsRUs.Tests.Unit.Models.Converters
{
    public class SecretDomainFromViewModelTests
    {
        [Fact]
        public async void ConstructorWithoutSecretThrowsArgumentException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretDomainFromViewModel(null)
            ));

            Assert.Contains("vm", ex.Message);
        }

        [Fact]
        public void ConstructsValidViewModel()
        {
            var vm = new SecretViewModel
            {
                Id = 1234,
                Key = "Key1234",
                Value = "Value1234"
            };

            var domain = new SecretDomainFromViewModel(vm);

            Assert.Equal(vm.Id, domain.Id);
            Assert.Equal(vm.Key, domain.Key);
            Assert.Equal(vm.Value, domain.Value);
        }
    }
}
