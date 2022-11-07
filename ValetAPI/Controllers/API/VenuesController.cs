using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Controllers.API;
/// <summary>
///     Venues controller v1
/// </summary>
[Authorize]
[ApiVersion("1.0")]
[Route("api/venues")]
[ApiController]
public class VenuesV1Controller : ControllerBase
{
    private readonly IVenueService _venueService;

    /// <summary>
    ///     Venues constructor
    /// </summary>
    /// <param name="venueService"></param>
    public VenuesV1Controller(IVenueService venueService)
    {
        _venueService = venueService;
    }


    /// <summary>
    ///     Get all venues.
    /// </summary>
    /// <returns>All venues</returns>
    /// <response code="200">Returns all venues</response>
    /// <response code="404">If there are no venues</response>
    [HttpGet("", Name = nameof(GetVenues))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Venue>>> GetVenues()
    {
        var venues = await _venueService.GetVenuesAsync();
        if (venues == null) return NotFound();
        return Ok(new { venues });
    }

    /// <summary>
    ///     Get venue by id.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns>Venue</returns>
    [HttpGet("{id:int}", Name = nameof(GetVenue))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Venue>> GetVenue(int id)
    {
        var venue = await _venueService.GetVenueAsync(id);
        if (venue == null) return NotFound();
        return Ok(venue);
    }

    /// <summary>
    ///     Update venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <param name="venue">Venue Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateVenue(int id, [FromBody] Venue venue)
    {
        if (id != venue.Id) return BadRequest();

        await _venueService.UpdateVenueAsync(venue);

        return NoContent();
    }

    /// <summary>
    ///     Create new venue.
    /// </summary>
    /// <param name="venue">Venue Object</param>
    /// <returns>Newly created venue</returns>
    [HttpPost("", Name = nameof(CreateVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Venue>> CreateVenue(Venue venue)
    {
        var venueId = await _venueService.CreateVenueAsync(venue);
        var venueEntity = await _venueService.GetVenueAsync(venueId);
        return Created($"api/venue/{venueId}", venueEntity);
    }

    /// <summary>
    ///     Delete venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}", Name = nameof(DeleteVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteVenue(int id)
    {
        await _venueService.DeleteVenueAsync(id);

        return NoContent();
    }

    /// <summary>
    ///     Gets areas for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/areas", Name = nameof(GetVenueAreas))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Area>>> GetVenueAreas(int id)
    {
        var areas = await _venueService.GetAreasAsync(id);
        // if (!areas.Any()) return NotFound();

        return Ok(areas);
    }

    /// <summary>
    ///     Get sittings for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/sittings", Name = nameof(GetVenueSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetVenueSittings(int id)
    {
        var sittings = await _venueService.GetSittingsAsync(id);
        // if (!sittings.Any()) return NotFound();

        return Ok(sittings);
    }

    /// <summary>
    ///     Get reservations for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetVenueReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetVenueReservations(int id)
    {
        var reservations = await _venueService.GetReservationsAsync(id);
        // if (!reservations.Any()) return NotFound();

        return Ok(reservations);
    }
}

/// <summary>
///     Venues controller v2
/// </summary>
[Authorize]
[ApiVersion("2.0")]
[Route("api/venues")]
[ApiController]
public class VenuesV2Controller : ControllerBase
{
    private readonly IVenueService _venueService;

    /// <summary>
    ///     Venues constructor
    /// </summary>
    /// <param name="venueService"></param>
    public VenuesV2Controller(IVenueService venueService)
    {
        _venueService = venueService;
    }


    /// <summary>
    ///     Get all venues.
    /// </summary>
    /// <returns>All venues</returns>
    /// <response code="200">Returns all venues</response>
    /// <response code="404">If there are no venues</response>
    [HttpGet("", Name = nameof(GetVenues))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Venue>>> GetVenues()
    {
        var venues = await _venueService.GetVenuesAsync();
        if (venues == null) return NotFound();
        return Ok(new { venues });
    }

    /// <summary>
    ///     Get venue by id.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns>Venue</returns>
    [HttpGet("{id:int}", Name = nameof(GetVenue))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Venue>> GetVenue(int id)
    {
        var venue = await _venueService.GetVenueAsync(id);
        if (venue == null) return NotFound();
        return Ok(venue);
    }

    /// <summary>
    ///     Update venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <param name="venue">Venue Object</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}", Name = nameof(UpdateVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateVenue(int id, [FromBody] Venue venue)
    {
        if (id != venue.Id) return BadRequest();

        await _venueService.UpdateVenueAsync(venue);

        return NoContent();
    }

    /// <summary>
    ///     Create new venue.
    /// </summary>
    /// <param name="venue">Venue Object</param>
    /// <returns>Newly created venue</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("", Name = nameof(CreateVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Venue>> CreateVenue(Venue venue)
    {
        var venueId = await _venueService.CreateVenueAsync(venue);
        var venueEntity = await _venueService.GetVenueAsync(venueId);
        return Created($"api/venue/{venueId}", venueEntity);
    }

    /// <summary>
    ///     Delete venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}", Name = nameof(DeleteVenue))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteVenue(int id)
    {
        await _venueService.DeleteVenueAsync(id);

        return NoContent();
    }

    /// <summary>
    ///     Gets areas for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/areas", Name = nameof(GetVenueAreas))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Area>>> GetVenueAreas(int id)
    {
        var areas = await _venueService.GetAreasAsync(id);
        // if (!areas.Any()) return NotFound();

        return Ok(areas);
    }

    /// <summary>
    ///     Get sittings for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/sittings", Name = nameof(GetVenueSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetVenueSittings(int id)
    {
        var sittings = await _venueService.GetSittingsAsync(id);
        // if (!sittings.Any()) return NotFound();

        return Ok(sittings);
    }

    /// <summary>
    ///     Get reservations for a venue.
    /// </summary>
    /// <param name="id">Venue Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetVenueReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetVenueReservations(int id)
    {
        var reservations = await _venueService.GetReservationsAsync(id);
        // if (!reservations.Any()) return NotFound();

        return Ok(reservations);
    }
}