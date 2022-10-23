using ValetAPI.Models;

namespace ValetAPI.Services;

public interface ISittingService
{
    // GetSitting // By Id
    // GetSittings
    // PostSitting
    // PutSitting
    // DeleteSitting

    // Get Areas
    // Get Reservations
    Task<SittingEntity> GetSittingEntityAsync(int sittingId);
    Task<IEnumerable<SittingEntity>> GetAllSittingEntitiesAsync();

    // ********* CRUD *********

    // Get all Sittings
    IQueryable<Sitting> GetSittingsAsync();

    // Get Sitting by Id
    Task<Sitting> GetSittingAsync(int sittingId);

    // Create Sitting
    Task<int> CreateSittingAsync(Sitting sitting);

    // Delete Sitting by Id
    Task DeleteSittingAsync(int sittingId);

    // Update Sitting 
    Task UpdateSittingAsync(Sitting sitting);

    // ********* GET *********

    // Get reservations by sitting Id
    Task<IEnumerable<Reservation>> GetReservationsAsync(int sittingId);

    // Get areas by sitting Id
    Task<IEnumerable<Area>> GetAreasAsync(int sittingId);
    Task<IQueryable<Table>> GetTablesAsync(int sittingId);
    Task<IQueryable<Table>> GetAvailableTablesAsync(int sittingId);
    Task<IQueryable<Table>> GetTakenTablesAsync(int sittingId);
    Task<Sitting> AddAreasToSitting(int sittingId, IEnumerable<int> areas);
}