namespace MedicineWebsite.Domain.Entities;

public class Pharmacy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool IsOpen24Hours { get; set; }
    public string WorkingDays { get; set; } = string.Empty; // JSON array of working days
    public decimal DeliveryFee { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<UserFavoritePharmacy> UserFavorites { get; set; } = new List<UserFavoritePharmacy>();
}
