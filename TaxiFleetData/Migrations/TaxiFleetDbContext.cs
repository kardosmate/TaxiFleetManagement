using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiFleetData.Entities;

namespace TaxiFleetData.Migrations
{
    public class TaxiFleetDbContext : DbContext
    {
        public TaxiFleetDbContext(DbContextOptions<TaxiFleetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    
    
}
