using MagicVilla_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.X86;

namespace MagicVilla_API.Data
{

    // Inheriting from IdentityDbContext<TUser> allows us to utilize ASP.NET Identity's
    // built-in tables and features. We're using our custom ApplicationUser class as the user model.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // When using IdentityDbContext, it's important to call the base method
            // OnModelCreating to ensure that the base configuration for Identity entities
            // is applied.

            // The base.OnModelCreating(modelBuilder) method includes essential configurations
            // for Identity entities such as roles, users, logins, and claims. Not calling this
            // method could lead to errors or missing configurations.

            // The error "The entity type 'IdentityUserLogin<string>' requires a primary key to be defined."
            // may occur if the base configuration is not applied. This error indicates that a
            // key is missing for an Identity entity. Calling base.OnModelCreating(modelBuilder)
            // allows the IdentityDbContext to set up the necessary primary keys and relationships
            // for Identity entities.

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
            new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                Occupancy = 4,
                Rate = 200,
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now,
            },
              new Villa
              {
                  Id = 2,
                  Name = "Premium Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                  Occupancy = 4,
                  Rate = 300,
                  Sqft = 550,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Villa
              {
                  Id = 3,
                  Name = "Luxury Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                  Occupancy = 4,
                  Rate = 400,
                  Sqft = 750,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Villa
              {
                  Id = 4,
                  Name = "Diamond Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                  Occupancy = 4,
                  Rate = 550,
                  Sqft = 900,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Villa
              {
                  Id = 5,
                  Name = "Diamond Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                  Occupancy = 4,
                  Rate = 600,
                  Sqft = 1100,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              }
                );
        }
    }
}
