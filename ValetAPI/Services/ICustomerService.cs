using ValetAPI.Models;

namespace ValetAPI.Services;

public interface ICustomerService
{
    // Get Customers
    // Get Customer
    // Post Customer
    // Put Customer
    // Delete Customer

    // Get Reservations
    Task<CustomerEntity> GetOrCreateCustomerEntityAsync(int? customerId = null, Customer? customer = null);
    Task<IEnumerable<CustomerEntity>> GetAllCustomerEntitiesAsync();

    // ********* CRUD *********

    // Get all Customers
    Task<IEnumerable<Customer>> GetCustomersAsync();

    // Get Customer by Id
    Task<Customer> GetCustomerAsync(int customerId);

    // Create Customer
    Task<int> CreateCustomerAsync(Customer customer);

    // Delete Customer by Id
    Task DeleteCustomerAsync(int customerId);

    // Update Customer 
    Task UpdateCustomerAsync(Customer customer);


    // Get reservations
    Task<IEnumerable<Reservation>> GetReservationsAsync(int customerId);
}