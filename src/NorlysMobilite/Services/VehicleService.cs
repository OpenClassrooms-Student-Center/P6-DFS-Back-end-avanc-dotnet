using NorlysMobilite.Exceptions;
using NorlysMobilite.Models;
using NorlysMobilite.Repositories;

namespace NorlysMobilite.Services;

/// <summary>
/// Règles de gestion de la flotte : affectation en station, location, batterie, maintenance, retrait.
/// </summary>
public class VehicleService : IVehicleService
{
    /// <summary>Niveau de batterie minimal pour qu'un véhicule soit louable.</summary>
    public const int MinBatteryForRental = 20;

    /// <summary>Niveau de batterie maximal.</summary>
    public const int MaxBatteryLevel = 100;

    private readonly IVehicleRepository _vehicleRepository;
    private readonly IStationRepository _stationRepository;

    public VehicleService(IVehicleRepository vehicleRepository, IStationRepository stationRepository)
    {
        _vehicleRepository = vehicleRepository;
        _stationRepository = stationRepository;
    }

    public async Task<List<Vehicle>> FindAllAsync()
    {
        return await _vehicleRepository.FindAllAsync();
    }

    public async Task<Vehicle> FindByIdAsync(long id)
    {
        return await _vehicleRepository.FindByIdAsync(id)
               ?? throw new ResourceNotFoundException($"Véhicule introuvable : {id}");
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle)
    {
        if (await _vehicleRepository.ExistsBySerialNumberAsync(vehicle.SerialNumber))
        {
            throw new BusinessRuleException($"Numéro de série déjà utilisé : {vehicle.SerialNumber}");
        }
        RequireValidBattery(vehicle.BatteryLevel);

        vehicle.Id = 0;
        vehicle.Station = null;
        vehicle.StationId = null;
        vehicle.Status = StatusForBattery(vehicle.BatteryLevel);
        return await _vehicleRepository.AddAsync(vehicle);
    }

    public async Task<Vehicle> AssignToStationAsync(long vehicleId, long stationId)
    {
        var vehicle = await FindByIdAsync(vehicleId);
        var station = await FindStationAsync(stationId);

        RequireActiveStation(station);
        if (vehicle.Status == VehicleStatus.InUse)
        {
            throw new BusinessRuleException("Un véhicule en cours d'utilisation ne peut pas être affecté");
        }
        if (vehicle.Status == VehicleStatus.OutOfService)
        {
            throw new BusinessRuleException("Un véhicule retiré ne peut pas être affecté");
        }
        await RequireFreeSlotAsync(station);

        vehicle.Station = station;
        vehicle.StationId = station.Id;
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle> StartRentalAsync(long vehicleId)
    {
        var vehicle = await FindByIdAsync(vehicleId);

        if (vehicle.Status != VehicleStatus.Available)
        {
            throw new BusinessRuleException("Le véhicule n'est pas disponible");
        }
        if (vehicle.StationId == null)
        {
            throw new BusinessRuleException("Le véhicule n'est rattaché à aucune station");
        }
        if (vehicle.BatteryLevel < MinBatteryForRental)
        {
            throw new BusinessRuleException("Batterie insuffisante pour une location");
        }

        vehicle.Status = VehicleStatus.InUse;
        vehicle.Station = null;
        vehicle.StationId = null;
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle> EndRentalAsync(long vehicleId, long stationId, int batteryLevel)
    {
        var vehicle = await FindByIdAsync(vehicleId);

        if (vehicle.Status != VehicleStatus.InUse)
        {
            throw new BusinessRuleException("Le véhicule n'est pas en cours d'utilisation");
        }
        RequireValidBattery(batteryLevel);

        var station = await FindStationAsync(stationId);
        RequireActiveStation(station);
        await RequireFreeSlotAsync(station);

        vehicle.Station = station;
        vehicle.StationId = station.Id;
        vehicle.BatteryLevel = batteryLevel;
        vehicle.Status = StatusForBattery(batteryLevel);
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle> UpdateBatteryAsync(long vehicleId, int level)
    {
        var vehicle = await FindByIdAsync(vehicleId);
        RequireValidBattery(level);

        vehicle.BatteryLevel = level;
        if (vehicle.Status == VehicleStatus.LowBattery && level >= MinBatteryForRental)
        {
            vehicle.Status = VehicleStatus.Available;
        }
        else if (vehicle.Status == VehicleStatus.Available && level < MinBatteryForRental)
        {
            vehicle.Status = VehicleStatus.LowBattery;
        }
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle> SendToMaintenanceAsync(long vehicleId)
    {
        var vehicle = await FindByIdAsync(vehicleId);

        if (vehicle.Status == VehicleStatus.InUse)
        {
            throw new BusinessRuleException("Un véhicule en cours d'utilisation ne peut pas partir en maintenance");
        }
        if (vehicle.Status == VehicleStatus.OutOfService)
        {
            throw new BusinessRuleException("Un véhicule retiré ne peut pas partir en maintenance");
        }

        vehicle.Status = VehicleStatus.Maintenance;
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task<Vehicle> ReturnFromMaintenanceAsync(long vehicleId)
    {
        var vehicle = await FindByIdAsync(vehicleId);

        if (vehicle.Status != VehicleStatus.Maintenance)
        {
            throw new BusinessRuleException("Le véhicule n'est pas en maintenance");
        }

        vehicle.LastMaintenanceAt = DateTime.UtcNow;
        vehicle.Status = StatusForBattery(vehicle.BatteryLevel);
        return await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task RetireAsync(long vehicleId)
    {
        var vehicle = await FindByIdAsync(vehicleId);

        if (vehicle.Status == VehicleStatus.InUse)
        {
            throw new BusinessRuleException("Un véhicule en cours d'utilisation ne peut pas être retiré");
        }

        vehicle.Status = VehicleStatus.OutOfService;
        vehicle.Station = null;
        vehicle.StationId = null;
        await _vehicleRepository.UpdateAsync(vehicle);
    }

    private async Task<Station> FindStationAsync(long stationId)
    {
        return await _stationRepository.FindByIdAsync(stationId)
               ?? throw new ResourceNotFoundException($"Station introuvable : {stationId}");
    }

    private static void RequireActiveStation(Station station)
    {
        if (!station.Active)
        {
            throw new BusinessRuleException("La station est désactivée");
        }
    }

    private async Task RequireFreeSlotAsync(Station station)
    {
        if (await _vehicleRepository.CountByStationIdAsync(station.Id) >= station.Capacity)
        {
            throw new BusinessRuleException("La station est pleine");
        }
    }

    private static void RequireValidBattery(int level)
    {
        if (level < 0 || level > MaxBatteryLevel)
        {
            throw new BusinessRuleException("Le niveau de batterie doit être compris entre 0 et 100");
        }
    }

    private static VehicleStatus StatusForBattery(int level)
    {
        return level < MinBatteryForRental ? VehicleStatus.LowBattery : VehicleStatus.Available;
    }
}
