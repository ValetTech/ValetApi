using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ValetAPI.Data;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Controllers.API;

/// <summary>
/// Sittings controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SittingsController : ControllerBase
{
    private readonly ISittingService _sittingService;

    /// <summary>
    /// Sittings constructor
    /// </summary>
    /// <param name="sittingService"></param>
    public SittingsController(ISittingService sittingService)
    {
        _sittingService = sittingService;
    }

    /// <summary>
    /// Get all sittings.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetSittings()
    {
        var sittings = await _sittingService.GetSittingsAsync();
        if (sittings == null) return NotFound();
        return Ok(sittings);
    }

    
    /// <summary>
    /// Get sitting by Id.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = nameof(GetSitting))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Sitting>> GetSitting(int id)
    {
        var sitting = await _sittingService.GetSittingAsync(id);
        if (sitting == null) return NotFound();
        return Ok(sitting);
    }
    
    /// <summary>
    /// Create a new Sitting.
    /// </summary>
    /// <param name="sitting">Sitting Object</param>
    /// <returns>Newly created sitting</returns>
    [HttpPost("", Name = nameof(CreateSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Sitting>> CreateSitting(Sitting sitting)
    {
        var sittingId = await _sittingService.CreateSittingAsync(sitting);
        var sittingEntity = await _sittingService.GetSittingAsync(sittingId);
        return Created($"api/sitting/{sittingId}", sittingEntity);
    }

    /// <summary>
    /// Update sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="sitting">Sitting Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateSitting(int id, Sitting sitting)
    {
        if (id != sitting.Id) return BadRequest();

        await _sittingService.UpdateSittingAsync(sitting);

        return NoContent();
    }

    /// <summary>
    /// Delete a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}", Name = nameof(DeleteSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteSitting(int id)
    {
        await _sittingService.DeleteSittingAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Get all reservations for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetSittingReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetSittingReservations(int id)
    {
        var reservations = await _sittingService.GetReservationsAsync(id);
        if (reservations == null) return NotFound();
        return Ok(reservations);
    }

    /// <summary>
    /// Get all areas for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/areas", Name = nameof(GetSittingAreas))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Area>>> GetSittingAreas(int id)
    {
        var areas = await _sittingService.GetAreasAsync(id);
        if (areas == null) return NotFound();
        return Ok(areas);
    }

    /// <summary>
    /// Get all tables for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/tables", Name = nameof(GetSittingTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetSittingTables(int id, [FromQuery] SittingQueryParameters queryParameters)
    {
        var tables = await _sittingService.GetTablesAsync(id);

        var available = queryParameters.Available;
        var taken = queryParameters.Taken;
        if (available && !taken)
        {
            tables = await _sittingService.GetAvailableTablesAsync(id);
        }
        if (taken && !available)
        {
            tables = await _sittingService.GetTakenTablesAsync(id);
        }

        if (tables == null) return NotFound();
        
        tables = tables
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        return Ok(tables);
    }
}

