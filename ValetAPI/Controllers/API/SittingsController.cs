using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using ValetAPI.Models.QueryParameters;
using ValetAPI.Services;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Sittings controller v1
/// </summary>
[ApiVersion("1.0")]
[Route("api/sittings")]
[ApiController]
public class SittingsV1Controller : ControllerBase
{
    private readonly IConfigurationProvider _mappingConfiguration;
    private readonly ISittingService _sittingService;

    /// <summary>
    ///     Sittings constructor
    /// </summary>
    /// <param name="sittingService"></param>
    public SittingsV1Controller(ISittingService sittingService, IConfigurationProvider mappingConfiguration)
    {
        _sittingService = sittingService;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Get all sittings.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetSittings(
        [FromQuery] SittingQueryParameters queryParameters)
    {
        var sittings = _sittingService.GetSittingsAsync();

        if (!string.IsNullOrEmpty(queryParameters.Date))
        {
            var date = DateTime.Parse(queryParameters.Date).Date;
            sittings = sittings.Where(s => date <= s.EndTime.Date && date >= s.StartTime.Date);
        }

        if (!string.IsNullOrEmpty(queryParameters.MinDateTime))
        {
            var dateTime = DateTime.Parse(queryParameters.MinDateTime);
            sittings = sittings.Where(s => dateTime <= s.EndTime && dateTime >= s.StartTime);
        }

        if (!string.IsNullOrEmpty(queryParameters.MaxDateTime))
        {
            var dateTime = DateTime.Parse(queryParameters.MaxDateTime);
            sittings = sittings.Where(s => dateTime <= s.EndTime && dateTime >= s.StartTime);
        }

        if (queryParameters.Capacity.HasValue)
        {
            var capacity = queryParameters.Capacity.Value;
            sittings = sittings.Where(s => s.Capacity == capacity);
        }

        if (queryParameters.Type.HasValue)
        {
            var type = queryParameters.Type.Value;
            sittings = sittings.Where(s => s.Type == type);
        }

        if (queryParameters.hasReservations.HasValue)
        {
            var hasReservations = queryParameters.hasReservations.Value;
            sittings = sittings.Where(s => s.Reservations.Any() == hasReservations);
        }

        if (queryParameters.hasAreas.HasValue)
        {
            var hasAreas = queryParameters.hasAreas.Value;
            sittings = sittings.Where(s => s.Areas.Any() == hasAreas);
        }

        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            sittings = sittings.Where(a => true
            );

        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            if (typeof(Sitting).GetProperty(queryParameters.SortBy) != null)
                sittings = sittings.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);

        sittings = sittings
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        var mapper = _mappingConfiguration.CreateMapper();
        var sittingsDto = sittings.Select(s => new
        {
            s.Id,
            s.Capacity,
            s.Type,
            s.StartTime,
            s.EndTime,
            Areas = mapper.Map<Models.DTO.Area[]>(s.Areas),
            Reservations = mapper.Map<Models.DTO.Reservation[]>(s.Reservations)
        });

        return Ok(new { sittings = await sittingsDto.ToArrayAsync() });
    }


