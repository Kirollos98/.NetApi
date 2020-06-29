using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class EditViewModel
    {
        [StringLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }
    }
}
