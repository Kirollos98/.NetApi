using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        //address ssn 
        /* Column(TypeName = "nvarchar(MAX)")]
         public string ssn { get; set; }*/
        [MaxLength(500)]
        public string ssn { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
    }
}
