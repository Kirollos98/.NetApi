using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Reviews
    {
        public int Id { get; set; }


        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        
        [Display(Name = "Booking ID")]
        public int BookingId { get; set; }


        [ForeignKey("BookingId")]
        public virtual Bookings Bookings { get; set; }

         [Display(Name = "Review Reason ID")]
        public int ReviewReasonId { get; set; }


        [ForeignKey("ReviewReasonId")]
        public virtual ReviewReasons ReviewReasons { get; set; }

        public float ReviewRate { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }




    }
}
