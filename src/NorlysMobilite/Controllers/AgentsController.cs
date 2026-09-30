using Microsoft.AspNetCore.Mvc;
using NorlysMobilite.Models;
using NorlysMobilite.Services;

namespace NorlysMobilite.Controllers;

/// <summary>
/// Comptes des agents de terrain et des superviseurs.
/// </summary>
[ApiController]
[Route("api/agents")]
[Produces("application/json")]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    /// <summary>Liste des agents.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Agent>>> GetAll()
    {
        return await _agentService.FindAllAsync();
    }

    /// <summary>Détail d'un agent.</summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Agent>> GetById(long id)
    {
        return await _agentService.FindByIdAsync(id);
    }

    /// <summary>Création d'un agent.</summary>
    [HttpPost]
    public async Task<ActionResult<Agent>> Create([FromBody] Agent agent)
    {
        var created = await _agentService.CreateAsync(agent);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Changement du mot de passe d'un agent.</summary>
    [HttpPut("{id:long}/password")]
    public async Task<ActionResult<Agent>> ChangePassword(long id, [FromBody] PasswordChangeRequest request)
    {
        return await _agentService.ChangePasswordAsync(id, request.Password);
    }

    /// <summary>Désactivation d'un agent.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Deactivate(long id)
    {
        await _agentService.DeactivateAsync(id);
        return NoContent();
    }
}
