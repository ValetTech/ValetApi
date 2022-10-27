using ValetAPI.Models;

namespace ValetAPI.Services;

public interface ITableService
{
    // GetTables
    // GetTable
    // PostTable
    // PutTable
    // DeleteTable

    // ********* CRUD *********

    // Get all Tables
    Task<IEnumerable<Table>> GetTablesAsync();

    // Get Table by Id
    Task<Table> GetTableAsync(int tableId);

    // Create Table
    Task<int> CreateTableAsync(Table table);

    // Create Tables
    Task CreateTablesAsync(Table[] tables);

    // Delete Table by Id
    Task DeleteTableAsync(int tableId);

    // Update Table 
    Task UpdateTableAsync(Table table);
}