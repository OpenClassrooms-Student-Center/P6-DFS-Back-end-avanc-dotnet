using NorlysMobilite.Models;

namespace NorlysMobilite.Services;

/// <summary>
/// Règles de gestion des stations : capacité, responsable, désactivation, occupation.
/// </summary>
public interface IStationService
{
    Task<List<Station>> FindAllAsync();

    Task<Station> FindByIdAsync(long id);

    Task<Station> CreateAsync(Station station);

    Task<Station> UpdateAsync(long id, Station changes);

    Task<Station> AssignManagerAsync(long stationId, long agentId);

    Task DeactivateAsync(long id);

    Task<List<Vehicle>> VehiclesOfAsync(long stationId);

    Task<StationOccupancy> OccupancyAsync(long stationId);

    Task<bool> HasFreeSlotAsync(Station station);
}
