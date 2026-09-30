namespace NorlysMobilite.Models;

/// <summary>
/// Véhicule partagé (vélo ou trottinette) de la flotte.
/// </summary>
public class Vehicle
{
    public long Id { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public VehicleType Type { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Available;

    public int BatteryLevel { get; set; } = 100;

    public long? StationId { get; set; }

    public Station? Station { get; set; }

    public DateTime? LastMaintenanceAt { get; set; }
}
