using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class ApplicationDb : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options):base(options)
        {
                
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<TransportationTypes> TransportationTypes { get; set; }
        public DbSet<Companies> Companies { get; set; }
    }
}
