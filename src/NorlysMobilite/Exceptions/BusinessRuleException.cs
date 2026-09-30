namespace NorlysMobilite.Exceptions;

/// <summary>
/// Levée lorsqu'une opération viole une règle de gestion
/// (station pleine, véhicule non disponible, capacité invalide...).
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
