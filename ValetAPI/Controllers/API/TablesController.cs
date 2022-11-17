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
///     Tables controller v1
/// </summary>
[Route("api/tables")]
[ApiVersion("1.0")]
[ApiController]
public class TablesV1Controller : ControllerBase
{
    private readonly ITableService _tableService;
    

    /// <summary>
    ///     Tables controller
    /// </summary>
    /// <param name="tableService"></param>
    public TablesV1Controller(ITableService tableService)
    {
        _tableService = tableService;
    }

    /// <summary>
    ///     Get all tables.
    /// </summary>
    /// <returns>All tables</returns>
    [HttpGet("", Name = nameof(GetTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetTables()
    {
        
             
        var tables = await _tableService.GetTablesAsync();
        if (tables == null) return NotFound();
        return Ok(new { tables });
    }

    /// <summary>
    ///     Get table by Id.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = nameof(GetTable))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Table>> GetTable(int id)
    {
        var table = await _tableService.GetTableAsync(id);
        if (table == null) return NotFound();
        return Ok(table);
    }

    /// <summary>
    ///     Update table.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <param name="table">Table Object</param>
    /// <returns></returns>
    [HttpPut("{id:int}", Name = nameof(UpdateTable))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateTable(int id, Table table)
    {
        if (id != table.Id) return BadRequest();

        await _tableService.UpdateTableAsync(table);

        return NoContent();
    }


    /// <summary>
    ///     Create new tables.
    /// </summary>
    /// <returns></returns>
    [HttpPost("", Name = nameof(CreateTables))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CreateTables([FromBody]Table[] tables)
    {
        await _tableService.CreateTablesAsync(tables);
        return NoContent();
    }

    /// <summary>
    ///     Delete table.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}", Name = nameof(DeleteTable))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteTable(int id)
    {
        await _tableService.DeleteTableAsync(id);

        return NoContent();
    }
}

/// <summary>
///     Tables controller v2
/// </summary>
[Authorize]
[ApiVersion("2.0")]
[Route("api/tables")]
[ApiController]
public class TablesV2Controller : ControllerBase
{
    private readonly ITableService _tableService;
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;

    /// <summary>
    ///     Tables controller
    /// </summary>
    /// <param name="tableService"></param>
    public TablesV2Controller(ITableService tableService, ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _tableService = tableService;
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Get all tables.
    /// </summary>
    /// <returns>All tables</returns>
    [HttpGet("", Name = nameof(GetTables))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Table>>> GetTables([FromQuery] TableQueryParameters queryParameters)
    {
        //-- Id, Type,Capacity, AreaId, SittingId, SittingType, Date, MinDate, MaxDate, IsPositioned, HasReservations, 
        var queryString = $"EXECUTE dbo.GetReservations ";
            if(!string.IsNullOrEmpty(queryParameters.MinDate))
            queryString += $"@MinDate = '{queryParameters.MinDate}', "; // MinDate
            if(!string.IsNullOrEmpty(queryParameters.MaxDate))
                queryString += $"@MaxDate = '{queryParameters.MaxDate}', "; // MaxDate
            if(!string.IsNullOrEmpty(queryParameters.Date))
                queryString += $"@Date = '{queryParameters.Date}', "; // Date
            if (queryParameters.Capacity.HasValue) 
                queryString += $"@Duration = {queryParameters.Capacity.Value}, "; // Capacity
            queryString += $"@Id = {queryParameters.Id ?? "null"}, "; // Id
            queryString += $"@AreaId = {queryParameters.AreaId ?? "null"}, "; // AreaId
            queryString += $"@SittingId = {queryParameters.SittingId ?? "null"}, "; // SittingId
            queryString += $"@SittingType = {queryParameters.SittingType ?? "null"}, "; // SittingType
            
             if (queryParameters.IsPositioned.HasValue) 
                 queryString += $"@IsPositioned = {queryParameters.IsPositioned.Value}, "; // IsPositioned
             if (queryParameters.HasReservations.HasValue) 
                 queryString += $"@HasReservations = {queryParameters.HasReservations.Value}, "; // HasReservations
             
            
             queryString += $"@Page = {queryParameters.Page}, "; // Page
             queryString += $"@Limit = {queryParameters.Size}, "; // Size
             if (typeof(Table).GetProperty(queryParameters.SortBy) != null)
                 queryString += $"@OrderBy = {queryParameters.SortBy}, "; // orderBy
             queryString += $"@OrderByAsc = {(queryParameters.SortOrder.ToLower() == "asc" ? 1 : 0)} "; // orderByAsc
         
             
             
             var tables =
                 _context.Tables
                     .FromSqlRaw<TableEntity>(queryString)
                     .AsNoTracking()
                     .AsEnumerable()
             ;
        return Ok(new { tables = tables });
    }

    /// <summary>
    ///     Get table by Id.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = nameof(GetTable))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Table>> GetTable(int id)
    {
        var table = await _tableService.GetTableAsync(id);
        if (table == null) return NotFound();
        return Ok(table);
    }

    /// <summary>
    ///     Update table.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <param name="table">Table Object</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}", Name = nameof(UpdateTable))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateTable(int id, Table table)
    {
        if (id != table.Id) return BadRequest();

        await _tableService.UpdateTableAsync(table);

        return NoContent();
    }

    /// <summary>
    ///     Create new table.
    /// </summary>
    /// <param name="table">Table Object</param>
    /// <returns></returns>
    // [HttpPost(Name = nameof(CreateTable))]
    // [ProducesResponseType(400)]
    // [ProducesResponseType(201)]
    // public async Task<ActionResult<Table>> CreateTable(Table table)
    // {
    //     var tableId = await _tableService.CreateTableAsync(table);
    //     var tableEntity = await _tableService.GetTableAsync(tableId);
    //     return Created($"api/table/{tableId}", tableEntity);
    // }

    /// <summary>
    ///     Create new tables.
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("", Name = nameof(CreateTables))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CreateTables([FromBody]Table[] tables)
    {
        await _tableService.CreateTablesAsync(tables);
        return NoContent();
    }

    /// <summary>
    ///     Delete table.
    /// </summary>
    /// <param name="id">Table Id</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}", Name = nameof(DeleteTable))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteTable(int id)
    {
        await _tableService.DeleteTableAsync(id);

        return NoContent();
    }
}