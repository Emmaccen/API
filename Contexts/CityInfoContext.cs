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

        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            :base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(new City()
                {
                    Id = 1,
                    Name = "Lagos",
                    Description = "City of the wise"
                },
                new City()
                {
                    Id = 2,
                    Name = "Bangalore",
                    Description = "A lone city"
                }); 
            
            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                 new PointOfInterest
                 {
                     Name = "some City",
                     Description = "City of the gods",
                     Id = 1,
                     CityId = 1,
                 },
                new PointOfInterest
                {
                    Name = "some City",
                    Description = "City of the gods",
                    Id = 2,
                    CityId = 1,
                },
                new PointOfInterest
                {
                    Name = "some City",
                    Description = "City of the gods",
                    Id = 3,
                    CityId = 2,
                });

            base.OnModelCreating(modelBuilder);
        }

        /* protected override object OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseSqlServer("connectionstring");
             base.OnConfiguring(optionsBuilder);
         }*/
    }
}
