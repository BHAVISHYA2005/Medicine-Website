using Microsoft.EntityFrameworkCore;
using MedicineWebsite.Application.Interfaces;
using MedicineWebsite.Domain.Entities;
using MedicineWebsite.Persistence.Data;

namespace MedicineWebsite.Persistence.Repositories;

public class PharmacyRepository : GenericRepository<Pharmacy>, IPharmacyRepository
{
    public PharmacyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pharmacy>> GetNearbyPharmaciesAsync(double latitude, double longitude, double radiusKm)
    {
        // Using a simple approximation for distance calculation in SQL
        // For production, consider using more sophisticated spatial queries
        return await _dbSet
            .Where(p => p.IsActive)
            .Where(p => 
                Math.Sqrt(
                    Math.Pow(69.1 * (p.Latitude - latitude), 2) +
                    Math.Pow(69.1 * (longitude - p.Longitude) * Math.Cos(latitude / 57.3), 2)
                ) <= radiusKm)
            .Include(p => p.PharmacyMedicines)
            .ThenInclude(pm => pm.Medicine)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pharmacy>> GetPharmaciesWithMedicineAsync(int medicineId)
    {
        return await _dbSet
            .Where(p => p.IsActive && p.PharmacyMedicines.Any(pm => pm.MedicineId == medicineId && pm.IsAvailable))
            .Include(p => p.PharmacyMedicines.Where(pm => pm.MedicineId == medicineId))
            .ThenInclude(pm => pm.Medicine)
            .ToListAsync();
    }

    public override async Task<Pharmacy?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.PharmacyMedicines)
            .ThenInclude(pm => pm.Medicine)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
