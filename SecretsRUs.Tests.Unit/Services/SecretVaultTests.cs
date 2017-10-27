using Moq;
using SecretsRUs.Models;
using SecretsRUs.Repositories;
using SecretsRUs.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecretsRUs.Tests.Unit.Services
{
    public class SecretVaultTests
    {
        [Fact]
        public async void CreatingWithoutAUserIdWillThrowNullArgumentException()
        {
            var repository = new Mock<ISecretsRepository>();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretVault(null, repository.Object)
            ));

            Assert.Contains("userId", ex.Message);
        }

        [Fact]
        public async void CreatingWithoutARepositoryWillThrowNullArgumentException()
        {
            var userId = Guid.NewGuid().ToString();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretVault(userId, null)
            ));

            Assert.Contains("repository", ex.Message);
        }

        [Fact]
        public void ReturnsSecretsForAUser()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            repository.Setup(x => x.GetAll(It.IsAny<string>())).Returns(new List<Secret>
            {
                new Secret(),
                new Secret()
            });
            var sut = new SecretVault(userId, repository.Object);

            Assert.Equal(2, sut.GetAll().Count);
        }

        [Fact]
        public void SavesWithNoSecretThrowsArgumentException()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            var sut = new SecretVault(userId, repository.Object);

            var ex = Assert.Throws<ArgumentNullException>(() => 
                sut.Save(null)
            );

            Assert.Contains("secret", ex.Message);
        }

        [Fact]
        public void SavesSecretForAUser()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            repository.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<Secret>()));
            var sut = new SecretVault(userId, repository.Object);

            sut.Save(new Secret());

            repository.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<Secret>()), Times.Once());
        }
    }
}
