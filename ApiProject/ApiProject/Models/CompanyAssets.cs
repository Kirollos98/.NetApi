using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class CompanyAssets
    {

        public int Id { get; set; }


        [Display(Name = "Company")]
        public int CompniesId { get; set; }


        [ForeignKey("CompniesId")]
        public virtual Companies Companies { get; set; }




        [Display(Name = "Transportation Category")]
        public int TransportationCategoriesId { get; set; }


        [ForeignKey("TransportationCategoriesId")]
        public virtual TransportationCategories TransportationCategories { get; set; }

    }

}

