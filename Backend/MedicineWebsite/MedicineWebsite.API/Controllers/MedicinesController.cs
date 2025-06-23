using Microsoft.AspNetCore.Mvc;
using MedicineWebsite.Application.Interfaces;
using MedicineWebsite.Shared.DTOs.Medicine;

namespace MedicineWebsite.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly IMedicineService _medicineService;
    private readonly ILogger<MedicinesController> _logger;

    public MedicinesController(IMedicineService medicineService, ILogger<MedicinesController> logger)
    {
        _medicineService = medicineService;
        _logger = logger;
    }

    /// <summary>
    /// Search for medicines based on various criteria
    /// </summary>
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> SearchMedicines([FromBody] MedicineSearchDto searchDto)
    {
        try
        {
            var medicines = await _medicineService.SearchMedicinesAsync(searchDto);
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching medicines with term: {SearchTerm}", searchDto.SearchTerm);
            return StatusCode(500, "An error occurred while searching medicines");
        }
    }

    /// <summary>
    /// Get medicine details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineDto>> GetMedicine(int id)
    {
        try
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            
            if (medicine == null)
                return NotFound($"Medicine with ID {id} not found");

            return Ok(medicine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medicine with ID: {MedicineId}", id);
            return StatusCode(500, "An error occurred while retrieving the medicine");
        }
    }

    /// <summary>
    /// Get all medicines available at a specific pharmacy
    /// </summary>
    [HttpGet("pharmacy/{pharmacyId}")]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicinesByPharmacy(int pharmacyId)
    {
        try
        {
            var medicines = await _medicineService.GetMedicinesByPharmacyAsync(pharmacyId);
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medicines for pharmacy: {PharmacyId}", pharmacyId);
            return StatusCode(500, "An error occurred while retrieving medicines");
        }
    }

    /// <summary>
    /// Get alternative medicines for a specific medicine
    /// </summary>
    [HttpGet("{id}/alternatives")]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAlternatives(int id)
    {
        try
        {
            var alternatives = await _medicineService.GetAlternativeMedicinesAsync(id);
            return Ok(alternatives);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting alternatives for medicine: {MedicineId}", id);
            return StatusCode(500, "An error occurred while retrieving alternatives");
        }
    }

    /// <summary>
    /// Check if a medicine is available in a specific location
    /// </summary>
    [HttpGet("{id}/availability")]
    public async Task<ActionResult<bool>> CheckAvailability(int id, [FromQuery] string location)
    {
        try
        {
            var isAvailable = await _medicineService.CheckMedicineAvailabilityAsync(id, location);
            return Ok(isAvailable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking availability for medicine: {MedicineId} at location: {Location}", id, location);
            return StatusCode(500, "An error occurred while checking availability");
        }
    }
}
