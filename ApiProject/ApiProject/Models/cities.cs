﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Cities
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


       // public virtual ICollection<Trips> CityFromTrips { get; set; }
        //public virtual ICollection<Trips> CityToTrips { get; set; }
    }
}
