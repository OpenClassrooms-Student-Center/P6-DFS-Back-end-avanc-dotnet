using Microsoft.EntityFrameworkCore;
using NorlysMobilite.Data;
using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Implémentation EF Core de l'accès aux véhicules.
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    private readonly NorlysDbContext _context;

    public VehicleRepository(NorlysDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> FindAllAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task<Vehicle?> FindByIdAsync(long id)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Vehicle>> FindByStationIdAsync(long stationId)
    {
        return await _context.Vehicles.Where(v => v.StationId == stationId).ToListAsync();
    }

    public async Task<int> CountByStationIdAsync(long stationId)
    {
        return await _context.Vehicles.CountAsync(v => v.StationId == stationId);
    }

    public async Task<List<Vehicle>> FindByStatusAsync(VehicleStatus status)
    {
        return await _context.Vehicles.Where(v => v.Status == status).ToListAsync();
    }

    public async Task<Vehicle?> FindBySerialNumberAsync(string serialNumber)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.SerialNumber == serialNumber);
    }

    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber)
    {
        return await _context.Vehicles.AnyAsync(v => v.SerialNumber == serialNumber);
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
    {
        await _context.SaveChangesAsync();
        return vehicle;
    }
}
