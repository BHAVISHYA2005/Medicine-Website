namespace MedicineWebsite.Domain.Entities;

public class UserFavoritePharmacy
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PharmacyId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Pharmacy Pharmacy { get; set; } = null!;
}
