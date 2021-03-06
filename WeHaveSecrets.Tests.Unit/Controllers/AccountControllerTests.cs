﻿using WeHaveSecrets.Controllers;
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
using WeHaveSecrets.Models.AccountViewModels;
using WeHaveSecrets.Tests.Unit.TestUtils;

namespace WeHaveSecrets.Tests.Unit.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async void ConstructorWithoutUserManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(null, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object)
            ));

            Assert.Contains("userManager", ex.Message);
        }

        [Fact]
        public async void ConstructorWithoutSignInManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(IdentityMocks.UserManager().Object, null, IdentityMocks.RoleManager().Object)
            ));

            Assert.Contains("signInManager", ex.Message);
        }

        [Fact]
        public async void ConstructorWithoutRoleManagerThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new AccountController(IdentityMocks.UserManager().Object, IdentityMocks.SignInManager().Object, null)
            ));

            Assert.Contains("roleManager", ex.Message);
        }

        [Fact]
        public void LoginReturnsView()
        {
            var signInManager = IdentityMocks.SignInManager();
            signInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

            var sut = new AccountController(IdentityMocks.UserManager().Object, signInManager.Object, IdentityMocks.RoleManager().Object);
            var result = sut.Login().Result;
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ValidLoginReturnsRedirect()
        {
            var signInManager = IdentityMocks.SignInManager();
            signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<bool>(),
                                                            It.IsAny<bool>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var sut = new AccountController(IdentityMocks.UserManager().Object, signInManager.Object, IdentityMocks.RoleManager().Object);

            var viewModel = new LoginViewModel();
            var returnTo = "Index";

            var result = sut.Login(viewModel, returnTo).Result;

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void FailedLoginReturnsView()
        {
            var signInManager = IdentityMocks.SignInManager();
            signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<bool>(),
                                                            It.IsAny<bool>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var sut = new AccountController(IdentityMocks.UserManager().Object, signInManager.Object, IdentityMocks.RoleManager().Object);

            var viewModel = new LoginViewModel
            {
                Username = "User1",
                Password = "12345"
            };
            var returnTo = "Index";

            var result = sut.Login(viewModel, returnTo).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal("User1", model.Username);
            Assert.Equal("12345", model.Password);
        }

        [Fact]
        public void InvalidLoginReturnsView()
        {
            var sut = new AccountController(IdentityMocks.UserManager().Object, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object);
            sut.ModelState.AddModelError("Username", "Required");

            var viewModel = new LoginViewModel
            {
                Username = null,
                Password = "12345"
            };
            var returnTo = "Index";

            var result = sut.Login(viewModel, returnTo).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Null(model.Username);
            Assert.Equal("12345", model.Password);
            Assert.Single(viewResult.ViewData.ModelState);
        }

        [Fact]
        public void RegisterReturnsView()
        {
            var sut = new AccountController(IdentityMocks.UserManager().Object, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object);
            var result = sut.Login().Result;
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ValidRegistrtionReturnsRedirect()
        {
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

            var sut = new AccountController(userManager.Object, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object);

            var viewModel = new RegisterViewModel
            {
                Username = "TestUser",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };
            var returnTo = "Index";

            var result = sut.Register(viewModel, returnTo).Result;

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void FailedRegistrationReturnsView()
        {
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(null)));

            var sut = new AccountController(userManager.Object, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object);

            var viewModel = new RegisterViewModel
            {
                Username = "TestUser",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };
            var returnTo = "Index";

            var result = sut.Register(viewModel, returnTo).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal("TestUser", model.Username);
            Assert.Equal("TestPassword", model.Password);
            Assert.Equal("TestPassword", model.ConfirmPassword);
        }

        [Fact]
        public void InvalidRegistrationReturnsView()
        {
            var sut = new AccountController(IdentityMocks.UserManager().Object, IdentityMocks.SignInManager().Object, IdentityMocks.RoleManager().Object);
            sut.ModelState.AddModelError("Username", "Required");

            var viewModel = new RegisterViewModel
            {
                Username = null,
                Password = "12345",
                ConfirmPassword = "12345"
            };
            var returnTo = "Index";

            var result = sut.Register(viewModel, returnTo).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Null(model.Username);
            Assert.Equal("12345", model.Password);
            Assert.Equal("12345", model.ConfirmPassword);
            Assert.Single(viewResult.ViewData.ModelState);
        }

        [Fact]
        public void ValidRegistrationAddedUserToUsersRole()
        {
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "User")))
                        .Returns(Task.FromResult(IdentityResult.Success));
            var roleManager = IdentityMocks.RoleManager();
            roleManager.Setup(x => x.FindByIdAsync(It.Is<string>(y => y == "User")))
                        .Returns(Task.FromResult(new ApplicationRole
                        {
                            Id = "User",
                            Name = "User"
                        }));

            var sut = new AccountController(userManager.Object, IdentityMocks.SignInManager().Object, roleManager.Object);

            var viewModel = new RegisterViewModel
            {
                Username = "TestUser",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };

            var result = sut.Register(viewModel).Result;

            roleManager.Verify(x => x.FindByIdAsync(It.Is<string>(y => y == "User")), Times.Once);
            roleManager.Verify(x => x.FindByIdAsync(It.Is<string>(y => y == "Admin")), Times.Never);
            userManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "User")), Times.Once);
            userManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "Admin")), Times.Never);
        }

        [Fact]
        public void ValidAdminRegistrationAddedUserToUsersAndAdminRole()
        {
            var userManager = IdentityMocks.UserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "User")))
                        .Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "Admin")))
                        .Returns(Task.FromResult(IdentityResult.Success));
            var roleManager = IdentityMocks.RoleManager();
            roleManager.Setup(x => x.FindByIdAsync(It.Is<string>(y => y == "User")))
                        .Returns(Task.FromResult(new ApplicationRole
                        {
                            Id = "User",
                            Name = "User"
                        }));
            roleManager.Setup(x => x.FindByIdAsync(It.Is<string>(y => y == "Admin")))
                        .Returns(Task.FromResult(new ApplicationRole
                        {
                            Id = "Admin",
                            Name = "Admin"
                        }));

            var sut = new AccountController(userManager.Object, IdentityMocks.SignInManager().Object, roleManager.Object);

            var viewModel = new RegisterViewModel
            {
                Username = "TestUser.Admin",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };

            var result = sut.Register(viewModel).Result;

            roleManager.Verify(x => x.FindByIdAsync(It.Is<string>(y => y == "User")), Times.Once);
            roleManager.Verify(x => x.FindByIdAsync(It.Is<string>(y => y == "Admin")), Times.Once);
            userManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "User")), Times.Once);
            userManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.Is<string>(y => y == "Admin")), Times.Once);
        }
    }
}
