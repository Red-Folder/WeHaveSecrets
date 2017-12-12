using Microsoft.AspNetCore.Identity;
using Moq;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using WeHaveSecrets.Services.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WeHaveSecrets.Tests.Unit.Services.Identity
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

        #region CreateAsync
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
        #endregion

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

        #region FindByIdAsync
        [Fact]
        public async void FindByIdAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.FindByIdAsync(null, cancellationToken)
            );

            Assert.Contains("userId", ex.Message);
        }

        [Fact]
        public async void FindByIdAsyncCancelledTokenThrowsException()
        {
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.FindByIdAsync("1234", cancellationToken)
            );
        }

        [Fact]
        public async void FindByIdAsyncIfDisposedThrowsObjectDisposedException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.FindByIdAsync("1234", cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void FindByIdAsyncReturnsRoleIfFound()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindById(It.IsAny<string>())).Returns(new ApplicationUser
            {
                Id = "1234",
                UserName = "TEST"
            });
            var sut = new CustomUserStore(mockRepository.Object);

            var role = await sut.FindByIdAsync("1234", cancellationToken);
            Assert.Equal("1234", role.Id);
        }

        [Fact]
        public async void FindByIdAsyncReturnsNullIfNotFound()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.FindById(It.IsAny<string>())).Returns<ApplicationUser>(null);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Null(await sut.FindByIdAsync("1234", cancellationToken));
        }

        #endregion FindByIdAsync


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

        #region UpdateAsync
        [Fact]
        public async void UpdateAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.UpdateAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void UpdateAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.UpdateAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void UpdateAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.UpdateAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void UpdateAsyncReturnsSuccessIfSaved()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Update(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.Equal(IdentityResult.Success, await sut.UpdateAsync(user, cancellationToken));
        }

        [Fact]
        public async void UpdateAsyncReturnsFailureIfNotSaved()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Update(It.IsAny<ApplicationUser>())).Returns(false);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.NotEqual(IdentityResult.Success, await sut.UpdateAsync(user, cancellationToken));
        }
        #endregion

        #region AddToRoleAsync
        [Fact]
        public async void AddToRoleAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.AddToRoleAsync(null, "TEST", cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void AddToRoleAsyncWithoutRoleNameThrowsArgumentNullException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.AddToRoleAsync(user, null, cancellationToken)
            );

            Assert.Contains("roleName", ex.Message);
        }

        [Fact]
        public async void AddToRoleAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.AddToRoleAsync(user, "TEST", cancellationToken)
            );
        }

        [Fact]
        public async void AddToRoleAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.AddToRoleAsync(user, "TEST", cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void AddToRoleAsyncReturnsNothingIfSaved()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await sut.AddToRoleAsync(user, "TEST", cancellationToken);
        }
        #endregion AddToRoleAsync

        #region GetRolesAsync
        [Fact]
        public async void GetRolesAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.GetRolesAsync(null, cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void GetRolesAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.GetRolesAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetRolesAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.GetRolesAsync(user, cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void GetRolesAsyncReturnsListIfFound()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var list = new List<ApplicationRole>
            {
                new ApplicationRole { Id = "TEST1"},
                new ApplicationRole { Id = "TEST2"}
            };
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            var roles = await sut.GetRolesAsync(user, cancellationToken);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async void GetRolesAsyncReturnsEmptyListIfNotFound()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var list = new List<ApplicationRole>();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.Create(It.IsAny<ApplicationUser>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            var roles = await sut.GetRolesAsync(user, cancellationToken);
            Assert.Empty(list);
        }
        #endregion GetRolesAsync

        [Fact]
        public async void GetUsersInRoleAsyncThrowsNotImplementedException()
        {
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetUsersInRoleAsync(roleName, cancellationToken)
            );
        }

        #region IsInRoleAsync
        [Fact]
        public async void IsInRoleAsyncWithoutUserThrowsArgumentNullException()
        {
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.IsInRoleAsync(null, "TEST", cancellationToken)
            );

            Assert.Contains("user", ex.Message);
        }

        [Fact]
        public async void IsInRoleAsyncWithoutRoleIdThrowsArgumentNullException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.IsInRoleAsync(user, null, cancellationToken)
            );

            Assert.Contains("roleName", ex.Message);
        }

        [Fact]
        public async void IsInRoleAsyncCancelledTokenThrowsException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken(true);
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                sut.IsInRoleAsync(user, "TEST", cancellationToken)
            );
        }

        [Fact]
        public async void IsInRoleAsyncIfDisposedThrowsObjectDisposedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);
            sut.Dispose();

            var ex = await Assert.ThrowsAsync<ObjectDisposedException>(() =>
                sut.IsInRoleAsync(user, "TEST", cancellationToken)
            );

            Assert.Contains("CustomUserStore", ex.Message);
        }

        [Fact]
        public async void IsInRoleAsyncReturnsTrueIfFound()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.IsInRole(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.True(await sut.IsInRoleAsync(user, "TEST", cancellationToken));
        }

        [Fact]
        public async void IsInRoleAsyncReturnsFalseIfNotFound()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            mockRepository.Setup(x => x.IsInRole(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var sut = new CustomUserStore(mockRepository.Object);

            Assert.False(await sut.IsInRoleAsync(user, "TEST", cancellationToken));
        }
        #endregion IsInRoleAsync

        [Fact]
        public async void RemoveFromRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var mockRepository = new Mock<IIdentityRepository>();
            var sut = new CustomUserStore(mockRepository.Object);

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.RemoveFromRoleAsync(user, roleName, cancellationToken)
            );
        }
    }
}
