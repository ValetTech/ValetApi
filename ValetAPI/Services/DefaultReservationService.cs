using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Filters;
using ValetAPI.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ValetAPI.Services;

public class DefaultReservationService : IReservationService
{
    // GetReservation // By Id
    // GetReservations
    // PostReservation
    // PutReservation
    // DeleteReservation

    // Patch

    // Get tables

    // Add table
    // Patch - /api/reservation/:id/?tableId

    // Create Reservation : 
    // Customer, Sitting, DateTime, Duration, NoGuests, Source, Status, Notes?
    // VenueId = Sitting.VenueId

    private readonly ApplicationDbContext _context;
    private readonly ICustomerService _customerService;
    private readonly IConfigurationProvider _mappingConfiguration;
    private readonly ISittingService _sittingService;

    public DefaultReservationService(ApplicationDbContext context, IConfigurationProvider mappingConfiguration,
        ICustomerService customerService, ISittingService sittingService)
    {
        _context = context;
        _mappingConfiguration = mappingConfiguration;
        _customerService = customerService;
        _sittingService = sittingService;
    }

    // Get All Reservations
    public async Task<IQueryable<Reservation>> GetReservationsAsync()
    {
        var reservations = _context
            .Reservations
            // .Include(r => r.Customer)
            // .Include(r => r.Tables)
            // .Include(r => r.Sitting)
            // .Include(r => r.Venue)
            .AsSplitQuery()
            .AsQueryable();

        return reservations.ProjectTo<Reservation>(_mappingConfiguration);
        // return await _context.Reservations.ProjectTo<Reservation>(_mappingConfiguration).ToListAsync();
    }