    /// <summary>
    ///     Get sitting by Id.
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
    ///     Create a new Sitting.
    /// </summary>
    /// <param name="sitting">Sitting Object</param>
    /// <returns>Newly created sitting</returns>
    [HttpPost("", Name = nameof(CreateSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Sitting>> CreateSitting(Sitting sitting)
    {
        var id = await _sittingService.CreateSittingAsync(sitting);

        var entity = await _sittingService.GetSittingAsync(id);
        if (entity == null) return BadRequest();

        return CreatedAtAction("GetSitting", new { id = entity.Id }, entity);
    }

    /// <summary>
    ///     Update sitting.
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
    ///     Delete a sitting.
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
    ///     Get all reservations for a sitting.
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
    ///     Get all areas for a sitting.
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
    ///     Get all sitting types
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [HttpGet("types", Name = nameof(GetSittingTypes))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<SittingType>>> GetSittingTypes([FromQuery] DateTime? date)
    {
        var types = Enum.GetNames(typeof(SittingType));

        if (date != null)
        {
            var sittings = _sittingService.GetSittingsAsync();
            types = await sittings.Where(s => date >= s.StartTime.Date && date <= s.EndTime.Date)
                .Select(s => s.Type.ToString()).Distinct().ToArrayAsync();
        }

        if (types.Length == 0) return NoContent();

        return Ok(types);
    }

    /// <summary>
    ///     Get all tables for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/tables", Name = nameof(GetSittingTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetSittingTables(int id,
        [FromQuery] SittingTableQueryParameters queryParameters)
    {
        var tables = await _sittingService.GetTablesAsync(id);

        var available = queryParameters.Available;
        var taken = queryParameters.Taken;
        if (available && !taken) tables = await _sittingService.GetAvailableTablesAsync(id);
        if (taken && !available) tables = await _sittingService.GetTakenTablesAsync(id);

        if (tables == null) return NotFound();

        tables = tables
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        return Ok(tables);
    }


    /// <summary>
    ///     Add Areas to Sitting
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="areas">List of area Ids</param>
    /// <returns></returns>
    [HttpPost("{id:int}/areas", Name = nameof(AddAreasToSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Sitting>> AddAreasToSitting(int id, [FromBody] List<int> areas)
    {
        var sitting = await _sittingService.AddAreasToSitting(id, areas);
        return Ok(sitting);
    }
}

/// <summary>
///     Sittings controller v2
/// </summary>
[ApiVersion("2.0")]
[Route("api/sittings")]
[ApiController]
public class SittingsV2Controller : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;
    private readonly ISittingService _sittingService;

    /// <summary>
    ///     Sittings constructor
    /// </summary>
    /// <param name="sittingService"></param>
    public SittingsV2Controller(ISittingService sittingService, IConfigurationProvider mappingConfiguration,
        ApplicationDbContext context)
    {
        _sittingService = sittingService;
        _mappingConfiguration = mappingConfiguration;
        _context = context;
    }

    /// <summary>
    ///     Get all sittings.
    /// </summary>
    /// <returns></returns>
    [HttpGet("", Name = nameof(GetSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetSittings(
        [FromQuery] SittingQueryParameters queryParameters)
    {
        var queryString = "EXECUTE dbo.GetSittings ";
        if (queryParameters.Id.HasValue)
            queryString += $"@Id = {queryParameters.Id.Value}, "; // Id
        if (!string.IsNullOrEmpty(queryParameters.MinDateTime))
            queryString += $"@MinDate = '{queryParameters.MinDateTime}', "; // MinDate
        if (!string.IsNullOrEmpty(queryParameters.MaxDateTime))
            queryString += $"@MaxDate = '{queryParameters.MaxDateTime}', "; // MaxDate
        if (!string.IsNullOrEmpty(queryParameters.Date))
            queryString += $"@Date = '{queryParameters.Date}', "; // Date
        if (queryParameters.Capacity.HasValue)
            queryString += $"@Capacity = {queryParameters.Capacity.Value}, "; // Capacity
        if (queryParameters.hasAreas.HasValue)
            queryString += $"@HasAreas = {queryParameters.hasAreas.Value}, "; // HasAreas
        if (queryParameters.hasReservations.HasValue)
            queryString += $"@HasReservations = {queryParameters.hasReservations.Value}, "; // HasAreas
        if (!string.IsNullOrEmpty(queryParameters.Title))
            queryString += $"@Title = '{queryParameters.Title}', "; // Title
        

        if (queryParameters.Areas.Any())
            queryString += $"@Areas = '{string.Join(',', queryParameters.Areas)}', "; // Areas
        if (queryParameters.Types.Any())
            queryString += $"@Type = '{string.Join(',', queryParameters.Types)}', "; // Types
        else if (queryParameters.Type.HasValue)
            queryString += $"@Type = {queryParameters.Type.Value}, "; // Type 

        queryString += $"@Page = {queryParameters.Page}, "; // Page
        queryString += $"@Limit = {queryParameters.Size}, "; // Size
        if (typeof(Sitting).GetProperty(queryParameters.SortBy) != null)
            queryString += $"@OrderBy = {queryParameters.SortBy}, "; // orderBy
        queryString += $"@OrderByAsc = {(queryParameters.SortOrder.ToLower() == "asc" ? 1 : 0)} "; // orderByAsc


        var sittings =
                _context.Sittings
                    .FromSqlRaw(queryString)
                    .AsNoTracking()
                    .AsEnumerable()
            ;
        var areas = await _context.Areas
            .FromSqlInterpolated(
                $"SELECT * FROM Areas")
            .Include(a=>a.AreaSittings)
            .ThenInclude(sa=>sa.Sitting)
            .AsNoTracking()
            .ToListAsync();

        var reservations = await _context.Reservations
            .FromSqlInterpolated(
                $"SELECT * FROM Reservations")
            .AsNoTracking()
            .ToListAsync();


        var mapper = _mappingConfiguration.CreateMapper();
        var sittingsDto = sittings.Select(s => new
        {
            s.Id,
            s.Capacity,
            s.Title,
            s.Type,
            s.StartTime,
            s.EndTime,
            s.GroupId,
            Areas = mapper.Map<Models.DTO.Area[]>(areas.Where(a => a.AreaSittings.Any(sa=>sa.SittingId == s.Id))),
            Reservations = mapper.Map<Models.DTO.Reservation[]>(reservations.Where(r => r.SittingId == s.Id).ToArray())
        });

        return Ok(new { sittings = sittingsDto });
    }


    /// <summary>
    ///     Get sitting by Id.
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
    ///     Create a new Sitting.
    /// </summary>
    /// <param name="sitting">Sitting Object</param>
    /// <returns>Newly created sitting</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("", Name = nameof(CreateSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    
    public async Task<ActionResult<Sitting>> CreateSitting([FromBody]Sitting sitting)
    {
        var id = await _sittingService.CreateSittingAsync(sitting);

        var entity = await _sittingService.GetSittingAsync(id);
        if (entity == null) return BadRequest();

        return CreatedAtAction("GetSitting", new { id = entity.Id }, entity);
    }

    /// <summary>
    ///     Update sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="sitting">Sitting Object</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}", Name = nameof(UpdateSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateSitting(int id, Sitting sitting)
    {
        if (id != sitting.Id) return BadRequest();
        
        if (!(sitting.VenueId > 0)) sitting.VenueId = 1;

        sitting.StartTime.ToUniversalTime();
        sitting.EndTime.ToUniversalTime();

        var queryString = "EXECUTE dbo.UpdateSitting ";
            queryString += $"@Id = {sitting.Id}, "; // Id
            queryString += $"@Capacity = '{sitting.Capacity}', "; // Capacity
            queryString += $"@Type = '{sitting.Type}', "; // Type
            queryString += $"@Title = '{sitting.Title}', "; // Title
            queryString += $"@StartTime = '{sitting.StartTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture)}', "; // StartTime
            queryString += $"@EndTime = '{sitting.EndTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture)}', "; // EndTime
            queryString += (sitting?.GroupId != null) ? $"@GroupId = '{sitting.GroupId}', @UpdateGroup = 1 " : $"@GroupId = null, @UpdateGroup = 0 "; // GroupId
        

        var rows = _context.Database.ExecuteSqlRaw(queryString);

        if (sitting.Areas?.Any() != null)
        {
            var areaSittingEntities = _context.AreaSittings
                .Where(sa=>sa.SittingId == sitting.Id)
                .AsSplitQuery();
            _context.AreaSittings
                .RemoveRange(areaSittingEntities);
            _context.AreaSittings
                .AddRange(sitting.Areas.Select(a=>new AreaSittingEntity
                {
                    AreaId = a.Id,
                    SittingId = sitting.Id.GetValueOrDefault()
                }));
        }
        
        if (sitting.AreaIds?.Any() != null)
        {
            var areaSittingEntities = _context.AreaSittings
                .Where(sa=>sa.SittingId == sitting.Id)
                .AsSplitQuery();
            _context.AreaSittings
                .RemoveRange(areaSittingEntities);
            _context.AreaSittings
                .AddRange(sitting.AreaIds.Select(id=>new AreaSittingEntity
                {
                    AreaId = int.Parse(id),
                    SittingId = sitting.Id.GetValueOrDefault()
                }));
        }
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest();
        }

        // await _sittingService.UpdateSittingAsync(sitting);

        return NoContent();
    }

    /// <summary>
    ///     Delete a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}", Name = nameof(DeleteSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteSitting(int id)
    {
        var rows = _context.Database.ExecuteSqlInterpolated(
            $"EXECUTE dbo.DeleteSitting @Id = {id}, @UpdateGroup = 0 ");
        
        // await _sittingService.DeleteSittingAsync(id);
        return NoContent();
    }
    
    /// <summary>
    ///     Delete group sittings.
    /// </summary>
    /// <param name="id">Group Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}/group", Name = nameof(DeleteSittingGroup))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteSittingGroup(string id)
    {
        var rows = await _context.Database.ExecuteSqlRawAsync(
            $"EXECUTE dbo.DeleteSittingGroup @GroupId = '{id}'");
        
        // await _sittingService.DeleteSittingAsync(id);

        return NoContent();
    }

    /// <summary>
    ///     Get all reservations for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetSittingReservations))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetSittingReservations(int id)
    {
        var reservations = await _sittingService.GetReservationsAsync(id);
        if (reservations == null) return NotFound();
        return Ok(reservations);
    }

    /// <summary>
    ///     Get all areas for a sitting.
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
    ///     Get all sitting types
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [HttpGet("types", Name = nameof(GetSittingTypes))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<SittingType>>> GetSittingTypes([FromQuery] DateTime? date)
    {
        var types = Enum.GetNames(typeof(SittingType));

        if (date != null)
        {
            var sittings = _sittingService.GetSittingsAsync();
            types = await sittings.Where(s => date >= s.StartTime.Date && date <= s.EndTime.Date)
                .Select(s => s.Type.ToString()).Distinct().ToArrayAsync();
        }

        if (types.Length == 0) return NoContent();

        return Ok(types);
    }

    /// <summary>
    ///     Get all tables for a sitting.
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/tables", Name = nameof(GetSittingTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Table>>> GetSittingTables(int id,
        [FromQuery] SittingTableQueryParameters queryParameters)
    {
        var tables = await _sittingService.GetTablesAsync(id);

        var available = queryParameters.Available;
        var taken = queryParameters.Taken;
        if (available && !taken) tables = await _sittingService.GetAvailableTablesAsync(id);
        if (taken && !available) tables = await _sittingService.GetTakenTablesAsync(id);

        if (tables == null) return NotFound();

        tables = tables
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        return Ok(tables);
    }


    /// <summary>
    ///     Add Areas to Sitting
    /// </summary>
    /// <param name="id">Sitting Id</param>
    /// <param name="areas">List of area Ids</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/areas", Name = nameof(AddAreasToSitting))]
    [ProducesResponseType(400)]
    [ProducesResponseType(200)]
    [Authorize]
    public async Task<ActionResult<Sitting>> AddAreasToSitting(int id, [FromBody] List<int> areas)
    {
        var sitting = await _sittingService.AddAreasToSitting(id, areas);
        return Ok(sitting);
    }
}