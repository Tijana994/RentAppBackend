using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using RentApp.Models.Entities;

namespace RentApp.Persistance
{
    public class RADBContext : IdentityDbContext<RAIdentityUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }

        public DbSet<Branch> Branches { get; set; }

       
        public DbSet<Rate> Rates { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

       

        public DbSet<Pic> Pics { get; set; }

        public DbSet<TypeOfVehicle> TypeOfVehicles { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<BranchReservation> BranchReservations { get; set; }

        public RADBContext() : base("name=RADB")
        {
            
        }

        public static RADBContext Create()
        {
            return new RADBContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BranchReservation>()
            .HasRequired(c => c.Branch)
            .WithMany()
            .WillCascadeOnDelete(false);

            builder.Entity<BranchReservation>()
            .HasRequired(c => c.Reservation)
            .WithMany()
            .WillCascadeOnDelete(false);

           builder.Entity<Service>()
            .HasRequired(c => c.AppUser)
            .WithMany()
            .WillCascadeOnDelete(false);

           builder.Entity<Vehicle>()
           .HasRequired(c => c.Service)
           .WithMany()
           .WillCascadeOnDelete(true);

            builder.Entity<Vehicle>()
           .HasRequired(c => c.TypeOfVehicle)
           .WithMany()
           .WillCascadeOnDelete(true);

            /*builder.Entity<PriceList>()
              .HasRequired(c => c.Vehicle)
              .WithMany()
              .WillCascadeOnDelete(true);*/

            /*builder.Entity<Rate>()
              .HasRequired(c => c.Service)
              .WithMany()
              .WillCascadeOnDelete(false);

            builder.Entity<Rate>()
              .HasRequired(c => c.AppUser)
              .WithMany()
              .WillCascadeOnDelete(false);*/


        }
    }
}