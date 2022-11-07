using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Filters;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultCustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfigurationProvider _mappingConfiguration;

    public DefaultCustomerService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllCustomerEntitiesAsync()
    {
        if (_context.Customers == null) return null;
        return await _context.Customers.AsSplitQuery().ToArrayAsync();
    }

    public async Task<IQueryable<Customer>> GetCustomersAsync()
    {
        if (_context.Customers == null) 
            throw new HttpResponseException(404, "Customers not found.");
        return _context.Customers.AsSplitQuery().AsQueryable().ProjectTo<Customer>(_mappingConfiguration);
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        if (_context.Customers == null) return null;
        var customer = await _context.Customers
            .SingleOrDefaultAsync(v => v.Id == customerId);

        if (customer == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Customer>(customer);
    }

    public async Task<int> CreateCustomerAsync(Customer customer)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Customers.AddAsync(mapper.Map<CustomerEntity>(customer));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create customer.");

        return entity.Entity.Id;
    }

    public async Task<CustomerEntity> GetOrCreateCustomerEntityAsync(int? customerId = null, Customer? customer = null)
    {
        var mapper = _mappingConfiguration.CreateMapper();
        var id = customerId ?? customer?.Id;
        if (id != null)
        {
            var customerEntity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customerEntity != null) return customerEntity;
        }

        if (customer == null)
            throw new InvalidOperationException("Can not find or create Customer: Missing information.");
        var entity = await _context.Customers.AddAsync(mapper.Map<CustomerEntity>(customer));
        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new InvalidOperationException("Could not create customer.");

        return entity.Entity;
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        var customer = await _context.Customers
            .SingleOrDefaultAsync(v => v.Id == customerId);

        if (customer == null) return;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Customers.FirstOrDefaultAsync(v => v.Id == customer.Id);
        if (entity == null) return;


        _context.Entry(entity).CurrentValues.SetValues(mapper.Map<CustomerEntity>(customer));

        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new HttpResponseException(400, "Could not update customer.");
    }

    public async Task<IEnumerable<Reservation>> GetReservationsAsync(int customerId)
    {
        if (_context.Customers == null) return null;

        var customer = await _context.Customers
            .Include(v => v.Reservations)
            .SingleOrDefaultAsync(v => v.Id == customerId);

        if (customer?.Reservations == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Reservation[]>(customer.Reservations);
    }
}