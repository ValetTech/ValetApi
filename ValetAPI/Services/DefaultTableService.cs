using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultTableService : ITableService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;

    public DefaultTableService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }

    public async Task<IEnumerable<Table>> GetTablesAsync()
    {
        if (_context.Tables == null) return null;
        return await _context.Tables.ProjectTo<Table>(_mappingConfiguration).ToArrayAsync();
    }

    public async Task<Table> GetTableAsync(int tableId)
    {
        if (_context.Tables == null) return null;

        var table = await _context.Tables
            .SingleOrDefaultAsync(v => v.Id == tableId);

        if (table == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Table>(table);
    }

    public async Task<int> CreateTableAsync(Table table)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Tables.AddAsync(mapper.Map<TableEntity>(table));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create table.");

        return entity.Entity.Id;
    }

    public async Task CreateTablesAsync(int noTables, int tableCapacity, int areaId)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var tables = new List<Table>();

        for (var i = 0; i < noTables; i++)
        {
            tables.Add(new Table
            {
                Type = "Square",
                AreaId = areaId,
                Capacity = tableCapacity
            });
        }

        await _context.Tables.AddRangeAsync(mapper.Map<TableEntity[]>(tables));


        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create tables.");
    }

    public async Task DeleteTableAsync(int tableId)
    {
        var table = await _context.Tables
            .SingleOrDefaultAsync(v => v.Id == tableId);

        if (table == null) return;

        _context.Tables.Remove(table);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTableAsync(Table table)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Tables.FirstOrDefaultAsync(v => v.Id == table.Id);
        if (entity == null) return;


        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<TableEntity>(table));
        
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not update table.");

    }
}