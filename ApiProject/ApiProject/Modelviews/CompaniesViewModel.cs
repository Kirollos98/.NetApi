using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class CompaniesViewModel
    {
        public Companies Companies { get; set; }
        public IEnumerable<TransportationTypes> TransportationTypes { get; set; }
    }
}
