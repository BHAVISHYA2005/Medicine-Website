using MedicineWebsite.Shared.DTOs.Medicine;

namespace MedicineWebsite.Application.Interfaces;

public interface IMedicineService
{
    Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(MedicineSearchDto searchDto);
    Task<MedicineDto?> GetMedicineByIdAsync(int id);
    Task<IEnumerable<MedicineDto>> GetMedicinesByPharmacyAsync(int pharmacyId);
    Task<IEnumerable<MedicineDto>> GetAlternativeMedicinesAsync(int medicineId);
    Task<bool> CheckMedicineAvailabilityAsync(int medicineId, string location);
}
