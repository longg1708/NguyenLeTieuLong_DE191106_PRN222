using Microsoft.AspNetCore.Mvc;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbTravelCenterContext _context;

        public LoginController(DbTravelCenterContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string code, string password)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Please enter both Code and Password.";
                return View();
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.Code == code && c.Password == password);

            if (customer == null)
            {
                ViewBag.ErrorMessage = "Invalid Code or Password.";
                return View();
            }

            // Store customer info in session
            HttpContext.Session.SetInt32("CustomerID", customer.CustomerId);
            HttpContext.Session.SetString("CustomerCode", customer.Code);
            HttpContext.Session.SetString("CustomerName", customer.FullName);
            HttpContext.Session.SetString("CustomerRole", customer.Role);

            return RedirectToAction("Index", "Trips");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
