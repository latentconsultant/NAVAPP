using System.ComponentModel.DataAnnotations;

namespace navigateapp.Models
{
    public class BillOfLading
    {
        public int Id { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        public string ShipperName { get; set; }
        public string ReceiverName { get; set; }
        public string Description { get; set; }
        public string? SignedBolPhotoPath { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public DateTime LastFollowUp { get; set; }
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; }
        public string? AssignedDriverId { get; set; }
        public string? CurrentRoute { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
