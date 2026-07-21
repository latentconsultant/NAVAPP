using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using navigateapp.Data;
using navigateapp.Models;

namespace navigateapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MobileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MobileController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/Mobile/UpdateLocation
        [HttpPost("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationUpdateRequest request)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == request.VehicleId);
            if (vehicle == null) return NotFound("Vehicle not found");

            vehicle.Latitude = request.Latitude;
            vehicle.Longitude = request.Longitude;
            vehicle.LastUpdate = DateTime.Now;
            vehicle.Status = "In Transit";

            if (!string.IsNullOrEmpty(request.CurrentRoute))
            {
                vehicle.CurrentRoute = request.CurrentRoute;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Mobile/UploadBol
        [HttpPost("UploadBol")]
        public async Task<IActionResult> UploadBol([FromForm] BolUploadRequest request)
        {
            var bol = await _context.BillsOfLading.FirstOrDefaultAsync(b => b.OrderNumber == request.OrderNumber);
            if (bol == null) return NotFound("Order not found");

            if (request.Photo != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "bols");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + "_" + request.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Photo.CopyToAsync(fileStream);
                }

                bol.SignedBolPhotoPath = "/uploads/bols/" + fileName;
                bol.IsDelivered = true;
                await _context.SaveChangesAsync();
            }

            return Ok(new { path = bol.SignedBolPhotoPath });
        }

        public class LocationUpdateRequest
        {
            public string VehicleId { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string? CurrentRoute { get; set; }
        }

        public class BolUploadRequest
        {
            public string OrderNumber { get; set; }
            public IFormFile Photo { get; set; }
        }
    }
}
