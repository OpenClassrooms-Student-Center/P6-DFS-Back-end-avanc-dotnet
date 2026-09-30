using Microsoft.AspNetCore.Mvc;
using NorlysMobilite.Models;
using NorlysMobilite.Services;

namespace NorlysMobilite.Controllers;

/// <summary>
/// Vélos et trottinettes de la flotte.
/// </summary>
[ApiController]
[Route("api/vehicles")]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>Liste des véhicules.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Vehicle>>> GetAll()
    {
        return await _vehicleService.FindAllAsync();
    }

    /// <summary>Détail d'un véhicule.</summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Vehicle>> GetById(long id)
    {
        return await _vehicleService.FindByIdAsync(id);
    }

    /// <summary>Ajout d'un véhicule à la flotte.</summary>
    [HttpPost]
    public async Task<ActionResult<Vehicle>> Create([FromBody] Vehicle vehicle)
    {
        var created = await _vehicleService.CreateAsync(vehicle);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Affectation du véhicule à une station.</summary>
    [HttpPut("{id:long}/station/{stationId:long}")]
    public async Task<ActionResult<Vehicle>> AssignToStation(long id, long stationId)
    {
        return await _vehicleService.AssignToStationAsync(id, stationId);
    }

    /// <summary>Début de location.</summary>
    [HttpPost("{id:long}/rental/start")]
    public async Task<ActionResult<Vehicle>> StartRental(long id)
    {
        return await _vehicleService.StartRentalAsync(id);
    }

    /// <summary>Fin de location : retour en station avec le niveau de batterie constaté.</summary>
    [HttpPost("{id:long}/rental/end")]
    public async Task<ActionResult<Vehicle>> EndRental(long id, [FromQuery] long stationId, [FromQuery] int batteryLevel)
    {
        return await _vehicleService.EndRentalAsync(id, stationId, batteryLevel);
    }

    /// <summary>Mise à jour du niveau de batterie.</summary>
    [HttpPut("{id:long}/battery")]
    public async Task<ActionResult<Vehicle>> UpdateBattery(long id, [FromQuery] int level)
    {
        return await _vehicleService.UpdateBatteryAsync(id, level);
    }

    /// <summary>Envoi en maintenance.</summary>
    [HttpPost("{id:long}/maintenance")]
    public async Task<ActionResult<Vehicle>> SendToMaintenance(long id)
    {
        return await _vehicleService.SendToMaintenanceAsync(id);
    }

    /// <summary>Retour de maintenance.</summary>
    [HttpPost("{id:long}/maintenance/return")]
    public async Task<ActionResult<Vehicle>> ReturnFromMaintenance(long id)
    {
        return await _vehicleService.ReturnFromMaintenanceAsync(id);
    }

    /// <summary>Retrait définitif de la flotte.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Retire(long id)
    {
        await _vehicleService.RetireAsync(id);
        return NoContent();
    }
}
