using Microsoft.AspNetCore.Mvc;
using navigateapp.Data;
using navigateapp.Models;
using Microsoft.EntityFrameworkCore;

namespace navigateapp.Controllers
{
    public class LogisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public LogisticsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Logistics/Bills
        public async Task<IActionResult> Bills()
        {
            return View(await _context.BillsOfLading.ToListAsync());
        }

        // POST: Logistics/CreateBill
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBill(BillOfLading bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Bills));
            }
            return View("Bills", await _context.BillsOfLading.ToListAsync());
        }

        // GET: Logistics/Customers
        public async Task<IActionResult> Customers()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // POST: Logistics/CreateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Customers));
            }
            return View("Customers", await _context.Customers.ToListAsync());
        }

        // GET: Logistics/Vehicles
        public async Task<IActionResult> Vehicles()
        {
            return View(await _context.Vehicles.ToListAsync());
        }

        // POST: Logistics/CreateVehicle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.LastUpdate = DateTime.Now;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Vehicles));
            }
            return View("Vehicles", await _context.Vehicles.ToListAsync());
        }

        // POST: Logistics/DeleteVehicle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Vehicles));
        }

        // GET: Logistics/Admin
        public IActionResult Admin()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            ViewBag.BillCount = _context.BillsOfLading?.Count() ?? 0;
            ViewBag.CustomerCount = _context.Customers?.Count() ?? 0;
            ViewBag.VehicleCount = _context.Vehicles?.Count() ?? 0;

            return View();
        }

        // GET: Logistics/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Logistics/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var adminUser = _configuration["AdminCredentials:Username"] ?? "admin";
            var adminPass = _configuration["AdminCredentials:Password"] ?? "admin123";

            if (username == adminUser && password == adminPass)
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction(nameof(Admin));
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLoggedIn");
            return RedirectToAction(nameof(Login));
        }

        // GET: Logistics/Driver
        public IActionResult Driver()
        {
            return View();
        }

        // API for vehicle tracking
        [HttpGet]
        public async Task<JsonResult> GetVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Json(vehicles);
        }
    }
}
