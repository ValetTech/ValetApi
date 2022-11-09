using ValetAPI.Models;

namespace ValetAPI.Services;

public interface IReservationService
{
    // GetReservation // By Id
    // GetReservations
    // PostReservation
    // PutReservation
    // DeleteReservation

    // Patch

    // Get tables

    // Add table
    // Patch - /api/reservation/:id/?tableId

    // Create Reservation : 
    // Customer, Sitting, DateTime, Duration, NoGuests, Source, Status, Notes?
    // VenueId = Sitting.VenueId
    // 
    //

    Task<IQueryable<Reservation>> GetReservationsAsync();

    Task<Reservation> GetReservationAsync(int reservationId);

    Task<int> CreateReservationAsync(Reservation reservation);
    Task UpdateReservationAsync(Reservation reservation);

    Task DeleteReservationAsync(int reservationId);
    Task<IEnumerable<Table>> GetReservationTables(int reservationId);

    Task AddTableToReservation(int reservationId, int tableId);
    Task<bool> AddTablesToReservation(int reservationId, int[] tableIds);
    Task<bool> RemoveTableFromReservation(int reservationId, int tableId);
    Task<bool> RemoveTablesFromReservation(int reservationId, int[] tableIds);

    // Task<IEnumerable<Reservation>> GetReservationsForUserIdAsync(
    //     int userId);
}