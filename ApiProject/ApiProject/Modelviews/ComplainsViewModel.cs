using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class ComplainsViewModel
    {

        //public List<Complains> Complains { get; set; }
         public Complains Complains { get; set; }
        
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Bookings> Bookings { get; set; }
        

    }
}
