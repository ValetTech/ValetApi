using ValetAPI.Models;

namespace ValetAPI.Services;

public interface IVenueService
{
    // GetVenue // By Id
    // GetVenues
    // PostVenue
    // PutVenue
    // DeleteVenue

    // GetAreasByVenue // :id/areas
    // GetSittingsByVenue // :id/sittings
    // GetReservationsByVenue // :id/reservations

    // Add area
    // Add sitting
    // Add reservation

    // ********* CRUD *********

    // Get all venues
    Task<IEnumerable<Venue>> GetVenuesAsync();
    // Get venue by Id
    Task<Venue> GetVenueAsync(int venueId);
    // Create venue
    Task<int> CreateVenueAsync(Venue venue);
    // Delete Venue by Id
    Task DeleteVenueAsync(int venueId);
    // Update Venue 
    Task UpdateVenueAsync(Venue venue);

    // ********* GET *********

    // Get areas by venue Id
    Task<IEnumerable<Area>> GetAreasAsync(int venueId);
    // Get sittings by venue Id
    Task<IEnumerable<Sitting>> GetSittingsAsync(int venueId);
    // Get reservations by venue Id
    Task<IEnumerable<Reservation>> GetReservationsAsync(int venueId);

    // ********* ADD *********
    // Add areas
    Task<IEnumerable<T>> AddAsync<T>(int venueId, T source);
    Task<IEnumerable<Area>> AddAreasAsync(int venueId, Area area);
    // Get sittings by venue Id
    Task<IEnumerable<Sitting>> AddSittingsAsync(int venueId, Sitting sitting);
    // Get reservations by venue Id
    Task<IEnumerable<Reservation>> AddReservationsAsync(int venueId, Reservation reservation);
}