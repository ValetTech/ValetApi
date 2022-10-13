using Microsoft.AspNetCore.Mvc;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Controllers.API;

/// <summary>
/// Customers endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    /// <summary>
    ///     Customer controllers constructor
    /// </summary>
    /// <param name="customerService"></param>
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <returns>All customers</returns>
    /// <response code="200">Returns all customers</response>
    /// <response code="404">If there are no customers</response>
    [HttpGet("", Name = nameof(GetCustomers))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _customerService.GetCustomersAsync();
        if (customers == null) return NotFound();
        return Ok(customers);
    }

    /// <summary>
    ///     Gets customer by Id.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <returns>An customer</returns>
    /// <response code="200">Returns an customer</response>
    /// <response code="404">If customer does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetCustomer))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    /// <summary>
    /// Create a new customer.
    /// </summary>
    /// <param name="customer">Customer Object</param>
    /// <returns>Newly created customer</returns>
    /// <response code="201">Successfully create</response>
    /// <response code="400">Failed to create</response>
    [HttpPost("", Name = nameof(CreateCustomer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        var id = await _customerService.CreateCustomerAsync(customer);
        var entity = await _customerService.GetCustomerAsync(id);
        return Created($"api/customer/{id}", entity);
    }


    /// <summary>
    /// Update customer
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <param name="customer">Customer Object</param>
    /// <returns></returns>
    /// <response code="204">Successfully updated</response>
    /// <response code="400">Failed to update</response>
    [HttpPut("{id:int}", Name = nameof(UpdateCustomer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();

        await _customerService.UpdateCustomerAsync(customer);

        return NoContent();
    }


    /// <summary>
    /// Delete customer
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <response code="201">Successfully deleted</response>
    /// <response code="400">Failed to delete</response>
    [HttpDelete("{id:int}", Name = nameof(DeleteCustomer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customerService.DeleteCustomerAsync(id);

        return NoContent();
    }

    // GET: api/customer/5/reservations
    /// <summary>
    /// Get all reservations for customer.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetReservationsForCustomer))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsForCustomer(int id)
    {
        var reservations = await _customerService.GetReservationsAsync(id);
        if (reservations == null) return NotFound();
        return Ok(reservations);
    }
}