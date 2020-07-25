using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class CodeActivated
    {
        public int CodeActivatedId { get; set; }


        [Display(Name = "Promo Code")]
        public int PromoCodeId { get; set; }

        [ForeignKey("PromoCodeId")]
        public virtual PromoCodes PromoCodes { get; set; }
        [StringLength(450)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime DateActivated { get; set; }

    }
}
