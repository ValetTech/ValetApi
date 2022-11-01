using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ValetAPI.Models;

namespace ValetAPI.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<VenueEntity> Venues { get; set; }
    public DbSet<AreaEntity> Areas { get; set; }
    public DbSet<TableEntity> Tables { get; set; }
    public DbSet<SittingEntity> Sittings { get; set; }
    public DbSet<ReservationEntity> Reservations { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<AreaSittingEntity> AreaSittings { get; set; }
    public DbSet<SittingTypeEntity> SittingTypes { get; set; }
    public DbSet<StateEntity> States { get; set; }
    public DbSet<SourceEntity> Sources { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in mb.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
        
        // mb.Property(x => x.StartTime).HasConversion((x) => x.ToUniversalTime(), (x) => DateTime.SpecifyKind(x, DateTimeKind.Utc) );

        // mb.Entity<SittingEntity>()
        //     .Property(e => e.StartTime)
        //     .HasConversion(
        //         v => v,
        //         v => new DateTime(v.Ticks, DateTimeKind.Utc));
        
        mb.Seed();


        mb.Entity<SittingEntity>().Property(s => s.Type).HasConversion<string>().IsRequired();
        mb.Entity<ReservationEntity>().Property(r => r.Source).HasConversion<string>().IsRequired();
        mb.Entity<ReservationEntity>().Property(r => r.Status).HasConversion<string>().IsRequired();


        mb
            .Entity<SittingTypeEntity>().HasData(
                Enum.GetValues(typeof(SittingType))
                    .Cast<SittingType>()
                    .Select(e => new SittingTypeEntity
                    {
                        Id = e,
                        Type = e.ToString()
                    })
            );

        mb
            .Entity<StateEntity>().HasData(
                Enum.GetValues(typeof(State))
                    .Cast<State>()
                    .Select(e => new StateEntity
                    {
                        Id = e,
                        State = e.ToString()
                    })
            );

        mb
            .Entity<SourceEntity>().HasData(
                Enum.GetValues(typeof(Source))
                    .Cast<Source>()
                    .Select(e => new SourceEntity
                    {
                        Id = e,
                        Source = e.ToString()
                    })
            );

        // Enum.GetValues(typeof(SittingType)).Cast<int>().Select(p => new { id = p,name=((SittingType)p).ToString() });
        // mb.Entity<SittingTypeEntity>().HasData(Enum.GetNames(typeof(SittingType)));
        // Enum
        // mb.Entity<StateEntity>().HasData(Enum.GetNames(typeof(State)));
        // mb.Entity<SourceEntity>().HasData(Enum.GetNames(typeof(Source)));
        // mb.Entity<VenueEntity>()
        //     .Property(b => b.Id)
        //     .HasColumnName("VenueId");
        //
        // mb.Entity<ReservationEntity>()
        //     .Property(b => b.Id)
        //     .HasColumnName("ReservationId");
        
        // mb.Entity<ReservationEntity>()
        //     .HasOne(k => k.AreaId)
        //     .WithMany(c => c.Res)
        //     .OnDelete(DeleteBehavior.Restrict);

        // mb.Entity<ReservationEntity>()
        //     .HasOne(k => k.Venue)
        //     .WithMany(k => k.Reservations)
        //     .OnDelete(DeleteBehavior.Restrict);
        //
        // mb.Entity<ReservationEntity>()
        //     .HasOne(k => k.Area)
        //     .WithMany(k => k.Reservations)
        //     .OnDelete(DeleteBehavior.Restrict);
        
        mb.Entity<AreaEntity>()
            .HasOne(k => k.Venue)
            .WithMany(c => c.Areas)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<AreaSittingEntity>()
            .HasKey(a => new {a.AreaId, a.SittingId});
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