using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Bookings
    {

        public int Id { get; set; }

        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }



        [Display(Name = "Trip ID")]
        public int TripId { get; set; }


        [ForeignKey("TripId")]
        public virtual Trips Trips { get; set; }

        public int DemandeSeats { get; set; }

    }
}
