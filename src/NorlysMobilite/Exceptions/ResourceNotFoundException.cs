namespace NorlysMobilite.Exceptions;

/// <summary>
/// Levée lorsqu'une ressource demandée (station, véhicule, agent) n'existe pas.
/// </summary>
public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }
}
