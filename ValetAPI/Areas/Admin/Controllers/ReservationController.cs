using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ValetAPI.Data;
using ValetAPI.Models;
using ValetAPI.Services;

namespace ValetAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly ISittingService _sittingService;

        public ReservationController(IReservationService reservationService, ICustomerService customerService, ISittingService sittingService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _sittingService = sittingService;
        }

        // GET: Admin/Reservation
        public async Task<IActionResult> Index()
        {
            var entities = await _reservationService.GetReservationsAsync();
            // var applicationDbContext = _context.Reservation.Include(r => r.Customer).Include(r => r.Sitting);
            return View(entities);
        }

        // GET: Admin/Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var entity = await _reservationService.GetReservationAsync(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Admin/Reservation/Create
        public async Task<IActionResult> Create()
        {
            const int increment = 15;


            var customers = await _customerService.GetCustomersAsync();
            var sittings = await _sittingService.GetSittingsAsync();
            ViewData["Customers"] = new SelectList(customers, "Id", "FullName");
            ViewData["Sittings"] = new SelectList(sittings, "Id", "StartTime");

            // ViewData["Source"] = new SelectList(Enum.GetValues<Source>(), "Id", "StartTime");

            var allStartTimes = sittings.Select(s => s.StartTime).ToArray();
            var allEndTimes = sittings.Select(s => s.EndTime).ToArray();


            var incrementedTimes = sittings.Select(s => Enumerable.Range(0, (int)(s.EndTime - s.StartTime).TotalMinutes / increment)
                    .Select(minutes => s.StartTime.AddMinutes(minutes * increment)).Select(t => new
                    {
                        DateTime = t,
                        Time = t.ToShortTimeString()
                    }).ToArray()).ToArray();
            var startDay = DateTime.Now;
            var endDay = DateTime.Now.AddDays(1);
            var allTimes = Enumerable.Range(0, (int)(endDay - startDay).TotalMinutes / increment)
                .Select(minutes => startDay.AddMinutes(minutes * increment)).Select(t => new
                {
                    DateTime = t,
                    Time = t.ToShortTimeString()
                }).ToArray();
            // ViewData["ReservationTimes"] = new SelectList(incrementedTimes, "Id", "Times");
            ViewData["allTimes"] = new SelectList(allTimes, "DateTime", "Time");
            ViewData["ReservationTime"] = incrementedTimes.Select(i=> new SelectList(i, "DateTime", "Time"));

            return View();
        }
        // https://www.jonthornton.com/jquery-timepicker/
        // POST: Admin/Reservation/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,CustomerId,SittingId,DateTime,Duration,NoGuests,Source,Status,Notes")] Reservation reservation)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(reservation);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Id", reservation.CustomerId);
        //     ViewData["SittingId"] = new SelectList(_context.Set<Sitting>(), "Id", "Id", reservation.SittingId);
        //     return View(reservation);
        // }
        //
        // // GET: Admin/Reservation/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null || _context.Reservation == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var reservation = await _context.Reservation.FindAsync(id);
        //     if (reservation == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Id", reservation.CustomerId);
        //     ViewData["SittingId"] = new SelectList(_context.Set<Sitting>(), "Id", "Id", reservation.SittingId);
        //     return View(reservation);
        // }
        //
        // // POST: Admin/Reservation/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,SittingId,DateTime,Duration,NoGuests,Source,Status,Notes")] Reservation reservation)
        // {
        //     if (id != reservation.Id)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(reservation);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!ReservationExists(reservation.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Id", reservation.CustomerId);
        //     ViewData["SittingId"] = new SelectList(_context.Set<Sitting>(), "Id", "Id", reservation.SittingId);
        //     return View(reservation);
        // }
        //
        // // GET: Admin/Reservation/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null || _context.Reservation == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var reservation = await _context.Reservation
        //         .Include(r => r.Customer)
        //         .Include(r => r.Sitting)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (reservation == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(reservation);
        // }
        //
        // // POST: Admin/Reservation/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     if (_context.Reservation == null)
        //     {
        //         return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        //     }
        //     var reservation = await _context.Reservation.FindAsync(id);
        //     if (reservation != null)
        //     {
        //         _context.Reservation.Remove(reservation);
        //     }
        //     
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }
        //
        // private bool ReservationExists(int id)
        // {
        //   return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        // }
    }

    public class TimeIncrement
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public List<DateTime> Increments = new();
    }
}
