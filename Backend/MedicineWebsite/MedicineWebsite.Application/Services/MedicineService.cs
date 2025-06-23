using MedicineWebsite.Application.Interfaces;
using MedicineWebsite.Shared.DTOs.Medicine;
using MedicineWebsite.Domain.Entities;

namespace MedicineWebsite.Application.Services;

public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly IPharmacyRepository _pharmacyRepository;

    public MedicineService(IMedicineRepository medicineRepository, IPharmacyRepository pharmacyRepository)
    {
        _medicineRepository = medicineRepository;
        _pharmacyRepository = pharmacyRepository;
    }

    public async Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(MedicineSearchDto searchDto)
    {
        var skip = (searchDto.PageNumber - 1) * searchDto.PageSize;
        var medicines = await _medicineRepository.SearchAsync(searchDto.SearchTerm, skip, searchDto.PageSize);

        var medicineDtos = new List<MedicineDto>();

        foreach (var medicine in medicines)
        {
            var medicineDto = MapToMedicineDto(medicine);
            
            // Filter pharmacies based on location if provided
            if (searchDto.Latitude.HasValue && searchDto.Longitude.HasValue)
            {
                medicineDto.AvailablePharmacies = medicineDto.AvailablePharmacies
                    .Where(pm => CalculateDistance(
                        searchDto.Latitude.Value, searchDto.Longitude.Value,
                        pm.Distance, 0) <= (double)(searchDto.MaxDistance ?? 50))
                    .OrderBy(pm => pm.Distance)
                    .ToList();
            }

            // Apply price filters
            if (searchDto.MinPrice.HasValue || searchDto.MaxPrice.HasValue)
            {
                medicineDto.AvailablePharmacies = medicineDto.AvailablePharmacies
                    .Where(pm => 
                        (!searchDto.MinPrice.HasValue || pm.Price >= searchDto.MinPrice.Value) &&
                        (!searchDto.MaxPrice.HasValue || pm.Price <= searchDto.MaxPrice.Value))
                    .ToList();
            }

            if (medicineDto.AvailablePharmacies.Any())
            {
                medicineDtos.Add(medicineDto);
            }
        }

        // Sort results
        return searchDto.SortBy.ToLower() switch
        {
            "price" => searchDto.SortOrder == "desc" 
                ? medicineDtos.OrderByDescending(m => m.Price) 
                : medicineDtos.OrderBy(m => m.Price),
            "distance" => searchDto.SortOrder == "desc" 
                ? medicineDtos.OrderByDescending(m => m.AvailablePharmacies.Min(p => p.Distance)) 
                : medicineDtos.OrderBy(m => m.AvailablePharmacies.Min(p => p.Distance)),
            _ => searchDto.SortOrder == "desc" 
                ? medicineDtos.OrderByDescending(m => m.Name) 
                : medicineDtos.OrderBy(m => m.Name)
        };
    }

    public async Task<MedicineDto?> GetMedicineByIdAsync(int id)
    {
        var medicine = await _medicineRepository.GetByIdAsync(id);
        return medicine != null ? MapToMedicineDto(medicine) : null;
    }

    public async Task<IEnumerable<MedicineDto>> GetMedicinesByPharmacyAsync(int pharmacyId)
    {
        var medicines = await _medicineRepository.GetByPharmacyAsync(pharmacyId);
        return medicines.Select(MapToMedicineDto);
    }

    public async Task<IEnumerable<MedicineDto>> GetAlternativeMedicinesAsync(int medicineId)
    {
        var alternatives = await _medicineRepository.GetAlternativesAsync(medicineId);
        return alternatives.Select(MapToMedicineDto);
    }

    public async Task<bool> CheckMedicineAvailabilityAsync(int medicineId, string location)
    {
        var medicine = await _medicineRepository.GetByIdAsync(medicineId);
        return medicine?.PharmacyMedicines.Any(pm => pm.IsAvailable && pm.StockQuantity > 0) ?? false;
    }

    private static MedicineDto MapToMedicineDto(Medicine medicine)
    {
        var availablePharmacies = medicine.PharmacyMedicines
            .Where(pm => pm.IsAvailable && pm.StockQuantity > 0)
            .Select(pm => new PharmacyMedicineDto
            {
                PharmacyId = pm.PharmacyId,
                PharmacyName = pm.Pharmacy.Name,
                PharmacyAddress = $"{pm.Pharmacy.Address}, {pm.Pharmacy.City}",
                PharmacyPhone = pm.Pharmacy.PhoneNumber,
                Price = pm.Price,
                DiscountPrice = pm.DiscountPrice,
                StockQuantity = pm.StockQuantity,
                IsAvailable = pm.IsAvailable,
                Distance = 0, // Will be calculated based on user location
                IsOpen = IsPharmacyOpen(pm.Pharmacy)
            })
            .ToList();

        return new MedicineDto
        {
            Id = medicine.Id,
            Name = medicine.Name,
            GenericName = medicine.GenericName,
            Manufacturer = medicine.Manufacturer,
            Description = medicine.Description,
            Strength = medicine.Strength,
            DosageForm = medicine.DosageForm,
            ActiveIngredients = medicine.ActiveIngredients,
            DrugClass = medicine.DrugClass,
            IsPrescriptionRequired = medicine.IsPrescriptionRequired,
            ImageUrl = medicine.ImageUrl,
            Price = availablePharmacies.Any() ? availablePharmacies.Min(p => p.DiscountPrice ?? p.Price) : null,
            DiscountPrice = availablePharmacies.Any() ? availablePharmacies.Min(p => p.DiscountPrice) : null,
            IsAvailable = availablePharmacies.Any(),
            AvailablePharmacies = availablePharmacies
        };
    }

    private static bool IsPharmacyOpen(Pharmacy pharmacy)
    {
        if (pharmacy.IsOpen24Hours) return true;
        
        var now = DateTime.Now.TimeOfDay;
        return now >= pharmacy.OpeningTime && now <= pharmacy.ClosingTime;
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadius = 6371; // km
        
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadius * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
