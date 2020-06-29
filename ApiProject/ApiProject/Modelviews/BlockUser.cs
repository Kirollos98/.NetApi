using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Modelviews
{
    public class BlockUser
    {

        [StringLength(256)]
        public string UserName { get; set; }


        public bool IsLocked { get; set; }
    }
}
