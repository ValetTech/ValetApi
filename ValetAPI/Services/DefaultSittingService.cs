using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultSittingService : ISittingService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;

    public DefaultSittingService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }

    public async Task<IEnumerable<SittingEntity>> GetAllSittingEntitiesAsync()
    {
        if (_context.Sittings == null) return null;
        return await _context.Sittings.AsSplitQuery().ToArrayAsync();
    }

    public IQueryable<Sitting> GetSittingsAsync()
    {
        if (_context.Sittings == null) return null;
        return _context.Sittings
            .Include(s => s.Reservations)
            .Include(s => s.AreaSittings)
            .ThenInclude(sa => sa.Area)
            .AsSplitQuery()
            .ProjectTo<Sitting>(_mappingConfiguration).AsQueryable();
    }

    public async Task<Sitting> GetSittingAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.AreaSittings)
            .ThenInclude(s => s.Area)
            .Include(s => s.Reservations)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Sitting>(sitting);
    }

    public async Task<SittingEntity> GetSittingEntityAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.AreaSittings)
            .ThenInclude(s => s.Area)
            .Include(s => s.Reservations)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return sitting;
    }

    public async Task<int> CreateSittingAsync(Sitting sitting)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        sitting.StartTime.ToUniversalTime();
        sitting.EndTime.ToUniversalTime();

        var entity = await _context.Sittings.AddAsync(mapper.Map<SittingEntity>(sitting));
        
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create sitting.");

        return entity.Entity.Id;
    }

    public async Task DeleteSittingAsync(int sittingId)
    {
        var sitting = await _context.Sittings
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting == null) return;

        _context.Sittings.Remove(sitting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSittingAsync(Sitting sitting)
    {
        var mapper = _mappingConfiguration.CreateMapper();
        
        sitting.StartTime.ToUniversalTime();
        sitting.EndTime.ToUniversalTime();

        var entity = await _context.Sittings.FirstOrDefaultAsync(v => v.Id == sitting.Id);
        if (entity == null) return;

        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<SittingEntity>(sitting));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not update sitting.");
    }

    public async Task<IEnumerable<Reservation>> GetReservationsAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(a => a.Reservations)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting?.Reservations == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Reservation[]>(sitting.Reservations);
    }

    public async Task<IEnumerable<Area>> GetAreasAsync(int sittingId)
    {
        if (_context.Areas == null) return null;

        var sitting = await _context.Sittings
            .Include(a => a.AreaSittings)
            .ThenInclude(sa => sa.Area)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        var areas = sitting?.AreaSittings
            .Where(a => a.SittingId == sittingId)
            .Select(a => a.Area);
        if (areas == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Area[]>(areas);
    }

    public async Task<IQueryable<Table>> GetTablesAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.AreaSittings)
            .ThenInclude(a => a.Area)
            .ThenInclude(a => a.Tables)
            .SingleOrDefaultAsync(a => a.Id == sittingId);


        if (sitting == null) return null;

        var tables = sitting!.AreaSittings
            .Select(sa => sa.Area)
            // .Select(a => a.Tables)
            .SelectMany(a => a.Tables)
            .AsQueryable();

        return tables.ProjectTo<Table>(_mappingConfiguration);
    }

    public async Task<IQueryable<Table>> GetAvailableTablesAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.Reservations)
            .ThenInclude(r => r.Tables)
            .Include(s => s.AreaSittings)
            .ThenInclude(a => a.Area)
            .ThenInclude(a => a.Tables)
            .SingleOrDefaultAsync(a => a.Id == sittingId);
        ;

        if (sitting == null) return null;

        var allTables = sitting!.AreaSittings
            .Select(sa => sa.Area)
            // .Select(a => a.Tables)
            .SelectMany(a => a.Tables)
            .AsQueryable();

        var takenTables = sitting.Reservations
            .SelectMany(r => r.Tables, (reservation, tableEntity) => tableEntity)
            .AsQueryable();

        var tables = allTables.Where(t => !takenTables.Contains(t));

        return tables?.ProjectTo<Table>(_mappingConfiguration);
    }

    public async Task<IQueryable<Table>> GetTakenTablesAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.Reservations)
            .ThenInclude(r => r.Tables)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting == null) return null;

        var tables = sitting.Reservations
            .SelectMany(r => r.Tables, (reservation, tableEntity) => tableEntity)
            .AsQueryable();

        return tables?.ProjectTo<Table>(_mappingConfiguration);
    }

    public async Task<Sitting> AddAreasToSitting(int sittingId, IEnumerable<int> areas)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.AreaSittings)
            .ThenInclude(sa => sa.Area)
            .Include(s => s.Reservations)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        sitting.AreaSittings.AddRange(areas.Select(id => new AreaSittingEntity {AreaId = id, SittingId = sitting.Id}));

        if (sitting == null) return null;

        await _context.SaveChangesAsync();

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Sitting>(sitting);
    }
}