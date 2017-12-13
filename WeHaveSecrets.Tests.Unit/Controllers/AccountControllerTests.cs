using WeHaveSecrets.Controllers;
using WeHaveSecrets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeHaveSecrets.Tests.Unit.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async void ConstructorWithoutUserManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(null, FakeSignInManager().Object, MockRoleManager().Object)
            ));

            Assert.Contains("userManager", ex.Message);
        }

        [Fact]
        public async void ConstructorWithoutSignInManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(MockUserManager().Object, null, MockRoleManager().Object)
            ));

            Assert.Contains("signInManager", ex.Message);
        }

        [Fact]
        public async void ConstructorWithoutRoleManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(MockUserManager().Object, FakeSignInManager().Object, null)
            ));

            Assert.Contains("roleManager", ex.Message);
        }

        [Fact]
        public void LoginGetReturnsView()
        {
            var signInManager = FakeSignInManager();
            signInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

            var sut = new AccountController(MockUserManager().Object, signInManager.Object, MockRoleManager().Object);
            var result = sut.Login().Result;
            Assert.IsType<ViewResult>(result);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager(IUserStore<ApplicationUser> userStore = null)
        {
            userStore = userStore ?? new Mock<IUserStore<ApplicationUser>>().Object;
            return new Mock<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<ApplicationUser>> FakeSignInManager(UserManager<ApplicationUser> userManager = null,
                                                                 IHttpContextAccessor httpContextAccessor = null,
                                                                 IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory = null)
        {
            userManager = userManager ?? MockUserManager().Object;
            httpContextAccessor = httpContextAccessor ?? new Mock<IHttpContextAccessor>().Object;
            userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object;
            return new Mock<SignInManager<ApplicationUser>>(userManager, httpContextAccessor, userClaimsPrincipalFactory, null, null, null);
        }

        private Mock<RoleManager<ApplicationRole>> MockRoleManager(IRoleStore<ApplicationRole> roleStore = null)
        {
            roleStore = roleStore ?? new Mock<IRoleStore<ApplicationRole>>().Object;
            return new Mock<RoleManager<ApplicationRole>>(roleStore, null, null, null, null);
        }
    }
}
