using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models.Testimonials;

namespace WeHaveSecrets.Services.Social
{
    public interface ISocialProof
    {
        void Save(Testimonial testimonial);
    }
}
