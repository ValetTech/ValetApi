using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ValetAPI.Models;
using System.Reflection.Emit;

namespace ValetAPI.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<VenueEntity> Venues { get; set; }
    public DbSet<AreaEntity> Areas { get; set; }
    public DbSet<TableEntity> Tables { get; set; }
    public DbSet<SittingEntity> Sittings { get; set; }
    public DbSet<ReservationEntity> Reservations { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<AreaSittingEntity> AreaSittings { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Seed();

        // mb.Entity<VenueEntity>()
        //     .Property(b => b.Id)
        //     .HasColumnName("VenueId");
        //
        // mb.Entity<ReservationEntity>()
        //     .Property(b => b.Id)
        //     .HasColumnName("ReservationId");

        mb.Entity<AreaEntity>()
            .HasOne(k => k.Venue)
            .WithMany(c => c.Areas)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<AreaSittingEntity>()
            .HasKey(a => new { a.AreaId, a.SittingId });
        mb.Entity<AreaSittingEntity>()
            .HasOne(bc => bc.Area)
            .WithMany(b => b.AreaSittings)
            .HasForeignKey(bc => bc.AreaId);
        mb.Entity<AreaSittingEntity>()
            .HasOne(bc => bc.Sitting)
            .WithMany(c => c.AreaSittings)
            .HasForeignKey(bc => bc.SittingId);


        // mb.Entity<SittingEntity>()
        //     .HasOne(k => k.AreaEntity)
        //     .WithMany(c => c.Sessions)
        //     .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ReservationEntity>()
            .HasOne(k => k.Sitting)
            .WithMany(c => c.Reservations)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<ReservationEntity>()
            .HasOne(k => k.Customer)
            .WithMany(c => c.Reservations)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<TableEntity>()
            .HasOne(k => k.Area)
            .WithMany(c => c.Tables)
            .OnDelete(DeleteBehavior.Restrict);

    }

}