using NorlysMobilite.Exceptions;
using NorlysMobilite.Models;
using NorlysMobilite.Repositories;

namespace NorlysMobilite.Services;

/// <summary>
/// Gestion des comptes des agents de terrain : création, mot de passe, désactivation.
/// </summary>
public class AgentService : IAgentService
{
    private readonly IAgentRepository _agentRepository;

    public AgentService(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task<List<Agent>> FindAllAsync()
    {
        return await _agentRepository.FindAllAsync();
    }

    public async Task<Agent> FindByIdAsync(long id)
    {
        return await _agentRepository.FindByIdAsync(id)
               ?? throw new ResourceNotFoundException($"Agent introuvable : {id}");
    }

    public async Task<Agent> FindByUsernameAsync(string username)
    {
        return await _agentRepository.FindByUsernameAsync(username)
               ?? throw new ResourceNotFoundException($"Agent introuvable : {username}");
    }

    public async Task<Agent> CreateAsync(Agent agent)
    {
        if (string.IsNullOrWhiteSpace(agent.Username))
        {
            throw new BusinessRuleException("L'identifiant est obligatoire");
        }
        if (await _agentRepository.ExistsByUsernameAsync(agent.Username))
        {
            throw new BusinessRuleException($"Identifiant déjà utilisé : {agent.Username}");
        }
        if (agent.Email != null && await _agentRepository.ExistsByEmailAsync(agent.Email))
        {
            throw new BusinessRuleException($"Adresse e-mail déjà utilisée : {agent.Email}");
        }

        agent.Id = 0;
        agent.Active = true;
        return await _agentRepository.AddAsync(agent);
    }

    public async Task<Agent> ChangePasswordAsync(long id, string? newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            throw new BusinessRuleException("Le mot de passe est obligatoire");
        }

        var agent = await FindByIdAsync(id);
        agent.Password = newPassword;
        return await _agentRepository.UpdateAsync(agent);
    }

    public async Task DeactivateAsync(long id)
    {
        var agent = await FindByIdAsync(id);
        agent.Active = false;
        await _agentRepository.UpdateAsync(agent);
    }
}
