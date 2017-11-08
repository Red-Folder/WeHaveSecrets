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

namespace WeHaveSecrets.Tests.Unit.Controllers
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

        [Fact]
        public async void PublicShareWithoutVaultThrowsArguementExcaption()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                new SecretsController().PublicShare(null, 1)
            ));

            Assert.Contains("vault", ex.Message);
        }

        [Fact]
        public void PublicShareWithoutValidIdReturnsNotFound()
        {
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.Get(It.IsAny<int>())).Returns<Secret>(null);

            var sut = new SecretsController();

            var result = sut.PublicShare(vault.Object, 999);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void PublicShareWithValidIdReturnsShaePage()
        {
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.Get(It.IsAny<int>())).Returns(new Secret
            {
                Id = 1,
                Key = "MySecret",
                Value = "SecretValue"
            });

            var sut = new SecretsController();

            var result = sut.PublicShare(vault.Object, 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SecretViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("MySecret", model.Key);
            Assert.Equal("SecretValue", model.Value);
        }
    }
}
