using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Models;
using ValetAPI.Models.QueryParameters;
using ValetAPI.Services;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Reservations controller v1
/// </summary>
[ApiVersion("1.0")]
[Route("api/reservations")]
[ApiController]
public class ReservationsV1Controller : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IConfigurationProvider _mappingConfiguration;

    /// <summary>
    ///     Reservations constructor
    /// </summary>
    /// <param name="reservationService"></param>
    public ReservationsV1Controller(IReservationService reservationService, IConfigurationProvider mappingConfiguration)
    {
        _reservationService = reservationService;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Get all Reservations.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetAllReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations(
        [FromQuery] ReservationQueryParameters queryParameters)
    {
        //YYYY-MM-DDTHH:MM:SS
        var reservations = await _reservationService.GetReservationsAsync();

        if (queryParameters.MinDate.HasValue)
            reservations = reservations.Where(r => r.DateTime >= queryParameters.MinDate.Value);

        if (queryParameters.MaxDate.HasValue)
            reservations = reservations.Where(r => r.DateTime <= queryParameters.MaxDate.Value);

        if (queryParameters.Date.HasValue)
        {
            var date = queryParameters.Date.Value;
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            reservations = reservations.Where(r => r.DateTime.Date == date);
        }

        if (queryParameters.Duration.HasValue)
            reservations = reservations.Where(r => r.Duration == queryParameters.Duration);

        if (queryParameters.Guests.HasValue)
            reservations = reservations.Where(r => r.NoGuests == queryParameters.Guests);
        
        if (queryParameters.Source.HasValue)
            reservations = reservations.Where(r => r.Source == queryParameters.Source);
        
        if (queryParameters.Status.HasValue)
            reservations = reservations.Where(r => r.Status == queryParameters.Status);
        
        
        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer != null && (
                a.Customer.FirstName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.LastName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.Email.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.Phone.ToLower().Contains(queryParameters.SearchTerm.ToLower()))
            );
        }
        
        if (!string.IsNullOrEmpty(queryParameters.Customer))
        {
            
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.Customer.ToLower()) || 
                a.Customer != null && (
                    a.Customer.FirstName.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.LastName.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.Email.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.Phone.ToLower().Contains(queryParameters.Customer.ToLower()))
            );
        }
        
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
            {
                reservations = reservations.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);
            }
        }

        reservations = reservations
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        var mapper = _mappingConfiguration.CreateMapper();
        var reservationsDto = reservations.Select(r => new {
            r.Id,
            r.CustomerId,
            Customer = mapper.Map<Models.DTO.Customer>(r.Customer),
            r.SittingId,
            Sitting = mapper.Map<Models.DTO.Sitting>(r.Sitting),
            r.AreaId,
            Area = mapper.Map<Models.DTO.Area>(r.Area),
            r.DateTime,
            r.Duration,
            r.NoGuests,
            r.Source,
            r.Status,
            r.Notes,
            r.Tables
        });

        return Ok(new {reservations = await reservationsDto.ToArrayAsync()});
    }

    /// <summary>
    ///     Get reservation by Id.
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
    ///     Create a new reservation.
    /// </summary>
    /// <param name="reservation">Reservation Id</param>
    /// <returns></returns>
    [HttpPost("", Name = nameof(CreateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
    {
        var id = await _reservationService.CreateReservationAsync(reservation);

        var entity = await _reservationService.GetReservationAsync(id);
        if (entity == null) return BadRequest();

        return Ok(entity);
    }

    /// <summary>
    ///     Update reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="reservation">Reservation Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        if (id != reservation.Id) return BadRequest();

        await _reservationService.UpdateReservationAsync(reservation);

        // if (!await ReservationExistsAsync(id))
        //     return NotFound();
        return NoContent();
    }


    /// <summary>
    ///     Delete a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}", Name = nameof(DeleteReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        await _reservationService.DeleteReservationAsync(id);


        return NoContent();
    }


    /// <summary>
    ///     Get tables for reservation.
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
    ///     Add table to a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableId">Table Id</param>
    /// <returns></returns>
    [HttpPost("{id:int}/table", Name = nameof(AddTableToReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AddTableToReservation(int id, [FromQuery] int tableId)
    {
        // if (id != reservation.Id) return BadRequest();

        var table = await _reservationService.AddTableToReservation(id, tableId);
        if (!table) return BadRequest();
        return NoContent();
    }

    // PUT: api/Reservations/5/table?tableIds
    /// <summary>
    ///     Add multiple tables to a reservation.
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
    ///     Remove a table from a reservation.
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
    ///     Remove multiple tables from a reservation.
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

/// <summary>
///     Reservations controller v2
/// </summary>
[Authorize]
[ApiVersion("2.0")]
[Route("api/reservations")]
[ApiController]
public class ReservationsV2Controller : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IConfigurationProvider _mappingConfiguration;

    /// <summary>
    ///     Reservations constructor
    /// </summary>
    /// <param name="reservationService"></param>
    public ReservationsV2Controller(IReservationService reservationService, IConfigurationProvider mappingConfiguration)
    {
        _reservationService = reservationService;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Get all Reservations.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetAllReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations(
        [FromQuery] ReservationQueryParameters queryParameters)
    {
        //YYYY-MM-DDTHH:MM:SS
        var reservations = await _reservationService.GetReservationsAsync();

        if (queryParameters.MinDate.HasValue)
            reservations = reservations.Where(r => r.DateTime >= queryParameters.MinDate.Value);

        if (queryParameters.MaxDate.HasValue)
            reservations = reservations.Where(r => r.DateTime <= queryParameters.MaxDate.Value);

        if (queryParameters.Date.HasValue)
        {
            var date = queryParameters.Date.Value;
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            reservations = reservations.Where(r => r.DateTime.Date == date);
        }

        if (queryParameters.Duration.HasValue)
            reservations = reservations.Where(r => r.Duration == queryParameters.Duration);
        
        if (queryParameters.Guests.HasValue)
            reservations = reservations.Where(r => r.NoGuests == queryParameters.Guests);
        
        if (queryParameters.Source.HasValue)
            reservations = reservations.Where(r => r.Source == queryParameters.Source);
        
        if (queryParameters.Status.HasValue)
            reservations = reservations.Where(r => r.Status == queryParameters.Status);
        
        if (queryParameters.Area.Length > 0)
        {
            reservations = reservations.Where(r => r.Area != null && queryParameters.Area.Contains(r.Area.Name));
        }
        
        if (queryParameters.Sitting.Length > 0)
        {
            reservations = reservations.Where(r => r.Sitting != null && queryParameters.Area.Contains(r.Sitting.Type.ToString()));
        }
        
        
        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer != null && (
                a.Customer.FirstName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.LastName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.Email.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Customer.Phone.ToLower().Contains(queryParameters.SearchTerm.ToLower()))
            );
        }
        
        if (!string.IsNullOrEmpty(queryParameters.Customer))
        {
            
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.Customer.ToLower()) || 
                a.Customer != null && (
                    a.Customer.FirstName.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.LastName.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.Email.ToLower().Contains(queryParameters.Customer.ToLower()) || 
                    a.Customer.Phone.ToLower().Contains(queryParameters.Customer.ToLower()))
            );
        }
        
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
            {
                reservations = reservations.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);
            }
        }

        reservations = reservations
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        var mapper = _mappingConfiguration.CreateMapper();
        var reservationsDto = reservations.Select(r => new {
            r.Id,
            r.CustomerId,
            Customer = mapper.Map<Models.DTO.Customer>(r.Customer),
            r.SittingId,
            Sitting = mapper.Map<Models.DTO.Sitting>(r.Sitting),
            r.AreaId,
            Area = mapper.Map<Models.DTO.Area>(r.Area),
            r.DateTime,
            r.Duration,
            r.NoGuests,
            r.Source,
            r.Status,
            r.Notes,
            r.Tables
        });

        return Ok(new {reservations = await reservationsDto.ToArrayAsync()});
    }

    /// <summary>
    ///     Get reservation by Id.
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
    ///     Create a new reservation.
    /// </summary>
    /// <param name="reservation">Reservation Id</param>
    /// <returns></returns>
    [HttpPost("", Name = nameof(CreateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
    {
        var id = await _reservationService.CreateReservationAsync(reservation);

        var entity = await _reservationService.GetReservationAsync(id);
        if (entity == null) return BadRequest();

        return Ok(entity);
    }

    /// <summary>
    ///     Update reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="reservation">Reservation Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        if (id != reservation.Id) return BadRequest();

        await _reservationService.UpdateReservationAsync(reservation);

        // if (!await ReservationExistsAsync(id))
        //     return NotFound();
        return NoContent();
    }


    /// <summary>
    ///     Delete a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}", Name = nameof(DeleteReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        await _reservationService.DeleteReservationAsync(id);


        return NoContent();
    }


    /// <summary>
    ///     Get tables for reservation.
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
    ///     Add table to a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableId">Table Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/table", Name = nameof(AddTableToReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AddTableToReservation(int id, [FromQuery] int tableId)
    {
        // if (id != reservation.Id) return BadRequest();

        var table = await _reservationService.AddTableToReservation(id, tableId);
        if (!table) return BadRequest();
        return NoContent();
    }

    // PUT: api/Reservations/5/table?tableIds
    /// <summary>
    ///     Add multiple tables to a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableIds">Table Ids</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
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
    ///     Remove a table from a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableId">Table Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
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
    ///     Remove multiple tables from a reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="tableIds">Table Ids</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
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