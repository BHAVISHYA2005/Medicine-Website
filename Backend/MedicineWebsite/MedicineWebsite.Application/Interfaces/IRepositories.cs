using MedicineWebsite.Domain.Entities;

namespace MedicineWebsite.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(int id);
}

public interface IMedicineRepository : IGenericRepository<Medicine>
{
    Task<IEnumerable<Medicine>> SearchAsync(string searchTerm, int skip, int take);
    Task<IEnumerable<Medicine>> GetByPharmacyAsync(int pharmacyId);
    Task<IEnumerable<Medicine>> GetAlternativesAsync(int medicineId);
}

public interface IPharmacyRepository : IGenericRepository<Pharmacy>
{
    Task<IEnumerable<Pharmacy>> GetNearbyPharmaciesAsync(double latitude, double longitude, double radiusKm);
    Task<IEnumerable<Pharmacy>> GetPharmaciesWithMedicineAsync(int medicineId);
}

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<IEnumerable<Order>> GetPharmacyOrdersAsync(int pharmacyId);
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
}
