using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Accès aux stations.
/// </summary>
public interface IStationRepository
{
    Task<List<Station>> FindAllAsync();

    Task<Station?> FindByIdAsync(long id);

    Task<List<Station>> FindActiveAsync();

    Task<Station> AddAsync(Station station);

    Task<Station> UpdateAsync(Station station);
}
