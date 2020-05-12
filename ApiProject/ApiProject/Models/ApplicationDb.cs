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
        public DbSet<TransportationCategories> TransportationCategories { get; set; }

        public DbSet<CompanyAssets> CompanyAssets { get; set; }
        public DbSet<Cities> Cities { get; set; }

        public DbSet<Trips> Trips { get; set; }

        public DbSet<ReviewReasons> ReviewReasons { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Complains> Complains { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

    }
}
