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
            try
            {
                var bills = await _context.BillsOfLading.ToListAsync();
                return View(bills ?? new List<BillOfLading>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error in Bills: {ex.Message}");
                return View(new List<BillOfLading>());
            }
        }

        // POST: Logistics/CreateBill
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBill(BillOfLading bill)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(bill);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Bills));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error creating bill: {ex.Message}");
            }
            return View("Bills", await _context.BillsOfLading.ToListAsync());
        }

        // GET: Logistics/Customers
        public async Task<IActionResult> Customers()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                return View(customers ?? new List<Customer>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error in Customers: {ex.Message}");
                return View(new List<Customer>());
            }
        }

        // POST: Logistics/CreateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Customers));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error creating customer: {ex.Message}");
            }
            return View("Customers", await _context.Customers.ToListAsync());
        }

        // GET: Logistics/Vehicles
        public async Task<IActionResult> Vehicles()
        {
            try
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                return View(vehicles ?? new List<Vehicle>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error in Vehicles: {ex.Message}");
                return View(new List<Vehicle>());
            }
        }

        // POST: Logistics/CreateVehicle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vehicle.LastUpdate = DateTime.Now;
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Vehicles));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG_LOG] Error creating vehicle: {ex.Message}");
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
