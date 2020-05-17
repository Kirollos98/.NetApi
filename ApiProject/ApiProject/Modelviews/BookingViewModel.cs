using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class BookingViewModel
    {
        // public Bookings Bookings { get; set; }
        public List<Bookings> Bookings { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Trips> Trips { get; set; }


        public IEnumerable<Cities> CitiesTO { get; set; }
        public IEnumerable<Cities> CityFrom { get; set; }
        public IEnumerable<CompanyAssets> CompanyAssets { get; set; }
        public IEnumerable<TransportationCategories> TransportationCategories { get; set; }
        public IEnumerable<Companies> Companies { get; set; }

    }
}
