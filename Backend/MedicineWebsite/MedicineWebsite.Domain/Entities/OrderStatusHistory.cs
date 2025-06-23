using MedicineWebsite.Domain.Enums;

namespace MedicineWebsite.Domain.Entities;

public class OrderStatusHistory
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
}
