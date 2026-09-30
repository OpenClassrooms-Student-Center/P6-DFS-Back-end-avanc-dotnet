namespace NorlysMobilite.Models;

/// <summary>
/// Station de la flotte : emplacement, capacité et responsable.
/// </summary>
public class Station
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public int Capacity { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool Active { get; set; } = true;

    public long? ManagerId { get; set; }

    public Agent? Manager { get; set; }
}
