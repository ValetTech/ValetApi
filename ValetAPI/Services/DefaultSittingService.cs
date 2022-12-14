using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Filters;
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

        if (sitting.AreaIds.Any())
        {
            
            entity.Entity.AreaSittings
                .AddRange(sitting.AreaIds
                    .Select(id => new AreaSittingEntity {AreaId = int.Parse(id), SittingId = entity.Entity.Id.GetValueOrDefault() }));

        }

        
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create sitting.");

        if (!entity.Entity.Id.HasValue)
        {
            throw new HttpResponseException(400, "Something went wrong.");
        }
        
        return entity.Entity.Id.Value;
    }

    public async Task CreateSittingsAsync(RecurringSitting sitting)
    {
        // Title, Areas, Type, Capacity, Start, End/Duration?, Rrule, GroupId
        // Rrule = { freq, interval, byWeekDay, dtStart, until, count, 
        //  freq= day,week,month
        //  interval = int
        //  byWeekDay = [SU,MO,TU,WE,TH,FR,SA]
        //  until = dateTime
        //  count = int

        // Past not editable and remove groupId
        var sittings = new List<SittingEntity>();
        var freq = "day";
        var interval = 1;
        var until = new DateTime();
        var byWeekDay = new []{"SU","MO","TU","WE","TH","FR","SA"};
        var weekDays = new[] { 1, 1, 0, 1, 1, 1, 1 };
        var duration = 60;

        DateTime increment(DateTime i)
        {
            var dateTime = freq switch
            {
                "day" => i.AddDays(interval),
                "week" => i.AddDays(interval * 7),
                "month" => i.AddMonths(1),
                _ => i
            };
            return dateTime;
        }
        

        for (var i = sitting.StartTime; i <= until; i = increment(i))
        {
            Parallel.ForEach(weekDays, j => sittings.Add(new SittingEntity()));
            
            sittings.Add(new SittingEntity
            {
                // Title, Areas, Type, Capacity, Start, End/Duration?, Rrule, GroupId
                StartTime = i,
                EndTime = i.AddMinutes(duration),
                
            });
        }
        var mapper = _mappingConfiguration.CreateMapper();

        sitting.StartTime.ToUniversalTime();
        // sitting.EndTime.ToUniversalTime();
        
        

        var entity = await _context.Sittings.AddAsync(mapper.Map<SittingEntity>(sitting));
        
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create sitting.");

    }

    public async Task DeleteSittingAsync(int sittingId)
    {
        var sitting = await _context.Sittings
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting?.GroupId != null)
        {
            _context.Sittings.FromSqlInterpolated($"EXECUTE DeleteSitting @GroupId = {sitting.GroupId}, @UpdateGroup = 1 ");
            return;
        };
        if (sitting == null) return;
        _context.Sittings.FromSqlInterpolated($"EXECUTE DeleteSitting @Id = {sitting.Id}, @UpdateGroup = 0 ");

        // _context.Sittings.Remove(sitting);
        // await _context.SaveChangesAsync();
    }

    public async Task UpdateSittingAsync(Sitting sitting)
    {
        var mapper = _mappingConfiguration.CreateMapper();
        
        sitting.StartTime.ToUniversalTime();
        sitting.EndTime.ToUniversalTime();
        if (!(sitting.VenueId > 0)) sitting.VenueId = 1;
        
        var entity = await _context.Sittings.FirstOrDefaultAsync(v => v.Id == sitting.Id);
        if (entity == null) return;

        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<SittingEntity>(sitting));

        if (sitting.Areas?.Any() != null)
        {
            var areaSittingEntities = _context.AreaSittings
                .Where(sa=>sa.SittingId == sitting.Id)
                .AsSplitQuery();
            _context.AreaSittings
                .RemoveRange(areaSittingEntities);
            _context.AreaSittings
                .AddRange(sitting.Areas.Select(a=>new AreaSittingEntity
                {
                    AreaId = a.Id,
                    SittingId = sitting.Id.GetValueOrDefault()
                }));
        }
        
        if (sitting.AreaIds?.Any() != null)
        {
            var areaSittingEntities = _context.AreaSittings
                .Where(sa=>sa.SittingId == sitting.Id)
                .AsSplitQuery();
            _context.AreaSittings
                .RemoveRange(areaSittingEntities);
            _context.AreaSittings
                .AddRange(sitting.AreaIds.Select(id=>new AreaSittingEntity
                {
                    AreaId = int.Parse(id),
                    SittingId = sitting.Id.GetValueOrDefault()
                }));
        }
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new HttpResponseException(400, e.Message);
        }
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
            .ThenInclude(r => r.ReservationTables)
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
            .SelectMany(r => r.ReservationTables, (reservation, tableEntity) => tableEntity)
            .AsQueryable();

        var tables = allTables.Where(t => !takenTables.Select(x=>x.Table).Contains(t));

        return tables?.ProjectTo<Table>(_mappingConfiguration);
    }

    public async Task<IQueryable<Table>> GetTakenTablesAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.Reservations)
            .ThenInclude(r => r.ReservationTables)
            .SingleOrDefaultAsync(a => a.Id == sittingId);

        if (sitting == null) return null;

        var tables = sitting.Reservations
            .SelectMany(r => r.ReservationTables, (reservation, tableEntity) => tableEntity)
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

        sitting.AreaSittings.AddRange(areas.Select(id => new AreaSittingEntity {AreaId = id, SittingId = sitting.Id.GetValueOrDefault()}));

        if (sitting == null) return null;

        await _context.SaveChangesAsync();

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Sitting>(sitting);
    }
}