using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models.Testimonials;
using WeHaveSecrets.Repositories;

namespace WeHaveSecrets.Services.Social
{
    public class SocialProof : ISocialProof
    {
        private readonly ITestimonialRepository _testimonialRepository;

        public SocialProof(ITestimonialRepository testimonialRepository)
        {
            if (testimonialRepository == null) throw new ArgumentNullException("testimonialRepository");

            _testimonialRepository = testimonialRepository;
        }

        public void Save(Testimonial testimonial)
        {
            _testimonialRepository.Save(testimonial);
        }
    }
}
