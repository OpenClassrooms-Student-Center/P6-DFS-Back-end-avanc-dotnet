using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Accès aux véhicules de la flotte, dont le comptage par station.
/// </summary>
public interface IVehicleRepository
{
    Task<List<Vehicle>> FindAllAsync();

    Task<Vehicle?> FindByIdAsync(long id);

    Task<List<Vehicle>> FindByStationIdAsync(long stationId);

    Task<int> CountByStationIdAsync(long stationId);

    Task<List<Vehicle>> FindByStatusAsync(VehicleStatus status);

    Task<Vehicle?> FindBySerialNumberAsync(string serialNumber);

    Task<bool> ExistsBySerialNumberAsync(string serialNumber);

    Task<Vehicle> AddAsync(Vehicle vehicle);

    Task<Vehicle> UpdateAsync(Vehicle vehicle);
}
