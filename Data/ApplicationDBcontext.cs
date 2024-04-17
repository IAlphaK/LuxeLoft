using LuxeLoft.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LuxeLoft.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Property> Properties { get; set; }
        public DbSet<Interested_Property> Interested_Properties { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User to Property relationship with cascade delete
            modelBuilder.Entity<User>()
                .HasMany(u => u.Properties)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Models.Property>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.OwnerID)
            .OnDelete(DeleteBehavior.Restrict); // This is to prevent cascading deletes, but you can also configure it to not cascade on add.


            // Configure the Property to Interested_Property relationship with cascade delete
            modelBuilder.Entity<Models.Property>()
                .HasMany(p => p.InterestedParties)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the Property to Feature relationship with cascade delete
            modelBuilder.Entity<Models.Property>()
                .HasMany(p => p.Features)
                .WithOne(f => f.Property)
                .HasForeignKey(f => f.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the Property to Image relationship with cascade delete
            modelBuilder.Entity<Models.Property>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interested_Property>()
                .HasOne(ip => ip.Seller)
                .WithMany(u => u.Interested_PropertiesAsSeller)
                .HasForeignKey(ip => ip.SellerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Interested_Property>()
                .HasOne(ip => ip.Buyer)
                .WithMany(u => u.Interested_PropertiesAsBuyer)
                .HasForeignKey(ip => ip.BuyerID)
                .OnDelete(DeleteBehavior.Restrict); // come here if deltetion is wrong coz Change to Restrict to avoid multiple cascade paths

        }

    }
}
