using Microsoft.EntityFrameworkCore;
using MedicineWebsite.Application.Interfaces;
using MedicineWebsite.Domain.Entities;
using MedicineWebsite.Persistence.Data;

namespace MedicineWebsite.Persistence.Repositories;

public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
{
    public MedicineRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Medicine>> SearchAsync(string searchTerm, int skip, int take)
    {
        var query = _dbSet.Where(m => m.IsActive);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(m => 
                m.Name.ToLower().Contains(searchTerm) ||
                m.GenericName.ToLower().Contains(searchTerm) ||
                m.ActiveIngredients.ToLower().Contains(searchTerm) ||
                m.Manufacturer.ToLower().Contains(searchTerm)
            );
        }

        return await query
            .Include(m => m.PharmacyMedicines)
            .ThenInclude(pm => pm.Pharmacy)
            .OrderBy(m => m.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<Medicine>> GetByPharmacyAsync(int pharmacyId)
    {
        return await _dbSet
            .Where(m => m.IsActive && m.PharmacyMedicines.Any(pm => pm.PharmacyId == pharmacyId && pm.IsAvailable))
            .Include(m => m.PharmacyMedicines.Where(pm => pm.PharmacyId == pharmacyId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Medicine>> GetAlternativesAsync(int medicineId)
    {
        return await _dbSet
            .Where(m => m.IsActive && m.AlternativeTo.Any(alt => alt.MedicineId == medicineId))
            .Include(m => m.PharmacyMedicines)
            .ThenInclude(pm => pm.Pharmacy)
            .ToListAsync();
    }

    public override async Task<Medicine?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(m => m.PharmacyMedicines)
            .ThenInclude(pm => pm.Pharmacy)
            .Include(m => m.Alternatives)
            .ThenInclude(alt => alt.AlternativeMedicine)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}
