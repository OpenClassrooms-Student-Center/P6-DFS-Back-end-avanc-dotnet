using Microsoft.EntityFrameworkCore;
using NorlysMobilite.Data;
using NorlysMobilite.Models;

namespace NorlysMobilite.Repositories;

/// <summary>
/// Implémentation EF Core de l'accès aux comptes des agents.
/// </summary>
public class AgentRepository : IAgentRepository
{
    private readonly NorlysDbContext _context;

    public AgentRepository(NorlysDbContext context)
    {
        _context = context;
    }

    public async Task<List<Agent>> FindAllAsync()
    {
        return await _context.Agents.ToListAsync();
    }

    public async Task<Agent?> FindByIdAsync(long id)
    {
        return await _context.Agents.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Agent?> FindByUsernameAsync(string username)
    {
        return await _context.Agents.FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Agents.AnyAsync(a => a.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Agents.AnyAsync(a => a.Email == email);
    }

    public async Task<List<Agent>> FindActiveAsync()
    {
        return await _context.Agents.Where(a => a.Active).ToListAsync();
    }

    public async Task<Agent> AddAsync(Agent agent)
    {
        _context.Agents.Add(agent);
        await _context.SaveChangesAsync();
        return agent;
    }

    public async Task<Agent> UpdateAsync(Agent agent)
    {
        await _context.SaveChangesAsync();
        return agent;
    }
}
