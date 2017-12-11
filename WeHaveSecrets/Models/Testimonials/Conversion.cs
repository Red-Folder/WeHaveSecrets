using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Models.Testimonials
{
    public static class Conversion
    {
        public static TestimonialViewModel AsViewModel(Testimonial domain)
        {
            return new TestimonialViewModel
            {
                Comment = domain.Comment
            };
        }
    }
}
