using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class ReviewReasons
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
    }
}
