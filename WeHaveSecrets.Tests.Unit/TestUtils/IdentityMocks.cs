using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WeHaveSecrets.Models;

namespace WeHaveSecrets.Tests.Unit.TestUtils
{
    public class IdentityMocks
    {
        public static Mock<UserManager<ApplicationUser>> UserManager(IUserStore<ApplicationUser> userStore = null)
        {
            userStore = userStore ?? new Mock<IUserStore<ApplicationUser>>().Object;
            return new Mock<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        }

        public static Mock<SignInManager<ApplicationUser>> SignInManager(UserManager<ApplicationUser> userManager = null,
                                                                 IHttpContextAccessor httpContextAccessor = null,
                                                                 IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory = null)
        {
            userManager = userManager ?? UserManager().Object;
            httpContextAccessor = httpContextAccessor ?? new Mock<IHttpContextAccessor>().Object;
            userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object;
            return new Mock<SignInManager<ApplicationUser>>(userManager, httpContextAccessor, userClaimsPrincipalFactory, null, null, null);
        }

        public static Mock<RoleManager<ApplicationRole>> RoleManager(IRoleStore<ApplicationRole> roleStore = null)
        {
            roleStore = roleStore ?? new Mock<IRoleStore<ApplicationRole>>().Object;
            return new Mock<RoleManager<ApplicationRole>>(roleStore, null, null, null, null);
        }

    }
}
