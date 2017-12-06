using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets.Models
{
    public class Testimonial
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
