using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class PromoCodes
    {
        public int Id { get; set; }

        public string PromoCode  { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Companies Companies { get; set; }

        public int Value { get; set; }

        public bool IsActivated { get; set; }
    }
}
