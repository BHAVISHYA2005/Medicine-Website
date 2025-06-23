namespace MedicineWebsite.Domain.Entities;

public class PharmacyMedicine
{
    public int Id { get; set; }
    public int PharmacyId { get; set; }
    public int MedicineId { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public decimal? DiscountPrice { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime? ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual Pharmacy Pharmacy { get; set; } = null!;
    public virtual Medicine Medicine { get; set; } = null!;
}
