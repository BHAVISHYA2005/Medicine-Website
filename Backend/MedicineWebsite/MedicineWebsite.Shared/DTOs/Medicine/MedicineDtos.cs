namespace MedicineWebsite.Shared.DTOs.Medicine;

public class MedicineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Strength { get; set; } = string.Empty;
    public string DosageForm { get; set; } = string.Empty;
    public string ActiveIngredients { get; set; } = string.Empty;
    public string DrugClass { get; set; } = string.Empty;
    public bool IsPrescriptionRequired { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public bool IsAvailable { get; set; }
    public List<PharmacyMedicineDto> AvailablePharmacies { get; set; } = new List<PharmacyMedicineDto>();
}

public class MedicineSearchDto
{
    public string SearchTerm { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal? MaxDistance { get; set; } = 50; // in kilometers
    public bool? PrescriptionRequired { get; set; }
    public string? DrugClass { get; set; }
    public string? DosageForm { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "name"; // name, price, distance
    public string SortOrder { get; set; } = "asc"; // asc, desc
}

public class PharmacyMedicineDto
{
    public int PharmacyId { get; set; }
    public string PharmacyName { get; set; } = string.Empty;
    public string PharmacyAddress { get; set; } = string.Empty;
    public string PharmacyPhone { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public double Distance { get; set; }
    public bool IsOpen { get; set; }
}
