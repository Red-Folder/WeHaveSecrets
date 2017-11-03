﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SecretsRUs.Controllers;
using SecretsRUs.Models;
using SecretsRUs.Services;
using SecretsRUs.Services.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecretsRUs.Tests.Unit.Controllers
{
    public class SecretControllerTests
    {
        [Fact]
        public async void IndexGetWithoutVaultThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretsController().Index(null)
            ));

            Assert.Contains("vault", ex.Message);
        }

        [Fact]
        public async void IndexPostWithoutVaultThrowsArgumentExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretsController().Index(null, new NewSecretViewModel())
            ));

            Assert.Contains("vault", ex.Message);
        }



        [Fact]
        public void IndexReturnsViewWithModel()
        {
            var secrets = new List<Secret>
            {
                new Secret
                {
                    Id = 1,
                    Key = "Key1",
                    Value = "Value1"
                },
                new Secret
                {
                    Id = 2,
                    Key = "Key2",
                    Value = "Value2"
                }
            };
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.GetAll()).Returns(secrets);
            var sut = new SecretsController();

            var result = sut.Index(vault.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SecretListAndNewViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(2, secrets.Count());
        }

        [Fact]
        public void AddSavesToVaultAndReturnsViewWithModel()
        {
            var secrets = new List<Secret>
            {
                new Secret
                {
                    Id = 1,
                    Key = "Key1",
                    Value = "Value1"
                },
                new Secret
                {
                    Id = 2,
                    Key = "Key2",
                    Value = "Value2"
                }
            };
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.Save(It.IsAny<Secret>())).Callback<Secret>((x) => secrets.Add(x));
            vault.Setup(x => x.GetAll()).Returns(secrets);
            var sut = new SecretsController();

            var vm = new NewSecretViewModel
            {
                Key = "Key3",
                Value = "Vaule3"
            };
            var result = sut.Index(vault.Object, vm);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(3, secrets.Count);
        }

        [Fact]
        public void AddReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            var secrets = new List<Secret>
            {
                new Secret
                {
                    Id = 1,
                    Key = "Key1",
                    Value = "Value1"
                },
                new Secret
                {
                    Id = 2,
                    Key = "Key2",
                    Value = "Value2"
                }
            };
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.GetAll()).Returns(secrets);

            var sut = new SecretsController();
            sut.ModelState.AddModelError("Key", "Required");

            var vm = new NewSecretViewModel();
            var result = sut.Index(vault.Object, vm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SecretListAndNewViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(2, secrets.Count());
            Assert.Single(viewResult.ViewData.ModelState);
        }
    }
}
