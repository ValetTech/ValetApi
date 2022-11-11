using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using ValetAPI.Models.QueryParameters;
using ValetAPI.Services;
using Area = ValetAPI.Models.DTO.Area;
using Customer = ValetAPI.Models.DTO.Customer;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;
using Sitting = ValetAPI.Models.DTO.Sitting;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Reservations controller v1
/// </summary>
[ApiVersion("1.0")]
[Route("api/reservations")]
[ApiController]
public class ReservationsV1Controller : ControllerBase
{
    private readonly IConfigurationProvider _mappingConfiguration;
    private readonly IReservationService _reservationService;

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

        if (!string.IsNullOrEmpty(queryParameters.MinDate))
            reservations = reservations.Where(r => r.DateTime >= DateTime.Parse(queryParameters.MinDate));

        if (!string.IsNullOrEmpty(queryParameters.MaxDate))
            reservations = reservations.Where(r => r.DateTime <= DateTime.Parse(queryParameters.MaxDate));

        if (!string.IsNullOrEmpty(queryParameters.Date))
        {
            var date = DateTime.Parse(queryParameters.Date);
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
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) ||
                (a.Customer != null && (
                    a.Customer.FirstName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                    a.Customer.LastName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                    a.Customer.Email.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                    a.Customer.Phone.ToLower().Contains(queryParameters.SearchTerm.ToLower())))
            );

        if (!string.IsNullOrEmpty(queryParameters.Customer))
            reservations = reservations.Where(a =>
                a.Id.ToString().Contains(queryParameters.Customer.ToLower()) ||
                (a.Customer != null && (
                    a.Customer.FirstName.ToLower().Contains(queryParameters.Customer.ToLower()) ||
                    a.Customer.LastName.ToLower().Contains(queryParameters.Customer.ToLower()) ||
                    a.Customer.Email.ToLower().Contains(queryParameters.Customer.ToLower()) ||
                    a.Customer.Phone.ToLower().Contains(queryParameters.Customer.ToLower())))
            );

        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
                reservations = reservations.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);

        reservations = reservations
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        var mapper = _mappingConfiguration.CreateMapper();
        var reservationsDto = reservations.Select(r => new
        {
            r.Id,
            r.CustomerId,
            Customer = mapper.Map<Customer>(r.Customer),
            r.SittingId,
            Sitting = mapper.Map<Sitting>(r.Sitting),
            r.AreaId,
            Area = mapper.Map<Area>(r.Area),
            r.DateTime,
            r.Duration,
            r.NoGuests,
            r.Source,
            r.Status,
            r.Notes,
            r.Tables
        });

        return Ok(new { reservations = await reservationsDto.ToArrayAsync() });
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

        await _reservationService.AddTableToReservation(id, tableId);
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
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;
    private readonly IReservationService _reservationService;

    /// <summary>
    ///     Reservations constructor
    /// </summary>
    /// <param name="reservationService"></param>
    public ReservationsV2Controller(IReservationService reservationService, IConfigurationProvider mappingConfiguration,
        ApplicationDbContext context)
    {
        _reservationService = reservationService;
        _mappingConfiguration = mappingConfiguration;
        _context = context;
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

            var queryString = $"EXECUTE dbo.GetReservations ";
            if(!string.IsNullOrEmpty(queryParameters.MinDate))
            queryString += $"@MinDate = '{queryParameters.MinDate}', "; // MinDate
            if(!string.IsNullOrEmpty(queryParameters.MaxDate))
                queryString += $"@MaxDate = '{queryParameters.MaxDate}', "; // MaxDate
            if(!string.IsNullOrEmpty(queryParameters.Date))
                queryString += $"@Date = '{queryParameters.Date}', "; // Date
            if (queryParameters.Duration.HasValue) 
                queryString += $"@Duration = {queryParameters.Duration.Value}, "; // Duration
            if (queryParameters.Guests.HasValue) 
                queryString += $"@Guests = {queryParameters.Guests.Value}, "; // Guests
            queryString += $"@Id = {queryParameters.Id ?? "null"}, "; // Id
            queryString += $"@CustomerId = {queryParameters.CustomerId ?? "null"}, "; // CustomerId
            queryString += $"@SittingId = {queryParameters.SittingId ?? "null"}, "; // SittingId
            if (queryParameters.Source.HasValue) 
            queryString += $"@Source = {queryParameters.Source}, "; // Source
            if (queryParameters.Status.HasValue) 
                queryString += $"@Status = {queryParameters.Status}, "; // Status
            queryString += $"@Customer = {queryParameters.Customer ?? "null"}, "; // Customer
             if (queryParameters.hasTables.HasValue) 
                 queryString += $"@hasTables = {queryParameters.hasTables.Value}, "; // hasTables
             
             if(!string.IsNullOrEmpty(queryParameters.Areas?.Trim()))
                 queryString += $"@Areas = '{queryParameters.Areas}', "; // Areas
             if(!string.IsNullOrEmpty(queryParameters.Sittings?.Trim()))
                 queryString += $"@Sittings = '{queryParameters.Sittings}', "; // Sittings
            
             queryString += $"@Page = {queryParameters.Page}, "; // Page
             queryString += $"@Limit = {queryParameters.Size}, "; // Size
             if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
                 queryString += $"@OrderBy = {queryParameters.SortBy}, "; // orderBy
             queryString += $"@OrderByAsc = {(queryParameters.SortOrder.ToLower() == "asc" ? 1 : 0)} "; // orderByAsc
         
             var reservations =
                 _context.Reservations
                     .FromSqlRaw<ReservationEntity>(queryString)
                     // .Include(r=>r.Customer)
                     // .Include(r=>r.Area)
                     // .Include(r=>r.Sitting)
                     .AsNoTracking()
                     .AsEnumerable()
             ;

        //
        // if (queryParameters.Area.Length > 0)
        //     reservations = reservations.Where(r => r.Area != null && queryParameters.Area.Contains(r.Area.Name));
        //
        // if (queryParameters.Sitting.Length > 0)
        //     reservations = reservations.Where(r =>
        //         r.Sitting != null && queryParameters.Area.Contains(r.Sitting.Type.ToString()));


        // if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        //     reservations = reservations.Where(a =>
        //         a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) ||
        //         (a.Customer != null && (
        //             a.Customer.FirstName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
        //             a.Customer.LastName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
        //             a.Customer.Email.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
        //             a.Customer.Phone.ToLower().Contains(queryParameters.SearchTerm.ToLower())))
        //     );

        // if (!string.IsNullOrEmpty(queryParameters.Customer))
        //     reservations = reservations.Where(a =>
        //         a.Id.ToString().Contains(queryParameters.Customer.ToLower()) ||
        //         (a.Customer != null && (
        //             a.Customer.FirstName.ToLower().Contains(queryParameters.Customer.ToLower()) ||
        //             a.Customer.LastName.ToLower().Contains(queryParameters.Customer.ToLower()) ||
        //             a.Customer.Email.ToLower().Contains(queryParameters.Customer.ToLower()) ||
        //             a.Customer.Phone.ToLower().Contains(queryParameters.Customer.ToLower())))
        //     );

        // if (!string.IsNullOrEmpty(queryParameters.SortBy))
        //     if (typeof(Reservation).GetProperty(queryParameters.SortBy) != null)
        //         reservations = reservations.OrderByCustom(
        //             queryParameters.SortBy,
        //             queryParameters.SortOrder);
        //
        // reservations = reservations
        //     .Skip(queryParameters.Size * (queryParameters.Page - 1))
        //     .Take(queryParameters.Size);
        //
        // var tables = _context.Tables
        //     .FromSqlInterpolated($"SELECT * FROM Tables LEFT JOIN ReservationsTables RT on Tables.Id = RT.TableId WHERE rt.ReservationId is not null");
        
        var mapper = _mappingConfiguration.CreateMapper();
        
        var customers = await _context.Customers
            .FromSqlInterpolated(
                $"SELECT DISTINCT c.id, c.firstname, c.lastname, c.email, c.phone, c.isVip FROM Customers c LEFT JOIN Reservations r on c.Id = r.CustomerId WHERE r.Id is NOT null")
            .AsNoTracking()
            .ToListAsync();
        var sittings = await _context.Sittings
            .FromSqlInterpolated(
                $"SELECT DISTINCT s.Id, s.Capacity, s.Type, s.StartTime, s.EndTime, s.VenueId FROM Sittings s LEFT JOIN Reservations r on s.Id = r.SittingId WHERE r.Id is NOT null")
            .AsNoTracking()
            .ToListAsync();
        var areas = await _context.Areas
            .FromSqlInterpolated(
                $"SELECT * FROM Areas")
            .AsNoTracking()
            .ToListAsync();
        

        var tableRes = await _context.Tables
            .Select(t => new
            {
                t.Id,
                t.Type,
                t.Capacity,
                t.AreaId,
                t.xPosition,
                t.yPosition,
                ReservationId = t.ReservationTables.Select(rt=>rt.ReservationId)
            })
            .ToListAsync();

            
        
        var reservationsDto = reservations.Select((r) =>  new
        {
            r.Id,
            r.CustomerId,
            Customer = mapper.Map<Models.DTO.Customer>(customers.FirstOrDefault(c=>c.Id == r.CustomerId)),
            r.SittingId,
            Sitting = mapper.Map<Models.DTO.Sitting>(sittings.FirstOrDefault(c=>c.Id == r.SittingId)),
            r.AreaId,
            Area = mapper.Map<Models.DTO.Area>(areas.FirstOrDefault(c=>c.Id == r.AreaId)),
            r.DateTime,
            r.Duration,
            r.NoGuests,
            r.Source,
            r.Status,
            r.Notes,
            Tables = tableRes.Where(t => t.ReservationId.Contains(r.Id)).ToList()
        });
        
        
        // Parallel.ForEach(customers, customer=>
        //     customer.Reservations = reservations.Where(r=>r.CustomerId == customer.Id).ToList());

        

        return Ok(new { reservations = reservationsDto });
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
    ///     Patch reservation.
    /// </summary>
    /// <param name="id">Reservation Id</param>
    /// <param name="reservation">Reservation Patch Object</param>
    /// <returns></returns>
    [HttpPatch("{id:int}", Name = nameof(PatchReservation))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PatchReservation(int id, [FromBody] JsonPatchDocument<ReservationEntity> patchRes)
    {
        if (patchRes == null) return BadRequest(ModelState);
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r=>r.Id == id);
        if(reservation == null) return BadRequest();
        patchRes.ApplyTo(reservation, ModelState);
        await _context.SaveChangesAsync();

        return !ModelState.IsValid ? BadRequest(ModelState) : new ObjectResult(reservation);
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

        return Ok(new { tables });
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

        await _reservationService.AddTableToReservation(id, tableId);
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