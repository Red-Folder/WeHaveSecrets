using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeHaveSecrets.Models;
using Microsoft.AspNetCore.Authorization;
using WeHaveSecrets.Models.Testimonials;
using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Services.Social;

namespace WeHaveSecrets.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddTestimonial([FromServices]UserManager<ApplicationUser> userManager,
                                            [FromServices]ISocialProof socialProof, 
                                            NewTestimonialViewModel testimonial)
        {
            if (userManager == null) throw new ArgumentNullException(nameof(userManager));
            if (socialProof == null) throw new ArgumentNullException(nameof(socialProof));
            if (testimonial == null) throw new ArgumentNullException(nameof(testimonial));

            if (ModelState.IsValid)
            {
                var domain = new Testimonial
                {
                    UserId = userManager.GetUserId(User),
                    Comment = testimonial.Comment,
                    Created = DateTime.Now
                };
                socialProof.Save(domain);
            }

            return RedirectToAction(actionName: nameof(Index));
        }
    }
}
