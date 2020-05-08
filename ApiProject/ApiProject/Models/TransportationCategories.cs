using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class TransportationCategories
    {

        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [Required]
        public int MaxSeats { get; set; }
    }
}
