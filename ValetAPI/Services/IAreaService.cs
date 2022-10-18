using ValetAPI.Models;

namespace ValetAPI.Services;

public interface IAreaService
{
    // GetSittingsByArea // :id/sittings
    // GetTablesByArea // :id/tables

    // Add table
    // Add sitting

    // ********* CRUD *********

    // Get all Areas
    IQueryable<Area> GetAreasAsync();
    // Get Area by Id
    Task<Area> GetAreaAsync(int areaId);
    // Create Area
    Task<int> CreateAreaAsync(Area area);
    // Delete Area by Id
    Task DeleteAreaAsync(int areaId);
    // Update Area 
    Task UpdateAreaAsync(Area area);

    // ********* GET *********

    // Get tables by area Id
    Task<IEnumerable<Table>> GetTablesAsync(int areaId);
    // Get sittings by area Id
    Task<IEnumerable<Sitting>> GetSittingsAsync(int areaId);

    // ********* ADD *********
    // Add table to area
}