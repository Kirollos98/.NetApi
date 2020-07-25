using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class PromoCodeViewModel
    {
        //public string PromoCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Value { get; set; }
        public int CompanyId { get; set; }

        
       

       

    }
}
