using NorlysMobilite.Models;

namespace NorlysMobilite.Services;

/// <summary>
/// Gestion des comptes des agents : création, mot de passe, désactivation.
/// </summary>
public interface IAgentService
{
    Task<List<Agent>> FindAllAsync();

    Task<Agent> FindByIdAsync(long id);

    Task<Agent> FindByUsernameAsync(string username);

    Task<Agent> CreateAsync(Agent agent);

    Task<Agent> ChangePasswordAsync(long id, string? newPassword);

    Task DeactivateAsync(long id);
}
