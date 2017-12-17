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
using WeHaveSecrets.Models.Testimonials;
using WeHaveSecrets.Services.Social;
using WeHaveSecrets.Tests.Unit.TestUtils;

namespace WeHaveSecrets.Tests.Unit.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexReturnsViewWithModel()
        {
            var sut = new HomeController();

            var result = sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void AddTestimonialsWithNoUserManagerThrowsArguementException()
        {
            var sut = new HomeController();

            var socialProof = new Mock<ISocialProof>();
            var vm = new NewTestimonialViewModel();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                sut.AddTestimonial(null, socialProof.Object, vm)
            ));

            Assert.Contains("userManager", ex.Message);
        }

        [Fact]
        public async void AddTestimonialsWithNoSocialProofThrowsArguementException()
        {
            var sut = new HomeController();

            var userManager = IdentityMocks.UserManager();
            var vm = new NewTestimonialViewModel();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                sut.AddTestimonial(userManager.Object, null, vm)
            ));

            Assert.Contains("socialProof", ex.Message);
        }

        [Fact]
        public async void AddTestimonialsWithNoTestimonialThrowsArguementException()
        {
            var sut = new HomeController();

            var userManager = IdentityMocks.UserManager();
            var socialProof = new Mock<ISocialProof>();

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(
                sut.AddTestimonial(userManager.Object, socialProof.Object, null)
            ));

            Assert.Contains("testimonial", ex.Message);
        }

        [Fact]
        public void AddTestimonialSavesToSocialProof()
        {
            var sut = new HomeController();

            var userManager = IdentityMocks.UserManager();
            var socialProof = new Mock<ISocialProof>();
            var vm = new NewTestimonialViewModel();

            var result = sut.AddTestimonial(userManager.Object, socialProof.Object, vm);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            socialProof.Verify(x => x.Save(It.IsAny<Testimonial>()), Times.Once);
        }
    }
}
