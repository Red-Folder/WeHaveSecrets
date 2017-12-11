using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models.Testimonials;
using WeHaveSecrets.Services.Social;

namespace WeHaveSecrets.ViewComponents
{
    public class TestimonialsViewComponent: ViewComponent
    {
        private readonly ISocialProof _socialProof;

        public TestimonialsViewComponent(ISocialProof socialProof)
        {
            if (socialProof == null) throw new ArgumentNullException("socialProof");

            _socialProof = socialProof;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _socialProof.AllTestimonials().Select(x => (TestimonialViewModel)x).ToList();
            return View(model);
        }
    }
}
