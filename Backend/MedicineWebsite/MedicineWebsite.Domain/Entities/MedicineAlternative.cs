namespace MedicineWebsite.Domain.Entities;

public class MedicineAlternative
{
    public int Id { get; set; }
    public int MedicineId { get; set; }
    public int AlternativeMedicineId { get; set; }
    public string Reason { get; set; } = string.Empty; // Why it's an alternative
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual Medicine Medicine { get; set; } = null!;
    public virtual Medicine AlternativeMedicine { get; set; } = null!;
}
