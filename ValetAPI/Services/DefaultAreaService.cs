using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultAreaService : IAreaService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;

    public DefaultAreaService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }


    public async Task<IEnumerable<Area>> GetAreasAsync()
    {
        if (_context.Areas == null) return null;
        return await _context.Areas.ProjectTo<Area>(_mappingConfiguration).ToArrayAsync();
    }

    public async Task<Area> GetAreaAsync(int areaId)
    {
        if (_context.Areas == null) return null;

        var area = await _context.Areas
            // .ProjectTo<Venue>(_mappingConfiguration)
            .SingleOrDefaultAsync(a => a.Id == areaId);

        if (area == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Area>(area);
    }

    public async Task<int> CreateAreaAsync(Area area)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        

        var entity = await _context.Areas.AddAsync(mapper.Map<AreaEntity>(area));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create area.");

        return entity.Entity.Id;
    }


    public async Task DeleteAreaAsync(int areaId)
    {
        var area = await _context.Areas
            .SingleOrDefaultAsync(a => a.Id == areaId);

        if (area == null) return;

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAreaAsync(Area area)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Areas.FirstOrDefaultAsync(v => v.Id == area.Id);
        if (entity == null) return;

        // vEntity = entity;

        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<AreaEntity>(area));
        // var newVenue = _context.Venues.Update(entity);

        // var newVenue = _context.Update(entity);
        // _context.Entry(entity).State = EntityState.Modified;
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not update area.");

    }

    public async Task<IEnumerable<Table>> GetTablesAsync(int areaId)
    {
        if (_context.Areas == null) return null;

        var area = await _context.Areas
            .Include(a => a.Tables)
            .SingleOrDefaultAsync(a => a.Id == areaId);

        if (area?.Tables == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Table[]>(area.Tables);
    }

    public async Task<IEnumerable<Sitting>> GetSittingsAsync(int areaId)
    {
        if (_context.Sittings == null) return null;

        var area = await _context.Areas
            .Include(a => a.AreaSittings)
            .ThenInclude(sa => sa.Sitting)
            .SingleOrDefaultAsync(a => a.Id == areaId);

        var sittings = area?.AreaSittings
            .Where(a => a.AreaId == areaId)
            .Select(a => a.Sitting);
        if (sittings == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Sitting[]>(sittings);
    }
}