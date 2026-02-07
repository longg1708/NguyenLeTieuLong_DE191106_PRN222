using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class TripsController : Controller
    {
        private readonly DbTravelCenterContext _context;

        public TripsController(DbTravelCenterContext context)
        {
            _context = context;
        }

        // Check if user is logged in
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("CustomerID") != null;
        }

        // Check if user is admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("CustomerRole") == "Admin";
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            var trips = await _context.Trips.ToListAsync();
            return View(trips);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to create trips. Admin access required.";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,Code,Destination,Price,Status")] Trip trip)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to create trips. Admin access required.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trip created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to edit trips. Admin access required.";
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            return View(trip);
        }

        // POST: Trips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripId,Code,Destination,Price,Status")] Trip trip)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to edit trips. Admin access required.";
                return RedirectToAction(nameof(Index));
            }

            if (id != trip.TripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Trip updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.TripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to delete trips. Admin access required.";
                return RedirectToAction(nameof(Index));
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            // Check if there are any bookings for this trip before deleting
            var hasBookings = await _context.Bookings.AnyAsync(b => b.TripId == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete trip because it has associated bookings.";
                return RedirectToAction(nameof(Edit), new { id = id });
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Trip deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.TripId == id);
        }
    }
}
