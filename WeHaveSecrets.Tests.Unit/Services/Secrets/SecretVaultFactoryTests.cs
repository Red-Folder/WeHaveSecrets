using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;
using WeHaveSecrets.Services.Secrets;
using Xunit;

namespace WeHaveSecrets.Tests.Unit.Services.Secrets
{
    public class SecretVaultFactoryTests
    {
        [Fact]
        public async void CreatingWithoutAUserManagerWillThrowNullArgumentException()
        {
            var httpContext = new Mock<IHttpContextAccessor>();
            var repository = new Mock<ISecretsRepository>();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretVaultFactory(null, httpContext.Object, repository.Object)
            ));

            Assert.Contains("userManager", ex.Message);
        }

        [Fact]
        public async void CreatingWithoutAHttpContextWillThrowNullArgumentException()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var repository = new Mock<ISecretsRepository>();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretVaultFactory(userManager, null, repository.Object)
            ));

            Assert.Contains("httpContext", ex.Message);
        }

        [Fact]
        public async void CreatingWithoutARepositoryWillThrowNullArgumentException()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContext = new Mock<IHttpContextAccessor>();
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretVaultFactory(userManager, httpContext.Object, null)
            ));

            Assert.Contains("repository", ex.Message);
        }


        [Fact]
        public void IfAnonymousAccessProvideAnonymousVault()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContext = new Mock<IHttpContextAccessor>();
            var repository = new Mock<ISecretsRepository>();
            var sut = new SecretVaultFactory(userManager, httpContext.Object, repository.Object);

            var vault = sut.CreateVault();

            Assert.IsType<AnonymousVault>(vault);
        }

        [Fact]
        public void IfUserLoggedInProvidesUserVault()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            userStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new ApplicationUser()));
            var userManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);

            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            var httpContext = new Mock<IHttpContextAccessor>();
            httpContext.SetupGet(x => x.HttpContext.User).Returns(new ClaimsPrincipal(FakeIdentity()));

            var repository = new Mock<ISecretsRepository>();
            var sut = new SecretVaultFactory(userManager, httpContext.Object, repository.Object);

            var vault = sut.CreateVault();

            userStore.VerifyAll();
            Assert.IsType<UserVault>(vault);
        }

        public ClaimsIdentity FakeIdentity()
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString())
            }, "test");
        }
    }
}
