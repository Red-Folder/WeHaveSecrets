using Microsoft.AspNetCore.Mvc;
using Moq;
using WeHaveSecrets.Controllers;
using WeHaveSecrets.Models;
using WeHaveSecrets.Services;
using WeHaveSecrets.Services.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WeHaveSecrets.Tests.Unit.TestUtils;
using WeHaveSecrets.Models.Secrets;
using Microsoft.AspNetCore.Identity;
using System.Threading;

namespace WeHaveSecrets.Tests.Unit.Controllers
{
    public class AdminControllerTests
    {
        [Fact]
        public async void IndexGetWithoutUserManagerThrowsArgumentExcaption()
        {
            var vault = new Mock<IAdminVault>();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AdminController(null, vault.Object)
            ));

            Assert.Contains("userManager", ex.Message);
        }

        [Fact]
        public async void IndexGetWithoutVaultThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AdminController(IdentityMocks.UserManager().Object, null))
            );

            Assert.Contains("vault", ex.Message);
        }

        [Fact]
        public void IndexReturnsView()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var result = sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void SecretsForReturnsView()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var result = sut.SecretsFor();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SecretsForPostWithoutUsernameThrowsArgumentException()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => 
                sut.SecretsFor(null)
            );

            Assert.Contains("userName", ex.Message);
        }

        [Fact]
        public void SecretsForPostReturnsViewWithModel()
        {
            var vault = new Mock<IAdminVault>();
            vault.Setup(x => x.GetAll()).Returns(new List<Secret>
            {
                new Secret(),
                new Secret()
            });
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                        .Returns(Task.FromResult(new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString()
                        }));
            var sut = new AdminController(userManager.Object, vault.Object);

            var result = sut.SecretsFor("User1").Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SecretsForViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Secrets.Count);
        }

        [Fact]
        public void ChangePasswordReturnsView()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var result = sut.ChangePassword();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void ChangePasswordPostWithoutPasswordStoreThrowsArgumentException()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            string userName = "User1";
            string newPassword = "NewPassword";
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.ChangePassword(null, userName, newPassword)
            );

            Assert.Contains("passwordStore", ex.Message);
        }

        [Fact]
        public async void ChangePasswordPostWithoutUsernameThrowsArgumentException()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var passwordStore = new Mock<IUserPasswordStore<ApplicationUser>>();
            string newPassword = "NewPassword";
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.ChangePassword(passwordStore.Object, null, newPassword)
            );

            Assert.Contains("userName", ex.Message);
        }

        [Fact]
        public async void ChangePasswordPostWithoutNewPasswordThrowsArgumentException()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var passwordStore = new Mock<IUserPasswordStore<ApplicationUser>>();
            string userName = "User1";
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                sut.ChangePassword(passwordStore.Object, userName, null)
            );

            Assert.Contains("newPassword", ex.Message);
        }

        [Fact]
        public void ChangePasswordPostReturnsViewWithModel()
        {
            var vault = new Mock<IAdminVault>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            passwordHasher.Setup(x => x.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .Returns("HashedPassword");
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.FindByNameAsync(It.Is<string>(y => y == "User1")))
                    .Returns(Task.FromResult(new ApplicationUser
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                UserName = "User1"
                                            }));
            //userManager.SetupGet(x => x.PasswordHasher).Returns(passwordHasher.Object);
            var userManagerObject = userManager.Object;
            userManagerObject.PasswordHasher = passwordHasher.Object;
            var sut = new AdminController(userManagerObject, vault.Object);

            var passwordStore = new Mock<IUserPasswordStore<ApplicationUser>>();
            string userName = "User1";
            string newPassword = "NewPassword";

            var result = sut.ChangePassword(passwordStore.Object, userName, newPassword).Result;

            var viewResult = Assert.IsType<ViewResult>(result);

            passwordStore.Verify(x => x.SetPasswordHashAsync(It.Is<ApplicationUser>(y => y.UserName == "User1"),
                                                            It.Is<string>(y => y == "HashedPassword"),
                                                            It.IsAny<CancellationToken>()),
                                                            Times.Once);
            passwordStore.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(y => y.UserName == "User1"),
                                                    It.IsAny<CancellationToken>()),
                                                    Times.Once);
        }

        [Fact]
        public async void BackupDatabaseWithoutDatabaseMaintenanceThrowsArgumentException()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                sut.BackupDatabase(null)
            ));

            Assert.Contains("databaseMaintenance", ex.Message);
        }

        [Fact]
        public void BackupDatabasePerformsBackupAndReturnsViewWithModel()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var databaseMaintenance = new Mock<IDatabaseMaintenance>();
            databaseMaintenance.Setup(x => x.Backup()).Returns(true);
            databaseMaintenance.Setup(x => x.Backups())
                                .Returns(new List<Backup>
                                {
                                    new Backup(),
                                    new Backup()
                                });

            var result = sut.BackupDatabase(databaseMaintenance.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BackupsViewModel>(viewResult.ViewData.Model);
            Assert.True(model.Successful);
            Assert.Equal(2, model.AvailableBackups.Count);
        }

        [Fact]
        public void BackupDatabaseReturnsAnErrorIfBackupFails()
        {
            var vault = new Mock<IAdminVault>();
            var userManager = IdentityMocks.UserManager();
            var sut = new AdminController(userManager.Object, vault.Object);

            var databaseMaintenance = new Mock<IDatabaseMaintenance>();

            var result = sut.BackupDatabase(databaseMaintenance.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BackupsViewModel>(viewResult.ViewData.Model);
            Assert.False(model.Successful);
            Assert.Contains("Backup failed", model.ErrorMessage);
        }
    }
}
