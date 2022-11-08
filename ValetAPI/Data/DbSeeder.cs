using Microsoft.EntityFrameworkCore;
using ValetAPI.Models;

namespace ValetAPI.Data;

public static class DbSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        
        var venues = new List<VenueEntity>
        {
            new() {Id = 1, Name = "Saki", Address = "123 George St"}
        };

        modelBuilder.Entity<VenueEntity>().HasData(venues);
        
        

        var areas = new List<AreaEntity>();
            areas.Add(new AreaEntity
            {
                Id = 1, VenueId = 1, Name = "Main",
                Description = "Main Dining Area",
                Width = 10, Height = 10
            });
            areas.Add(new AreaEntity
            {
                Id = 2, VenueId = 1, Name = "Outside", 
                Description = "Outside Dining Area",
                Width = 10, Height = 10
            });
            areas.Add(new AreaEntity
            {
                Id = 3, VenueId = 1, Name = "Balcony",
                Description = "Balcony Dining Area",
                Width = 10, Height = 10
            });

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
            areaSittings.Add(new AreaSittingEntity {SittingId = area.Id * 10 + 1, AreaId = area.Id});
            sittings.Add(new SittingEntity
            {
                Id = area.Id * 10 + 2,
                VenueId = area.VenueId, Capacity = 50, Type = SittingType.Lunch,
                StartTime = new DateTime(2022, 12, 25, 13, 00, 00), EndTime = new DateTime(2022, 12, 25, 16, 00, 00)
            });
            areaSittings.Add(new AreaSittingEntity {SittingId = area.Id * 10 + 2, AreaId = area.Id});

            sittings.Add(new SittingEntity
            {
                Id = area.Id * 10 + 3,
                VenueId = area.VenueId, Capacity = 50, Type = SittingType.Dinner,
                StartTime = new DateTime(2022, 12, 25, 17, 30, 00), EndTime = new DateTime(2022, 12, 25, 23, 00, 00)
            });
            areaSittings.Add(new AreaSittingEntity {SittingId = area.Id * 10 + 3, AreaId = area.Id});
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

        var tables = new List<TableEntity>();

        for (var i = 1; i < 11; i++)
        {
            tables.Add(new TableEntity
            {
                Id = 10 + i,
                Type = $"M{i}",
                Capacity = i,
                AreaId = 1
            });
            tables.Add(new TableEntity
            {
                Id = 20 + i,
                Type = $"O{i}",
                Capacity = i,
                AreaId = 2
            });
            tables.Add(new TableEntity
            {
                Id = 30 + i,
                Type = $"B{i}",
                Capacity = i,
                AreaId = 3
            });
        }
        modelBuilder.Entity<TableEntity>().HasData(tables);
    }
}