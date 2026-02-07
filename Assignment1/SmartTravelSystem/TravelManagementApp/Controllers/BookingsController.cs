using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly DbTravelCenterContext _context;

        public BookingsController(DbTravelCenterContext context)
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

        // GET: Bookings
        public async Task<IActionResult> Index(bool filterPending = false)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID").Value;

            var query = _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.Customer)
                .AsQueryable();

            if (!IsAdmin())
            {
                query = query.Where(b => b.CustomerId == customerId);
            }

            if (filterPending)
            {
                query = query.Where(b => b.Status == "Pending");
            }

            var bookings = await query.OrderBy(b => b.BookingDate).ToListAsync();

            ViewBag.FilterPending = filterPending;
            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            var trips = _context.Trips.ToList().Select(t => new
            {
                t.TripId,
                DisplayText = $"{t.Code} - {t.Destination} - ${t.Price:N2}"
            });

            ViewData["TripId"] = new SelectList(trips, "TripId", "DisplayText");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,BookingDate")] Booking booking)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID").Value;
            booking.CustomerId = customerId;
            booking.Status = "Pending";  // Always set to Pending for new bookings

            // Ignore navigation properties and manually set fields validation
            ModelState.Remove("Customer");
            ModelState.Remove("Trip");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var trips = _context.Trips.ToList().Select(t => new
            {
                t.TripId,
                DisplayText = $"{t.Code} - {t.Destination} - ${t.Price:N2}"
            });

            ViewData["TripId"] = new SelectList(trips, "TripId", "DisplayText", booking.TripId);
            return View(booking);
        }

        // GET: Bookings/Cancel/5 - Only for Pending bookings
        public async Task<IActionResult> Cancel(int? id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID").Value;
            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.CustomerId == customerId && b.Status == "Pending");

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Only pending bookings can be cancelled.";
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        // POST: Bookings/Cancel/5 - Cancel a Pending booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID").Value;
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id && b.CustomerId == customerId && b.Status == "Pending");

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found or cannot be cancelled.";
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Cancelled";

            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.BookingId))
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

        // POST: Bookings/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (!IsAuthenticated() || !IsAdmin())
            {
                return RedirectToAction("Index", "Login");
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending bookings can be approved.";
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Confirmed";
            _context.Update(booking);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Booking approved successfully.";
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Bookings/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            if (!IsAuthenticated() || !IsAdmin())
            {
                return RedirectToAction("Index", "Login");
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending bookings can be rejected.";
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Cancelled";
            _context.Update(booking);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Booking rejected successfully.";
            
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
