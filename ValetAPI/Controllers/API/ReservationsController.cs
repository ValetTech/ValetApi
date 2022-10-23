using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Infrastructure;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Controllers.API;

/// <summary>
/// Reservations controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    /// <summary>
    /// Reservations constructor
    /// </summary>
    /// <param name="reservationService"></param>
    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>
    /// Get all Reservations.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetAllReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations([FromQuery]ReservationQueryParameters queryParameters)
    {
        //YYYY-MM-DDTHH:MM:SS
        var reservations = await _reservationService.GetReservationsAsync();

        if (queryParameters.MinDate != null)
        {
            reservations = reservations.Where(r => r.DateTime >= queryParameters.MinDate.Value);
        }

        if (queryParameters.MaxDate != null)
        {
            reservations = reservations.Where(r => r.DateTime <= queryParameters.MaxDate.Value);
        }
        
        if (queryParameters.Date != null)
        {
            reservations = reservations.Where(r => r.DateTime.Date == queryParameters.Date.Value.Date);
        }

        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
            {
                reservations = reservations.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }
        }

        reservations = reservations
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        if (!await reservations.AnyAsync()) return NoContent();

        var details = reservations.Select(r => new
        {
            r.Id,
            r.CustomerId,
            r.SittingId,
            venueId = r.Sitting!.VenueId,
            r.DateTime,
            r.Duration,
            r.NoGuests,
            r.Source,
            r.Status,
            r.Notes
        });
        var response = new {reservations = (await reservations.ToArrayAsync())};

        return Ok(response);


    }

    /// <summary>
    /// Get reservation by Id.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = nameof(GetReservation))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Reservation>> GetReservation(int id)
    {
        var reservation = await _reservationService.GetReservationAsync(id);

        if (reservation == null) return NotFound();

        return Ok(reservation);

    }


    /// <summary>
    /// Create a new reservation.
    /// </summary>
    /// <param name="reservation">Reservation Id</param>
    /// <returns></returns>
    [HttpPost("", Name = nameof(CreateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
    {
        var reservationId = await _reservationService.CreateReservationAsync(reservation);

        var createdReservation = await _reservationService.GetReservationAsync(reservationId);



        return Ok(createdReservation);
    }

    /// <summary>
    /// Update reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="reservation">Reservation Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        if (id != reservation.Id ) return BadRequest();

        var res = await _reservationService.UpdateReservationAsync(reservation);

        if (res) return NoContent();
        if (!await ReservationExistsAsync(id))
            return NotFound();
        return BadRequest();
    }


    /// <summary>
    /// Delete a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}", Name = nameof(DeleteReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteReservation(int id)
    {

        var reservation = await _reservationService.DeleteReservationAsync(id);

        if (reservation) return NotFound();

        return NoContent();
    }


    /// <summary>
    /// Get tables for reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/tables", Name = nameof(GetReservationTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetReservationTables(int id)
    {
        var tables = await _reservationService.GetReservationTables(id);

        if (tables == null) return NotFound();

        return Ok(tables);

    }

    // PUT: api/Reservations/5/table?tableId
    /// <summary>
    /// Add table to a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableId">Table Id</param>
    /// <returns></returns>
    [HttpPost("{id:int}/table", Name = nameof(AddTableToReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AddTableToReservation(int id, [FromQuery]int tableId)
    {
        // if (id != reservation.Id) return BadRequest();

        var table = await _reservationService.AddTableToReservation(id, tableId);
        if (!table) return BadRequest();
        return NoContent();
    }

    // PUT: api/Reservations/5/table?tableIds
    /// <summary>
    /// Add multiple tables to a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableIds">Table Ids</param>
    /// <returns></returns>
    [HttpPost("{id:int}/tables", Name = nameof(AddTablesToReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AddTablesToReservation(int id, [FromQuery] int[] tableIds)
    {
        // if (id != reservation.Id) return BadRequest();

        var tables = await _reservationService.AddTablesToReservation(id, tableIds);
        if (!tables) return BadRequest();
        return NoContent();
    }

    // DELETE: api/Reservations/5/table?tableId
    /// <summary>
    /// Remove a table from a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableId">Table Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}/table", Name = nameof(RemoveTableFromReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> RemoveTableFromReservation(int id, [FromQuery] int tableId)
    {
        // if (id != reservation.Id) return BadRequest();

        var table = await _reservationService.RemoveTableFromReservation(id, tableId);
        if (!table) return BadRequest();
        return NoContent();
    }

    // DELETE: api/Reservations/5/table?tableIds
    /// <summary>
    /// Remove multiple tables from a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableIds">Table Ids</param>
    /// <returns></returns>
    [HttpDelete("{id:int}/tables", Name = nameof(RemoveTablesFromReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> RemoveTablesFromReservation(int id, [FromQuery] int[] tableIds)
    {
        // if (id != reservation.Id) return BadRequest();

        var tables = await _reservationService.RemoveTablesFromReservation(id, tableIds);
        if (!tables) return BadRequest();
        return NoContent();
    }

    private async Task<bool> ReservationExistsAsync(int id)
    {
        var reservation = await _reservationService.GetReservationAsync(id);

        return reservation != null;
    }
}