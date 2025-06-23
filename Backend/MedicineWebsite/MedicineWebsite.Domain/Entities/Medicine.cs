namespace MedicineWebsite.Domain.Entities;

public class Medicine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Strength { get; set; } = string.Empty;
    public string DosageForm { get; set; } = string.Empty; // Tablet, Capsule, Syrup, etc.
    public string ActiveIngredients { get; set; } = string.Empty;
    public string DrugClass { get; set; } = string.Empty;
    public bool IsPrescriptionRequired { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string RxNormId { get; set; } = string.Empty; // For RxNorm API integration
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<MedicineAlternative> Alternatives { get; set; } = new List<MedicineAlternative>();
    public virtual ICollection<MedicineAlternative> AlternativeTo { get; set; } = new List<MedicineAlternative>();
}