    // Get Reservation by id
    public async Task<Reservation> GetReservationAsync(int reservationId)
    {
        if (_context.Reservations == null) return null;
        var reservation = await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.ReservationTables)
            .ThenInclude(rt=>rt.Table)
            .Include(r => r.Sitting)
            .Include(r=>r.Area)
            .Include(r => r.Venue)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null) return null;

        var mapper = _mappingConfiguration.CreateMapper();
        return mapper.Map<Reservation>(reservation);
    }

    public async Task<int> CreateReservationAsync(Reservation reservation)
    {
        if (_context.Reservations == null) return -1;
        var mapper = _mappingConfiguration.CreateMapper();

        var reservationEntity = mapper.Map<ReservationEntity>(reservation);


        var customer =
            await _customerService.GetOrCreateCustomerEntityAsync(reservation.CustomerId, reservation.Customer);
        var sitting = await _sittingService.GetSittingEntityAsync(reservation.SittingId);
        var venueId = sitting.VenueId;

        reservationEntity.Customer = customer;
        reservationEntity.Sitting = sitting;
        reservationEntity.VenueId = venueId;


        _context.Reservations.Add(reservationEntity);
        await _context.SaveChangesAsync();

        return reservationEntity.Id;


        /*{
            "customerId": 1,
            "sittingId": 111,
            "dateTime": "2022-09-06T00:11:13.174Z",
            "duration": 0,
            "noGuests": 0,
            "source": 0,
            "tables": [],
            "notes": "string"
        }*/
    }


    public async Task UpdateReservationAsync(Reservation reservation)
    {
        
        var mapper = _mappingConfiguration.CreateMapper();

        var entity = await _context.Reservations.FirstOrDefaultAsync(v => v.Id == reservation.Id);
        if (entity == null) throw new HttpResponseException(404, "Reservation not found.");

        var reservationEntity = mapper.Map<ReservationEntity>(reservation);

        _context.Entry(entity).CurrentValues.SetValues(reservationEntity);

        var created = await _context.SaveChangesAsync();
        if (created < 1) throw new HttpResponseException(400, "Could not update reservation.");
        
    }
    
    public async Task DeleteReservationAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null) throw new HttpResponseException(404, "Reservation not found.");

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Table>> GetReservationTables(int reservationId)
    {
        // if (_context.Reservations == null) return null;
        // var reservationEntity = await _context.Reservations
        //     .Include(r => r.ReservationTables)
        //     .ThenInclude(rt=>rt.Table)
        //     .FirstOrDefaultAsync(r => r.Id == reservationId);
        // if (reservationEntity?.ReservationTables == null) return null;

        var resEntities = 
            await _context.Tables.FromSqlInterpolated($"EXECUTE GetReservationsTables {reservationId}").Cast<TableEntity>().ToListAsync();
        
        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<Table[]>(resEntities);
    }

    public async Task AddTableToReservation(int reservationId, int tableId)
    {
        if (_context.Reservations == null) throw new HttpResponseException(404);
        var reservationEntity = await _context.Reservations
            .Include(r => r.Sitting)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        var resArea = await _context.Areas.Include(a=>a.Tables).FirstOrDefaultAsync(a => a.Id == reservationEntity.AreaId);
        // var table = await _context.Tables.FirstOrDefaultAsync(t=>t.Id == tableId);
        // if(table == null) throw new HttpResponseException(404);
        // var tableEntity = resArea?.Tables.FirstOrDefault(t=>t.Id == tableId);
        var tableEntity = await _context.Tables.FirstOrDefaultAsync(t=>t.Id == tableId);
        // var tables = await GetAvailableTableEntitiesForSittingAsync(reservationEntity.Sitting.Id);

        if (tableEntity?.AreaId != resArea?.Id)
            throw new HttpResponseException(400, "Table not in Reservations Area.");


        if (reservationEntity == null || tableEntity == null) throw new HttpResponseException(404);
        
        reservationEntity.ReservationTables.Add(new ReservationTable{ReservationId = reservationEntity.Id, TableId = tableEntity.Id});
        // areas.Select(id => new AreaSittingEntity {AreaId = id, SittingId = sitting.Id})
        // _context.ReservationsTables.Add(new ReservationTable
        //     { ReservationId = reservationEntity.Id, TableId = tableEntity.Id });
        // reservationEntity.ReservationTables.Add(new ReservationTable{ReservationId = reservationEntity.Id, TableId = tableEntity.Id});
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AddTablesToReservation(int reservationId, int[] tableIds)
    {
        if (_context.Reservations == null) return false;
        var reservationEntity = await _context.Reservations
            .Include(r => r.ReservationTables)
            .Include(r => r.Sitting)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservationEntity == null) return false;

        // var tableEntity = await _context.Tables.FindAsync(tableIds);

        var availableTables = await GetAvailableTableEntitiesForSittingAsync(reservationEntity.Sitting.Id);

        var tables = availableTables
            .Where(t => tableIds.Contains(t.Id))
            .AsQueryable();


        reservationEntity.ReservationTables.AddRange(tables.Select(t=>new ReservationTable{ReservationId = reservationEntity.Id, TableId = t.Id}));

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveTableFromReservation(int reservationId, int tableId)
    {
        if (_context.Reservations == null) return false;
        var reservationEntity = await _context.Reservations.FindAsync(reservationId);
        var tableEntity = await _context.Tables.FindAsync(tableId);

        if (reservationEntity == null || tableEntity == null) return false;
        reservationEntity.ReservationTables.RemoveAll(t => t.TableId == tableEntity.Id);
        // reservationEntity.Tables.Remove();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveTablesFromReservation(int reservationId, int[] tableIds)
    {
        if (_context.Reservations == null) return false;

        var reservationEntity = await _context.Reservations
            .Include(r => r.ReservationTables)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        var tableEntity = await _context.Tables
            .Where(t => tableIds.Contains(t.Id))
            .ToArrayAsync();

        if (reservationEntity == null || tableEntity == null) return false;

        reservationEntity.ReservationTables.RemoveAll(t => tableIds.Contains(t.TableId));
        // reservationEntity.Tables.RemoveAll(t => tableIds.Contains(t.Id));

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<IQueryable<TableEntity>> GetAvailableTableEntitiesForSittingAsync(int sittingId)
    {
        if (_context.Sittings == null) return null;

        var sitting = await _context.Sittings
            .Include(s => s.Reservations)
            .ThenInclude(r => r.ReservationTables)
            .Include(s => s.AreaSittings)
            .ThenInclude(a => a.Area)
            .ThenInclude(a => a.Tables)
            .SingleOrDefaultAsync(a => a.Id == sittingId);
        ;

        if (sitting == null) return null;

        var takenTables = sitting.Reservations
            .SelectMany(r => r.ReservationTables, (reservation, tableEntity) => tableEntity)
            .AsQueryable();
        

        var tables = sitting!.AreaSittings
            .Select(sa => sa.Area)
            // .Select(a => a.Tables)
            .SelectMany(a => a.Tables)
            .Where(t => !takenTables.Select(rt=>rt.TableId).Contains(t.Id))
            .AsQueryable();
        
        

        return tables;
    }
}