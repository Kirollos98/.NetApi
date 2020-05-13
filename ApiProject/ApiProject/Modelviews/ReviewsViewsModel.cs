using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class ReviewsViewsModel
    {

        public Reviews Reviews { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Bookings> Bookings { get; set; }
        public IEnumerable<ReviewReasons> ReviewReasons { get; set; }

    }
}
