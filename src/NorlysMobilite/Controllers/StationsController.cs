using Microsoft.AspNetCore.Mvc;
using NorlysMobilite.Models;
using NorlysMobilite.Services;

namespace NorlysMobilite.Controllers;

/// <summary>
/// Stations de la flotte et leur occupation.
/// </summary>
[ApiController]
[Route("api/stations")]
[Produces("application/json")]
public class StationsController : ControllerBase
{
    private readonly IStationService _stationService;

    public StationsController(IStationService stationService)
    {
        _stationService = stationService;
    }

    /// <summary>Liste des stations.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Station>>> GetAll()
    {
        return await _stationService.FindAllAsync();
    }

    /// <summary>Détail d'une station.</summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Station>> GetById(long id)
    {
        return await _stationService.FindByIdAsync(id);
    }

    /// <summary>Création d'une station.</summary>
    [HttpPost]
    public async Task<ActionResult<Station>> Create([FromBody] Station station)
    {
        var created = await _stationService.CreateAsync(station);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Mise à jour du nom, de l'adresse, de la capacité et des coordonnées.</summary>
    [HttpPut("{id:long}")]
    public async Task<ActionResult<Station>> Update(long id, [FromBody] Station station)
    {
        return await _stationService.UpdateAsync(id, station);
    }

    /// <summary>Désignation du responsable de la station.</summary>
    [HttpPut("{id:long}/manager/{agentId:long}")]
    public async Task<ActionResult<Station>> AssignManager(long id, long agentId)
    {
        return await _stationService.AssignManagerAsync(id, agentId);
    }

    /// <summary>Véhicules présents dans la station.</summary>
    [HttpGet("{id:long}/vehicles")]
    public async Task<ActionResult<List<Vehicle>>> GetVehicles(long id)
    {
        return await _stationService.VehiclesOfAsync(id);
    }

    /// <summary>Occupation de la station : places libres et taux.</summary>
    [HttpGet("{id:long}/occupancy")]
    public async Task<ActionResult<StationOccupancy>> GetOccupancy(long id)
    {
        return await _stationService.OccupancyAsync(id);
    }

    /// <summary>Désactivation d'une station.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Deactivate(long id)
    {
        await _stationService.DeactivateAsync(id);
        return NoContent();
    }
}
