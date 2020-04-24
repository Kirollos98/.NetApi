using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Companies
    {
        public int Id { get; set; }
        public String Name { get; set; }
        [Display(Name = "Transportation Type")]
        public int TransportationTypeId { get; set; }

        [Display(Name = "Transportation Type")]
        [ForeignKey("TransportationTypeId")]
        public virtual TransportationTypes TransportationTypes { get; set; }
    }
}
