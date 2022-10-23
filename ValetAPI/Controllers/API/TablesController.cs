using Microsoft.AspNetCore.Mvc;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Tables controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TablesController : ControllerBase
{
    private readonly ITableService _tableService;

    /// <summary>
    ///     Tables controller
    /// </summary>
    /// <param name="tableService"></param>
    public TablesController(ITableService tableService)
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
        return Ok(tables);
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
    ///     Create new table.
    /// </summary>
    /// <param name="table">Table Object</param>
    /// <returns></returns>
    [HttpPost(Name = nameof(CreateTable))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Table>> CreateTable(Table table)
    {
        var tableId = await _tableService.CreateTableAsync(table);
        var tableEntity = await _tableService.GetTableAsync(tableId);
        return Created($"api/table/{tableId}", tableEntity);
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