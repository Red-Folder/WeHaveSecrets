using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeHaveSecrets.Models.Testimonials
{
    public class Testimonial
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
