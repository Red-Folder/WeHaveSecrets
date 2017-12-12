using Moq;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using WeHaveSecrets.Services;
using WeHaveSecrets.Services.Secrets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Security.Authentication;

namespace WeHaveSecrets.Tests.Unit.Services.Secrets
{
    public class AnonymousVaultTests
    {
        [Fact]
        public async void CreatingWithoutARepositoryWillThrowNullArgumentException()
        {
            var userId = Guid.NewGuid().ToString();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AnonymousVault(null)
            ));

            Assert.Contains("repository", ex.Message);
        }

        [Fact]
        public void ReturnsSecretForGet()
        {
            var repository = new Mock<ISecretsRepository>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Secret
            {
                Id = 1,
                Key = "SecretKey",
                Value = "SecretValue"
            });
            var sut = new AnonymousVault(repository.Object);

            var result = sut.Get(1);

            Assert.Equal(1, result.Id);
            Assert.Equal("SecretKey", result.Key);
            Assert.Equal("SecretValue", result.Value);
        }

        [Fact]
        public void GetAllThrowsAuthenticationException()
        {
            var repository = new Mock<ISecretsRepository>();
            var sut = new AnonymousVault(repository.Object);

            var ex = Assert.Throws<AuthenticationException>(() =>
                sut.GetAll()
            );

            Assert.Contains("Anonymous access not allowed for this method", ex.Message);
        }

        [Fact]
        public void SaveThrowsAuthenticationException()
        {
            var repository = new Mock<ISecretsRepository>();
            var sut = new AnonymousVault(repository.Object);

            var ex = Assert.Throws<AuthenticationException>(() =>
                sut.Save(new Secret())
            );

            Assert.Contains("Anonymous access not allowed for this method", ex.Message);
        }
    }
}
