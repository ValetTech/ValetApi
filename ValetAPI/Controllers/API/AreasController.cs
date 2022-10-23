using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Models;
using ValetAPI.Models.QueryParameters;
using ValetAPI.Services;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Areas endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AreasController : ControllerBase
{
    private readonly IAreaService _areaService;
    private readonly ITableService _tablesService;
    private readonly IConfigurationProvider _mappingConfiguration;


    /// <summary>
    ///     Area controllers constructor
    /// </summary>
    /// <param name="areaService"></param>
    /// <param name="tablesService"></param>
    public AreasController(IAreaService areaService, ITableService tablesService, IConfigurationProvider mappingConfiguration)
    {
        _areaService = areaService;
        _tablesService = tablesService;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Gets all areas.
    /// </summary>
    /// <returns>All areas</returns>
    /// <response code="200">Returns all areas</response>
    [HttpGet("", Name = nameof(GetAreas))]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Area>>> GetAreas([FromQuery] AreaQueryParameters queryParameters)
    {
        var areas = _areaService.GetAreasAsync();

        if (queryParameters.Date.HasValue)
        {
            var date = queryParameters.Date.Value;
            areas = areas.Where(a =>
                a.Sittings.Any(s => date.Date <= s.EndTime && date.Date >= s.StartTime));
        }

        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            areas = areas.Where(a =>
                a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower()) || 
                a.Description.ToLower().Contains(queryParameters.SearchTerm.ToLower())
            );
        }

        if (!string.IsNullOrEmpty(queryParameters.Name))
        {
            areas = areas.Where(a => a.Name.ToLower().Contains(queryParameters.Name.ToLower()));
        }
        
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            if (typeof(Area).GetProperty(queryParameters.SortBy) != null)
            {
                areas = areas.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);
            }
        }
        
        areas = areas
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);

        var mapper = _mappingConfiguration.CreateMapper();
        var areasDto = areas.Select(a => new
        {
            a.Id,
            a.Name,
            a.Description,
            a.Tables,
            sittings = mapper.Map<Models.DTO.Sitting[]>(a.Sittings)
        });
        // Outta left join
        return Ok(new {areas = await areasDto.ToArrayAsync()});
    }

    /// <summary>
    ///     Gets area by Id.
    /// </summary>
    /// <param name="id">Area Id</param>
    /// <returns>An area</returns>
    /// <response code="200">Returns an area</response>
    /// <response code="404">If area does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetArea))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Area>> GetArea(int id)
    {
        var area = await _areaService.GetAreaAsync(id);
        if (area == null) return NotFound();
        return Ok(area);
    }

    /// <summary>
    ///     Create area.
    /// </summary>
    /// <param name="area">Area Object</param>
    /// <returns>Created area</returns>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Failed to create</response>
    [HttpPost(Name = nameof(CreateArea))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateArea([FromBody] Area area)
    {
        var id = await _areaService.CreateAreaAsync(area);

        if (area.NoTables.HasValue && area.TableCapacity.HasValue)
            await _tablesService.CreateTablesAsync(area.NoTables.Value, area.TableCapacity.Value, id);


        var entity = await _areaService.GetAreaAsync(id);
        if (entity == null) return BadRequest();
        return CreatedAtAction($"GetArea",new { id = entity.Id }, entity);
    }


    /// <summary>
    ///     Update area.
    /// </summary>
    /// <param name="id">Area Id</param>
    /// <param name="area">Area Object</param>
    /// <returns>No content</returns>
    /// <response code="204">Successful update</response>
    /// <response code="400">Unsuccessful update</response>
    [HttpPut("{id:int}", Name = nameof(UpdateArea))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateArea(int id, [FromBody] Area area)
    {
        if (id != area.Id) return BadRequest();

        await _areaService.UpdateAreaAsync(area);

        return NoContent();
    }


    /// <summary>
    ///     Delete area.
    /// </summary>
    /// <param name="id">Area Id</param>
    /// <returns>No content</returns>
    /// <response code="201">Successfully deleted</response>
    /// <response code="400">Failed to delete</response>
    [HttpDelete("{id:int}", Name = nameof(DeleteArea))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteArea(int id)
    {
        
        await _areaService.DeleteAreaAsync(id);

        return NoContent();
    }

    /// <summary>
    ///     Gets tables for an area.
    /// </summary>
    /// <param name="id">Area Id</param>
    /// <returns>Tables for area.</returns>
    /// <response code="200">Returns list of tables</response>
    /// <response code="404">Area does not exist or no tables</response>
    [HttpGet("{id:int}/tables", Name = nameof(GetAreaTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetAreaTables(int id)
    {
        var tables = await _areaService.GetTablesAsync(id);
        if (tables == null) return NotFound();
        return Ok(tables);
    }

    /// <summary>
    ///     Gets sittings for an area.
    /// </summary>
    /// <param name="id">Area Id</param>
    /// <returns>Sittings for area.</returns>
    /// <response code="200">Returns list of sittings</response>
    /// <response code="404">Area does not exist or no sittings</response>
    [HttpGet("{id:int}/sittings", Name = nameof(GetAreaSittings))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Sitting>>> GetAreaSittings(int id)
    {
        var sittings = await _areaService.GetSittingsAsync(id);
        if (sittings == null) return NotFound();
        return Ok(sittings);
    }
}