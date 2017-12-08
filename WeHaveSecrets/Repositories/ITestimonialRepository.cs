using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models.Testimonials;

namespace WeHaveSecrets.Repositories
{
    public interface ITestimonialRepository
    {
        void Save(Testimonial testimonial);
        List<Testimonial> GetAll();
    }
}
