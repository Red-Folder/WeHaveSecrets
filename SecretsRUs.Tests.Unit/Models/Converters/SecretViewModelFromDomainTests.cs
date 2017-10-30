using SecretsRUs.Models;
using SecretsRUs.Models.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecretsRUs.Tests.Unit.Models.Converters
{
    public class SecretViewModelFromDomainTests
    {
        [Fact]
        public async void ConstructorWithoutSecretThrowsArgumentException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretViewModelFromDomain(null)
            ));

            Assert.Contains("domain", ex.Message);
        }

        [Fact]
        public void ConstructsValidViewModel()
        {
            var domain = new Secret
            {
                Id = 1234,
                Key = "Key1234",
                Value = "Value1234"
            };

            var vm = new SecretViewModelFromDomain(domain);

            Assert.Equal(domain.Id, vm.Id);
            Assert.Equal(domain.Key, vm.Key);
            Assert.Equal(domain.Value, vm.Value);
        }
    }
}
