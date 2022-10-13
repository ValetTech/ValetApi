using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultVenueService : IVenueService
{
    private readonly ApplicationDbContext _context;
    private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;

    public DefaultVenueService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }

    public async Task<IEnumerable<Venue>> GetVenuesAsync()
    {
        if (_context.Venues == null) return null;
        return await _context.Venues.ProjectTo<Venue>(_mappingConfiguration).ToArrayAsync();
    }

    public async Task<Venue> GetVenueAsync(int venueId)
    {
        if (_context.Venues == null) return null;

        var venue = await _context.Venues
            // .ProjectTo<Venue>(_mappingConfiguration)
            .SingleOrDefaultAsync(v=>v.Id == venueId);

        if (venue == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();
        
        return mapper.Map<Venue>(venue);
    }

    public async Task<int> CreateVenueAsync(Venue venue)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Venues.AddAsync(mapper.Map<VenueEntity>(venue));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create venue.");

        return entity.Entity.Id;
    }

    public async Task DeleteVenueAsync(int venueId)
    {
        var venue = await _context.Venues
            .SingleOrDefaultAsync(v => v.Id == venueId);

        if (venue == null) return;

        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVenueAsync(Venue venue)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Venues.FirstOrDefaultAsync(v=>v.Id == venue.Id);
        if (entity == null) return;
        
        // vEntity = entity;

        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<VenueEntity>(venue));
        // var newVenue = _context.Venues.Update(entity);

        // var newVenue = _context.Update(entity);
        // _context.Entry(entity).State = EntityState.Modified;
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not update venue.");

    }

    public async Task<IEnumerable<Area>> GetAreasAsync(int venueId)
    {
        if (_context.Venues == null) return null;

        var venue = await _context.Venues
            .Include(v=>v.Areas)
            .SingleOrDefaultAsync(v => v.Id == venueId);

        if (venue?.Areas == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Area[]>(venue.Areas);
    }

    public async Task<IEnumerable<Sitting>> GetSittingsAsync(int venueId)
    {
        if (_context.Venues == null) return null;

        var venue = await _context.Venues
            .Include(v => v.Sittings)
            .SingleOrDefaultAsync(v => v.Id == venueId);

        if (venue?.Areas == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Sitting[]>(venue.Sittings);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsAsync(int venueId)
    {
        if (_context.Venues == null) return null;

        var venue = await _context.Venues
            .Include(v => v.Reservations)
            .SingleOrDefaultAsync(v => v.Id == venueId);

        if (venue?.Areas == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Reservation[]>(venue.Reservations);
    }

    public async Task<IEnumerable<T>> AddAsync<T>(int venueId, T source)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Area>> AddAreasAsync(int venueId, Area area)
    {
        throw new NotImplementedException();

        var mapper = _mappingConfiguration.CreateMapper();
        
        var venueEntity = await _context.Venues.Include(v=>v.Areas).FirstOrDefaultAsync(v=>v.Id == venueId);
        if (venueEntity == null) return null;
        
        var venue = mapper.Map<Venue>(venueEntity);
        venue.Areas.Add(area);
        return venue.Areas;
        throw new NotImplementedException();

    }

    // public async Task<IEnumerable<T>> AddAsync<T>(int venueId, T source)
    // {
    //     var mapper = _mappingConfiguration.CreateMapper();
    //
    //     var entity = _context.Set<typeof(T)>();
    //     if (entity == null) return null;
    //
    //     var venue = mapper.Map<T>(entity);
    //
    //     var created = await _context.SaveChangesAsync();
    //     if (created < 1) throw new InvalidOperationException($"Could not add {typeof(T)}.");
    //
    // }

    public async Task<IEnumerable<Sitting>> AddSittingsAsync(int venueId, Sitting sitting)
    {
        throw new NotImplementedException();

    }

    public async Task<IEnumerable<Reservation>> AddReservationsAsync(int venueId, Reservation reservation)
    {
        throw new NotImplementedException();
    }
}