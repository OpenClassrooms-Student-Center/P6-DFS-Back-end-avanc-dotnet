namespace NorlysMobilite.Services;

/// <summary>
/// Vue calculée de l'occupation d'une station à un instant donné.
/// </summary>
/// <param name="StationId">Identifiant de la station.</param>
/// <param name="Capacity">Nombre de places de la station.</param>
/// <param name="VehicleCount">Nombre de véhicules actuellement rattachés.</param>
/// <param name="AvailableSlots">Places restantes (jamais négatif).</param>
/// <param name="OccupancyRate">Ratio véhicules / capacité, arrondi à deux décimales.</param>
public record StationOccupancy(
    long StationId,
    int Capacity,
    int VehicleCount,
    int AvailableSlots,
    double OccupancyRate);
