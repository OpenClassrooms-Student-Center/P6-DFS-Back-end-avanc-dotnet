using NorlysMobilite.Models;

namespace NorlysMobilite.Services;

/// <summary>
/// Règles de gestion de la flotte : affectation en station, location, batterie, maintenance, retrait.
/// </summary>
public interface IVehicleService
{
    Task<List<Vehicle>> FindAllAsync();

    Task<Vehicle> FindByIdAsync(long id);

    Task<Vehicle> CreateAsync(Vehicle vehicle);

    Task<Vehicle> AssignToStationAsync(long vehicleId, long stationId);

    Task<Vehicle> StartRentalAsync(long vehicleId);

    Task<Vehicle> EndRentalAsync(long vehicleId, long stationId, int batteryLevel);

    Task<Vehicle> UpdateBatteryAsync(long vehicleId, int level);

    Task<Vehicle> SendToMaintenanceAsync(long vehicleId);

    Task<Vehicle> ReturnFromMaintenanceAsync(long vehicleId);

    Task RetireAsync(long vehicleId);
}
