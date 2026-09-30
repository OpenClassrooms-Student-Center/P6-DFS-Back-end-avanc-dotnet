using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Accès aux comptes des agents.
/// </summary>
public interface IAgentRepository
{
    Task<List<Agent>> FindAllAsync();

    Task<Agent?> FindByIdAsync(long id);

    Task<Agent?> FindByUsernameAsync(string username);

    Task<bool> ExistsByUsernameAsync(string username);

    Task<bool> ExistsByEmailAsync(string email);

    Task<List<Agent>> FindActiveAsync();

    Task<Agent> AddAsync(Agent agent);

    Task<Agent> UpdateAsync(Agent agent);
}
