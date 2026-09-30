namespace NorlysMobilite.Models;

/// <summary>
/// Cycle de vie d'un véhicule de la flotte.
/// Available    : en station, louable.
/// InUse        : en cours de location, hors station.
/// LowBattery   : en station, batterie sous le seuil de location.
/// Maintenance  : immobilisé pour intervention technique.
/// OutOfService : retiré définitivement de la flotte.
/// </summary>
public enum VehicleStatus
{
    Available,
    InUse,
    LowBattery,
    Maintenance,
    OutOfService
}
