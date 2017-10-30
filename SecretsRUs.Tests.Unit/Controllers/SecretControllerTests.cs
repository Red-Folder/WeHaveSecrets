using Microsoft.AspNetCore.Mvc;
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
                new SecretsController().Index(null, new SecretViewModel())
            ));

            Assert.Contains("vault", ex.Message);
        }



        [Fact]
        public void IndexReturnsViewWithModel()
        {
            var secrets = new List<Secret>
            {
                new Secret(),
                new Secret()
            };
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.GetAll()).Returns(secrets);
            var sut = new SecretsController();

            var result = sut.Index(vault.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SecretViewModel>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void AddSavesToVaultAndReturnsViewWithModel()
        {
            var secrets = new List<Secret>
            {
                new Secret(),
                new Secret()
            };
            var vault = new Mock<ISecretVault>();
            vault.Setup(x => x.Save(It.IsAny<Secret>())).Callback<Secret>((x) => secrets.Add(x));
            vault.Setup(x => x.GetAll()).Returns(secrets);
            var sut = new SecretsController();

            var vm = new SecretViewModel();
            var result = sut.Index(vault.Object, vm);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void AddReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            var vault = new Mock<ISecretVault>();
            var sut = new SecretsController();
            sut.ModelState.AddModelError("Key", "Required");

            var vm = new SecretViewModel();
            var result = sut.Index(vault.Object, vm);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}
