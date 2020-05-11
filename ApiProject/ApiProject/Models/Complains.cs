using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Complains
    {
        public int Id { get; set; }

        [MaxLength(1000)]
        public string Complain { get; set; }

        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Booking ID")]
        public int BookingId { get; set; }


        [ForeignKey("BookingId")]
        public virtual Bookings Bookings { get; set; }
    }
}
