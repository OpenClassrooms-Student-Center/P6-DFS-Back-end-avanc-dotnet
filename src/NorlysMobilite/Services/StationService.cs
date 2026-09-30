using NorlysMobilite.Exceptions;
using NorlysMobilite.Models;
using NorlysMobilite.Repositories;

namespace NorlysMobilite.Services;

/// <summary>
/// Règles de gestion des stations : capacité, responsable, désactivation, occupation.
/// </summary>
public class StationService : IStationService
{
    private readonly IStationRepository _stationRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAgentRepository _agentRepository;

    public StationService(
        IStationRepository stationRepository,
        IVehicleRepository vehicleRepository,
        IAgentRepository agentRepository)
    {
        _stationRepository = stationRepository;
        _vehicleRepository = vehicleRepository;
        _agentRepository = agentRepository;
    }

    public async Task<List<Station>> FindAllAsync()
    {
        return await _stationRepository.FindAllAsync();
    }

    public async Task<Station> FindByIdAsync(long id)
    {
        return await _stationRepository.FindByIdAsync(id)
               ?? throw new ResourceNotFoundException($"Station introuvable : {id}");
    }

    public async Task<Station> CreateAsync(Station station)
    {
        RequirePositiveCapacity(station.Capacity);

        station.Id = 0;
        station.Active = true;
        station.Manager = null;
        station.ManagerId = null;
        return await _stationRepository.AddAsync(station);
    }

    public async Task<Station> UpdateAsync(long id, Station changes)
    {
        var station = await FindByIdAsync(id);
        RequirePositiveCapacity(changes.Capacity);

        var vehiclesPresent = await _vehicleRepository.CountByStationIdAsync(id);
        if (changes.Capacity < vehiclesPresent)
        {
            throw new BusinessRuleException("La capacité ne peut pas être inférieure au nombre de véhicules présents");
        }

        station.Name = changes.Name;
        station.Address = changes.Address;
        station.Capacity = changes.Capacity;
        station.Latitude = changes.Latitude;
        station.Longitude = changes.Longitude;
        return await _stationRepository.UpdateAsync(station);
    }

    public async Task<Station> AssignManagerAsync(long stationId, long agentId)
    {
        var station = await FindByIdAsync(stationId);
        var agent = await _agentRepository.FindByIdAsync(agentId)
                    ?? throw new ResourceNotFoundException($"Agent introuvable : {agentId}");

        if (!agent.Active)
        {
            throw new BusinessRuleException("Un agent désactivé ne peut pas gérer une station");
        }
        if (agent.Role != AgentRole.Superviseur)
        {
            throw new BusinessRuleException("Seul un superviseur peut gérer une station");
        }

        station.Manager = agent;
        station.ManagerId = agent.Id;
        return await _stationRepository.UpdateAsync(station);
    }

    public async Task DeactivateAsync(long id)
    {
        var station = await FindByIdAsync(id);
        if (await _vehicleRepository.CountByStationIdAsync(id) > 0)
        {
            throw new BusinessRuleException("Une station occupée ne peut pas être désactivée");
        }

        station.Active = false;
        await _stationRepository.UpdateAsync(station);
    }

    public async Task<List<Vehicle>> VehiclesOfAsync(long stationId)
    {
        await FindByIdAsync(stationId);
        return await _vehicleRepository.FindByStationIdAsync(stationId);
    }

    public async Task<StationOccupancy> OccupancyAsync(long stationId)
    {
        var station = await FindByIdAsync(stationId);
        var vehicleCount = await _vehicleRepository.CountByStationIdAsync(stationId);
        var availableSlots = Math.Max(0, station.Capacity - vehicleCount);
        var occupancyRate = Math.Round(vehicleCount / (double)station.Capacity, 2);
        return new StationOccupancy(station.Id, station.Capacity, vehicleCount, availableSlots, occupancyRate);
    }

    public async Task<bool> HasFreeSlotAsync(Station station)
    {
        return await _vehicleRepository.CountByStationIdAsync(station.Id) < station.Capacity;
    }

    private static void RequirePositiveCapacity(int capacity)
    {
        if (capacity <= 0)
        {
            throw new BusinessRuleException("La capacité d'une station doit être strictement positive");
        }
    }
}
