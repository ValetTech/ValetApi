using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ValetAPI.Data;
using ValetAPI.Models;
using ValetAPI.Models.QueryParameters;
using ValetAPI.Services;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Customers endpoint v1
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/customers")]
[Produces("application/json")]
public class CustomersV1Controller : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IConfigurationProvider _mappingConfiguration;


    /// <summary>
    ///     Customer controllers constructor
    /// </summary>
    /// <param name="customerService"></param>
    public CustomersV1Controller(ICustomerService customerService, IConfigurationProvider mappingConfiguration)
    {
        _customerService = customerService;
        _mappingConfiguration = mappingConfiguration;
    }

    /// <summary>
    ///     Gets all customers.
    /// </summary>
    /// <returns>All customers</returns>
    /// <response code="200">Returns all customers</response>
    /// <response code="404">If there are no customers</response>
    [HttpGet("", Name = nameof(GetCustomers))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(
        [FromQuery] CustomerQueryParameters queryParameters)
    {
        var customers = await _customerService.GetCustomersAsync();

        if (queryParameters.isVip.HasValue)
            customers = customers.Where(c =>
                c.IsVip == queryParameters.isVip.Value);

        if (queryParameters.hasReservations.HasValue)
            customers = customers.Where(c =>
                c.Reservations.Any() == queryParameters.hasReservations.Value);

        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            customers = customers.Where(a =>
                a.Id.ToString().Contains(queryParameters.SearchTerm.ToLower()) ||
                a.FirstName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                a.LastName.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                a.Email.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                a.Phone.ToLower().Contains(queryParameters.SearchTerm.ToLower())
            );


        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            if (typeof(Customer).GetProperty(queryParameters.SortBy) != null)
                customers = customers.OrderByCustom(
                    queryParameters.SortBy,
                    queryParameters.SortOrder);

        customers = customers
            .Skip(queryParameters.Size * (queryParameters.Page - 1))
            .Take(queryParameters.Size);


        var mapper = _mappingConfiguration.CreateMapper();
        var customersDto = customers.Select(c => new
        {
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.Phone,
            c.IsVip,
            c.FullName,
            Reservations = mapper.Map<Models.DTO.Reservation[]>(c.Reservations)
        });


        // Outta left join
        return Ok(new { customers = await customersDto.ToArrayAsync() });
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
    ///     Create a new customer.
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
        return CreatedAtAction("GetCustomer", new { id = entity.Id }, entity);
    }


    /// <summary>
    ///     Update customer
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
    ///     Delete customer
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
    ///     Get all reservations for customer.
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

/// <summary>
///     Customers endpoint v2
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/customers")]
[Produces("application/json")]
public class CustomersV2Controller : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ICustomerService _customerService;
    private readonly IConfigurationProvider _mappingConfiguration;


    /// <summary>
    ///     Customer controllers constructor
    /// </summary>
    /// <param name="customerService"></param>
    public CustomersV2Controller(ICustomerService customerService, IConfigurationProvider mappingConfiguration,
        ApplicationDbContext context)
    {
        _customerService = customerService;
        _mappingConfiguration = mappingConfiguration;
        _context = context;
    }

    /// <summary>
    ///     Gets all customers.
    /// </summary>
    /// <returns>All customers</returns>
    /// <response code="200">Returns all customers</response>
    /// <response code="404">If there are no customers</response>
    [HttpGet("", Name = nameof(GetCustomers))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(
        [FromQuery] CustomerQueryParameters queryParameters)
    {

        var queryString = $"EXECUTE dbo.GetCustomers ";
        if (queryParameters.Id.HasValue) 
            queryString += $"@Id = {queryParameters.Id.Value}, "; // ID
        queryString += $"@FirstName = {queryParameters.FirstName ?? "null"}, "; // FirstName
        queryString += $"@LastName = {queryParameters.LastName ?? "null"}, "; // LastName
        queryString += $"@Email = {queryParameters.Email ?? "null"}, "; // Email
        queryString += $"@Phone = {queryParameters.Phone ?? "null"}, "; // Phone
        if (queryParameters.hasReservations.HasValue) 
            queryString += $"@HasReservations = {queryParameters.hasReservations.Value}, "; // HasReservations
        queryString += $"@Name = {queryParameters.Name ?? "null"}, "; // Name
        if (queryParameters.isVip.HasValue) 
            queryString += $"@IsVip = {queryParameters.isVip.Value}, "; // IsVip
        queryString += $"@Page = {queryParameters.Page}, "; // Page
        queryString += $"@Limit = {queryParameters.Size}, "; // Size
        if (typeof(Customer).GetProperty(queryParameters.SortBy) != null)
            queryString += $"@OrderBy = {queryParameters.SortBy}, "; // orderBy
        queryString += $"@OrderByAsc = {(queryParameters.SortOrder.ToLower() == "asc" ? 1 : 0)} "; // orderByAsc

        var customers =
             _context.Customers
                 .FromSqlRaw<CustomerEntity>(queryString)
                .ToList<CustomerEntity>()
                 
                // .Include(c=>c.Reservations)
                 // .AsEnumerable()
         ;

        var reservations = await _context.Reservations
            .FromSqlInterpolated($"EXECUTE GetReservationsForCustomer")
            .AsNoTracking()
            .ToListAsync();
        
        Parallel.ForEach(customers, customer=>
            customer.Reservations = reservations.Where(r=>r.CustomerId == customer.Id).ToList());
        
       


        var mapper = _mappingConfiguration.CreateMapper();
        var customersDto = customers.Select(c => new
        {
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.Phone,
            c.IsVip,
            c.FullName,
            c.Reservations
            // Reservations = mapper.Map<Models.DTO.Reservation[]>(c.Reservations)
        });


        // Outta left join
        return Ok(new { customers = customersDto });
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
    ///     Create a new customer.
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
        return CreatedAtAction("GetCustomer", new { id = entity.Id }, entity);
    }


    /// <summary>
    ///     Update customer
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
    ///     Patch customer.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <param name="customer">Customer Patch Object</param>
    /// <returns></returns>
    [HttpPatch("{id:int}", Name = nameof(PatchCustomer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PatchCustomer(int id, [FromBody] JsonPatchDocument<CustomerEntity> patchCus)
    {
        if (patchCus == null) return BadRequest(ModelState);
        var customer = await _context.Customers.FirstOrDefaultAsync(r=>r.Id == id);
        if(customer == null) return BadRequest();
        patchCus.ApplyTo(customer, ModelState);
        await _context.SaveChangesAsync();
        return !ModelState.IsValid ? BadRequest(ModelState) : new ObjectResult(customer);
        
    }


    /// <summary>
    ///     Delete customer
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <response code="201">Successfully deleted</response>
    /// <response code="400">Failed to delete</response>
    [Authorize(Roles = "Admin")]
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
    ///     Get all reservations for customer.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <returns></returns>
    [HttpGet("{id:int}/reservations", Name = nameof(GetReservationsForCustomer))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsForCustomer(int id)
    {
        var reservationEntities = await _context.Reservations
            .FromSqlInterpolated($"SELECT * FROM Reservations r WHERE r.CustomerId = {id}")
            .Include(r=>r.Area)
            .Include(r=>r.Customer)
            .Include(r=>r.Sitting)
            .Include(r=>r.ReservationTables)
            .ThenInclude(rt=>rt.Table)
            .ToListAsync();
        
        var mapper = _mappingConfiguration.CreateMapper();
        var reservations = mapper.Map<Reservation[]>(reservationEntities);
        // var reservations = await _customerService.GetReservationsAsync(id);
        // if (reservations == null) return NotFound();
        return Ok(new {reservations = reservations});
    }
}