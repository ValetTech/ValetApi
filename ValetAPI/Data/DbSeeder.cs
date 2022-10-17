using Microsoft.EntityFrameworkCore;
using ValetAPI.Models;

namespace ValetAPI.Data;

public static class DbSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var venues = new List<VenueEntity>
        {
            new() { Id = 1, Name = "Saki", Address = "123 George St" },
            // new() { Id = 2, Name = "Maccas", Address = "321 Frank St" },
            // new() { Id = 3, Name = "Red Rooster", Address = "999 Kent St" },
            // new() { Id = 4, Name = "Guzman Y Gomez", Address = "555 Parade Dr" }
        };

        modelBuilder.Entity<VenueEntity>().HasData(venues);

        var areas = new List<AreaEntity>();
        foreach (var venue in venues)
        {
            areas.Add(new AreaEntity
            {
                Id = venue.Id * 10 + 1, VenueId = venue.Id, Name = "Main Dining",
                Description = "Gorgeous Main Dining AreaEntity"
            });
            areas.Add(new AreaEntity
                { Id = venue.Id * 10 + 2, VenueId = venue.Id, Name = "Outside", Description = "Outside with a view" });
            areas.Add(new AreaEntity
            {
                Id = venue.Id * 10 + 3, VenueId = venue.Id, Name = "Upstairs",
                Description = "Upstairs away from the noise"
            });
        }

        modelBuilder.Entity<AreaEntity>().HasData(areas);

        var sittings = new List<SittingEntity>();
        var areaSittings = new List<AreaSittingEntity>();

        foreach (var area in areas)
        {
            sittings.Add(new SittingEntity
            {
                Id = area.Id * 10 + 1,
                VenueId = area.VenueId, Capacity = 50, Type = SittingType.Breakfast,
                StartTime = new DateTime(2022, 12, 25, 10, 30, 00), EndTime = new DateTime(2022, 12, 25, 12, 30, 00)
            });
            areaSittings.Add(new AreaSittingEntity { SittingId = area.Id * 10 + 1, AreaId = area.Id });
            sittings.Add(new SittingEntity
            {
                Id = area.Id * 10 + 2,
                VenueId = area.VenueId, Capacity = 50, Type = SittingType.Lunch,
                StartTime = new DateTime(2022, 12, 25, 13, 00, 00), EndTime = new DateTime(2022, 12, 25, 16, 00, 00)
            });
            areaSittings.Add(new AreaSittingEntity { SittingId = area.Id * 10 + 2, AreaId = area.Id });

            sittings.Add(new SittingEntity
            {
                Id = area.Id * 10 + 3,
                VenueId = area.VenueId, Capacity = 50, Type = SittingType.Dinner,
                StartTime = new DateTime(2022, 12, 25, 17, 30, 00), EndTime = new DateTime(2022, 12, 25, 23, 00, 00)
            });
            areaSittings.Add(new AreaSittingEntity { SittingId = area.Id * 10 + 3, AreaId = area.Id });
        }

        modelBuilder.Entity<SittingEntity>().HasData(sittings);
        modelBuilder.Entity<AreaSittingEntity>().HasData(areaSittings);


        var customers = new List<CustomerEntity>
        {
            new()
            {
                Id = 1, FirstName = "John", LastName = "Wick", Email = "john.wick@gmail.com", Phone = "0411111111"
            },
            new()
            {
                Id = 2, FirstName = "Keith", LastName = "Smith", Email = "keith.smith@gmail.com", Phone = "0411111112"
            },
            new()
            {
                Id = 3, FirstName = "Penny", LastName = "Wayne", Email = "penny.wayne@gmail.com", Phone = "0411111113"
            }
        };

        modelBuilder.Entity<CustomerEntity>().HasData(customers);
    }
}