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

namespace WeHaveSecrets.Tests.Unit.Services
{
    public class AdminVaultTests
    {
        [Fact]
        public async void CreatingWithoutARepositoryWillThrowNullArgumentException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AdminVault(null)
            ));

            Assert.Contains("repository", ex.Message);
        }

        [Fact]
        public async void SettingUserIdToNullThrowsException()
        {
            var repository = new Mock<ISecretsRepository>();
            var sut = new AdminVault(repository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                sut.UserId = null
            ));

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public void ReturnsSecretsForAUser()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            repository.Setup(x => x.GetAll(It.Is<string>(y => y == userId))).Returns(new List<Secret>
            {
                new Secret(),
                new Secret()
            });
            var sut = new AdminVault(repository.Object);
            sut.UserId = userId;

            Assert.Equal(2, sut.GetAll().Count);
        }

        [Fact]
        public void SavesWithNoSecretThrowsArgumentException()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            var sut = new AdminVault(repository.Object);

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
            repository.Setup(x => x.Add(It.Is<string>(y => y == userId), It.IsAny<Secret>()));
            var sut = new AdminVault(repository.Object);
            sut.UserId = userId;

            sut.Save(new Secret());

            repository.Verify(x => x.Add(It.Is<string>(y => y == userId), It.IsAny<Secret>()), Times.Once());
        }

        [Fact]
        public void SavesAnUpdatedSecretForAUserReturnsNotImplemented()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();

            var sut = new AdminVault(repository.Object);
            var secret = new Secret
            {
                Id = 1234
            };
            var ex = Assert.Throws<NotImplementedException>(() => 
                sut.Save(secret)
            );

            Assert.Contains("Update has not been implemented yet", ex.Message);
        }

        [Fact]
        public void ReturnsSecretForGet()
        {
            var userId = Guid.NewGuid().ToString();
            var repository = new Mock<ISecretsRepository>();
            repository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Secret
            {
                Id = 1,
                Key = "SecretKey",
                Value = "SecretValue"
            });
            var sut = new AdminVault(repository.Object);

            var result = sut.Get(1);

            Assert.Equal(1, result.Id);
            Assert.Equal("SecretKey", result.Key);
            Assert.Equal("SecretValue", result.Value);
        }
    }
}
