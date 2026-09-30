using Microsoft.EntityFrameworkCore;
using NorlysMobilite.Data;
using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Implémentation EF Core de l'accès aux stations. Les lectures chargent le responsable.
/// </summary>
public class StationRepository : IStationRepository
{
    private readonly NorlysDbContext _context;

    public StationRepository(NorlysDbContext context)
    {
        _context = context;
    }

    public async Task<List<Station>> FindAllAsync()
    {
        return await WithNavigations().ToListAsync();
    }

    public async Task<Station?> FindByIdAsync(long id)
    {
        return await WithNavigations().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Station>> FindActiveAsync()
    {
        return await WithNavigations().Where(s => s.Active).ToListAsync();
    }

    public async Task<Station> AddAsync(Station station)
    {
        _context.Stations.Add(station);
        await _context.SaveChangesAsync();
        return station;
    }

    public async Task<Station> UpdateAsync(Station station)
    {
        await _context.SaveChangesAsync();
        return station;
    }

    private IQueryable<Station> WithNavigations()
    {
        return _context.Stations.Include(s => s.Manager);
    }
}
