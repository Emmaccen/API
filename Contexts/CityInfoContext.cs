using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> City { get; set; }

        public DbSet<PointOfInterest> pointOfInterests { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            :base()
        {
            Database.EnsureCreated();
        }

       /* protected override object OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("connectionstring");
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}
