using Microsoft.AspNetCore.Identity;
using Moq;
using SecretsRUs.Models;
using SecretsRUs.Repositories;
using SecretsRUs.Services.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SecretsRUs.Tests.Unit.Services.Identity
{
    public class CustomUserStoreTests
    {
        [Fact]
        public async void ConstructorWithoutRepoThrowsArgumentNullException()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new CustomUserStore(null)
            ));

            Assert.Contains("repository", ex.Message);
        }

        [Fact]
        public async void CreateAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.CreateAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void CreateAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.CreateAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void CreateAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.CreateAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void CreateAsyncReturnsSuccessIfSaved()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Equal(IdentityResult.Success, await sut.CreateAsync(user, cancellationToken));
        }

        [Fact]
        public async void CreateAsyncReturnsFailureIfNotSaved()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(false);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.NotEqual(IdentityResult.Success, await sut.CreateAsync(user, cancellationToken));
        }

        [Fact]
        public async void DeleteAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.DeleteAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void FindByIdAsyncThrowsNotImplementedException()
        {
            var userId = Guid.NewGuid().ToString();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByIdAsync(userId, cancellationToken)
            );
        }

        [Fact]
        public async void FindByNameAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.FindByNameAsync(null, cancellationToken)
            );

            Assert.Contains("normalizedUserName", ex.Message);
        }

        [Fact]
        public async void FindByNameAyncCancelledTokenThrowsException()
        {
            var normalizedUserName = "TEST";
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.FindByNameAsync(normalizedUserName, cancellationToken)
            );
        }

        [Fact]
        public async void FindByNameAsyncIfDisposedThrowsObjectDisposedException()
        {
            var normalizedUserName = "TEST";
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.FindByNameAsync(normalizedUserName, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void FindByNameAsyncReturnsModelIfFound()
        {
            var normalizedUserName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindByName(It.IsAny<string>()))
                .Returns(
                    new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = normalizedUserName
                    }
            );
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            var result = await sut.FindByNameAsync(normalizedUserName, cancellationToken);

            Assert.Equal(normalizedUserName, result.UserName);
        }

        [Fact]
        public async void FindByNameAsyncReturnsNullIfNotFound()
        {
            var normalizedUserName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindByName(It.IsAny<string>())).Returns<ApplicationUser>(null);
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(false);
            var sut = new CustomUserStore(mockRepository.Object);

            var result = await sut.FindByNameAsync(normalizedUserName, cancellationToken);

            Assert.Null(result);
        }





        [Fact]
        public async void GetNormalizedUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetNormalizedUserNameAsync(user, cancellationToken)
            );
        }

        #region GetPasswordAsync

        [Fact]
        public async void GetPasswordHashAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.GetPasswordHashAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void GetPasswordHashAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.GetPasswordHashAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetPasswordHashAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.GetPasswordHashAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void GetPasswordHashAsyncReturnsPasswordHash()
        {
            var user = new ApplicationUser
            {
                PasswordHash = "TEST1234"
            };
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Equal("TEST1234", await sut.GetPasswordHashAsync(user, cancellationToken));
        }

        #endregion GetPasswordAsync

        #region GetUserIdAsync

        [Fact]
        public async void GetUserIdAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.GetUserIdAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void GetUserIdAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.GetUserIdAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUserIdAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.GetUserIdAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void GetUserIdAsyncReturnsId()
        {
            var user = new ApplicationUser
            {
                Id = "TEST1234"
            };
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Equal("TEST1234", await sut.GetUserIdAsync(user, cancellationToken));
        }

        #endregion GetUserIdAsync

        #region GetUserNameAsync
        [Fact]
        public async void GetUserNameAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.GetUserNameAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void GetUserNameAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.GetUserNameAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUserNameAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.GetUserNameAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void GetUserNameAsyncReturnsUserName()
        {
            var user = new ApplicationUser
            {
                UserName = "TEST1234"
            };
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Equal("TEST1234", await sut.GetUserNameAsync(user, cancellationToken));
        }
        #endregion GetUserNameAsync

        [Fact]
        public async void HasPasswordAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.HasPasswordAsync(user, cancellationToken)
            );
        }

        #region SetNormalizedUserNameAsync

        [Fact]
        public async void SetNormalizedUserNameAsyncWithoutUserThrowsArgumentNullException()
        {
            var normalizedName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.SetNormalizedUserNameAsync(null, normalizedName, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void SetNormalizedUserNameAsyncWithoutNormalizedNameThrowsArgumentNullException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.SetNormalizedUserNameAsync(user, null, cancellationToken)
            );

            Assert.Contains("normalizedName", ex.Message);
        }

        [Fact]
        public async void SetNormalizedUserNameAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var normalizedName = "TEST";
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.SetNormalizedUserNameAsync(user, normalizedName, cancellationToken)
            );
        }

        [Fact]
        public async void SetNormalizedUserNameAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var normalizedName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.SetNormalizedUserNameAsync(user, normalizedName, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void SetNormalizedUserNameAsyncSetsUserName()
        {
            var user = new ApplicationUser();
            var normalizedName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            await sut.SetNormalizedUserNameAsync(user, normalizedName, cancellationToken);

            Assert.Equal("TEST", user.NormalizedUserName);
        }
        #endregion SetNormalizedUserNameAsync

        #region SetPasswordHashAsync

        [Fact]
        public async void SetPasswordHashAsyncWithoutUserThrowsArgumentNullException()
        {
            var passwordHash = "ABCDEF123456";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.SetPasswordHashAsync(null, passwordHash, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void SetPasswordHashAsyncWithoutPasswordHashThrowsArgumentNullException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.SetPasswordHashAsync(user, null, cancellationToken)
            );

            Assert.Contains("passwordHash", ex.Message);
        }

        [Fact]
        public async void SetPasswordHashAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var passwordHash = "ABCDEF123456";
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.SetPasswordHashAsync(user, passwordHash, cancellationToken)
            );
        }

        [Fact]
        public async void SetPasswordHashAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var passwordHash = "ABCDEF123456";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.SetPasswordHashAsync(user, passwordHash, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void SetPasswordHashAsyncSetsPasswordHash()
        {
            var user = new ApplicationUser();
            var passwordHash = "ABCDEF123456";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            await sut.SetPasswordHashAsync(user, passwordHash, cancellationToken);

            Assert.Equal("ABCDEF123456", user.PasswordHash);
        }
        #endregion SetPasswordHashAsync

        [Fact]
        public async void SetUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var userName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetUserNameAsync(user, userName, cancellationToken)
            );
        }

        [Fact]
        public async void UpdateAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.UpdateAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void AddToRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.AddToRoleAsync(user, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void GetRolesAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRolesAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUsersInRoleAsyncThrowsNotImplementedException()
        {
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetUsersInRoleAsync(roleName, cancellationToken)
            );
        }

        [Fact]
        public async void IsInRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.IsInRoleAsync(user, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void RemoveFromRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.RemoveFromRoleAsync(user, roleName, cancellationToken)
            );
        }
    }
}
