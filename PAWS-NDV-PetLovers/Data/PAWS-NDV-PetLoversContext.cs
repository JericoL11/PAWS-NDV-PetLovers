﻿using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.Data
{
    public class PAWS_NDV_PetLoversContext:DbContext
    {
        public PAWS_NDV_PetLoversContext(DbContextOptions<PAWS_NDV_PetLoversContext> options) 
            :base (options)
        {
            
        }

        //dbset -  represents a collection of entities of a specific type

        //records
        public DbSet<Owner> Owners { get; set; } = default!;

        public DbSet<Pet> Pets { get; set; } = default!;

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<Product> Products { get; set; } = default!;

        public DbSet<Services> Services { get; set; } = default!;

        //appointments
        public DbSet<Appointment> Appointments { get; set; } = default!;

        public DbSet<AppointmentDetails> AppointmentDetails { get; set; } = default!;


        #region == fluent API == 
        /*
         * The Fluent API is a way of configuring your Entity
        Framework model classes using method chaining in the
        OnModelCreating method of your DbContext class.
        This approach provides more control over the database schema and
        relationships compared to using only data annotations.
        */

        //FIXING potential for a cycle or multiple cascade paths in your foreign key constraints,

        /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              base.OnModelCreating(modelBuilder);

              modelBuilder.Entity<AppointmentDetails>()
                  .HasOne(ad => ad.Pet)
                  .WithMany()
                  .HasForeignKey(ad => ad.petID)
                  .OnDelete(DeleteBehavior.Restrict);

              modelBuilder.Entity<AppointmentDetails>()
                  .HasOne(ad => ad.Appointment)
                  .WithMany()
                  .HasForeignKey(ad => ad.AppointId)
                  .OnDelete(DeleteBehavior.Restrict);

              modelBuilder.Entity<AppointmentDetails>()
                  .HasOne(ad => ad.services)
                  .WithMany()
                  .HasForeignKey(ad => ad.serviceID)
                  .OnDelete(DeleteBehavior.Restrict);
          }*/
        #endregion
    }
}
