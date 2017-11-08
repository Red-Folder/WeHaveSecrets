using WeHaveSecrets.Models;
using WeHaveSecrets.Services.Identity;
using System;
using System.Threading;
using Xunit;
using Moq;
using WeHaveSecrets.Repositories;
using System.Threading.Tasks;

namespace WeHaveSecrets.Tests.Unit.Services.Identity
{
    public class CustomRoleStoreTests
    {
        [Fact]
        public async void ConstructorWithoutRepoThrowsArgumentNullException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new CustomRoleStore(null)
            ));

            Assert.Contains("repository", ex.Message);
        }

        [Fact]
        public async void CreateAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.CreateAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void DeleteAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.DeleteAsync(role, cancellationToken)
            );
        }

        #region FindByIdAsync
        [Fact]
        public async void FindByIdAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.FindByIdAsync(null, cancellationToken)
            );

            Assert.Contains("roleId", ex.Message);
        }

        [Fact]
        public async void FindByIdAsyncCancelledTokenThrowsException()
        {
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.FindByIdAsync("1234", cancellationToken)
            );
        }

        [Fact]
        public async void FindByIdAsyncIfDisposedThrowsObjectDisposedException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.FindByIdAsync("1234", cancellationToken)
            );

            Assert.Contains("CustomRoleStore", ex.Message);
        }

        [Fact]
        public async void FindByIdAsyncReturnsRoleIfFound()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindRoleById(It.IsAny<string>())).Returns(new ApplicationRole
            {
                Id = "1234",
                Name = "TEST"
            });
            var sut = new CustomRoleStore(mockRepository.Object);

            var role = await sut.FindByIdAsync("1234", cancellationToken);
            Assert.Equal("1234", role.Id);
        }

        [Fact]
        public async void FindByIdAsyncReturnsNullIfNotFound()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindRoleById(It.IsAny<string>())).Returns<ApplicationRole>(null);
            var sut = new CustomRoleStore(mockRepository.Object);

            Assert.Null(await sut.FindByIdAsync("1234", cancellationToken));
        }
        #endregion FindByIdAsync

        [Fact]
        public async void FindByNameAsyncThrowsNotImplementedException()
        {
            var normalizedRoleName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByNameAsync(normalizedRoleName, cancellationToken)
            );
        }

        [Fact]
        public async void GetNormalizedRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetNormalizedRoleNameAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void GetRoleIdAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRoleIdAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void GetRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRoleNameAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void SetNormalizedRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var normaliseRoleName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetNormalizedRoleNameAsync(role, normaliseRoleName, cancellationToken)
            );
        }

        [Fact]
        public async void SetRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetRoleNameAsync(role, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void UpdateAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomRoleStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.UpdateAsync(role, cancellationToken)
            );
        }
    }
}

