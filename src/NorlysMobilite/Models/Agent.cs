namespace NorlysMobilite.Models;

/// <summary>
/// Compte d'un agent de terrain ou d'un superviseur.
/// </summary>
public class Agent
{
    public long Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public AgentRole Role { get; set; }

    public bool Active { get; set; } = true;
}
