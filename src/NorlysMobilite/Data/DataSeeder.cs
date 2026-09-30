using Microsoft.EntityFrameworkCore;
using NorlysMobilite.Models;

namespace NorlysMobilite.Data;

/// <summary>
/// Insère un jeu de données de démarrage lorsque la base est vide,
/// afin que l'application soit utilisable dès le premier lancement.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(NorlysDbContext context)
    {
        if (await context.Agents.AnyAsync())
        {
            return;
        }

        var tverdier = Agent("tverdier", "lead2024", "Thomas Verdier",
            "thomas.verdier@norlys-mobilite.fr", "06 12 34 56 78", AgentRole.Superviseur, true);
        var mlefranc = Agent("mlefranc", "stationA", "Marion Lefranc",
            "marion.lefranc@norlys-mobilite.fr", "06 23 45 67 89", AgentRole.Superviseur, true);
        var kdiallo = Agent("kdiallo", "velo123", "Karim Diallo",
            "karim.diallo@norlys-mobilite.fr", "06 34 56 78 90", AgentRole.Terrain, true);
        var sbernard = Agent("sbernard", "trott2023", "Sophie Bernard",
            "sophie.bernard@norlys-mobilite.fr", "06 45 67 89 01", AgentRole.Terrain, false);

        context.Agents.AddRange(tverdier, mlefranc, kdiallo, sbernard);
        await context.SaveChangesAsync();

        var gareCentrale = Station("Gare Centrale", "1 place de la Gare", 20, 48.8412, 2.3218, tverdier);
        var placeDuMarche = Station("Place du Marché", "14 rue des Halles", 12, 48.8455, 2.3301, mlefranc);
        var campusNord = Station("Campus Nord", "Avenue de l'Université", 6, 48.8620, 2.3410, null);

        context.Stations.AddRange(gareCentrale, placeDuMarche, campusNord);
        await context.SaveChangesAsync();

        context.Vehicles.AddRange(
            Vehicle("NRL-B-001", VehicleType.Bike, 92, VehicleStatus.Available, gareCentrale),
            Vehicle("NRL-B-002", VehicleType.Bike, 15, VehicleStatus.LowBattery, gareCentrale),
            Vehicle("NRL-B-003", VehicleType.Bike, 60, VehicleStatus.Maintenance, gareCentrale),
            Vehicle("NRL-S-001", VehicleType.Scooter, 80, VehicleStatus.Available, placeDuMarche),
            Vehicle("NRL-S-002", VehicleType.Scooter, 45, VehicleStatus.InUse, null),
            Vehicle("NRL-S-003", VehicleType.Scooter, 100, VehicleStatus.Available, campusNord),
            Vehicle("NRL-S-004", VehicleType.Scooter, 33, VehicleStatus.Available, campusNord),
            Vehicle("NRL-B-004", VehicleType.Bike, 0, VehicleStatus.OutOfService, null));
        await context.SaveChangesAsync();
    }

    private static Agent Agent(string username, string password, string fullName, string email,
        string phone, AgentRole role, bool active)
    {
        return new Agent
        {
            Username = username,
            Password = password,
            FullName = fullName,
            Email = email,
            Phone = phone,
            Role = role,
            Active = active
        };
    }

    private static Station Station(string name, string address, int capacity,
        double latitude, double longitude, Agent? manager)
    {
        return new Station
        {
            Name = name,
            Address = address,
            Capacity = capacity,
            Latitude = latitude,
            Longitude = longitude,
            Active = true,
            Manager = manager,
            ManagerId = manager?.Id
        };
    }

    private static Vehicle Vehicle(string serialNumber, VehicleType type, int batteryLevel,
        VehicleStatus status, Station? station)
    {
        return new Vehicle
        {
            SerialNumber = serialNumber,
            Type = type,
            BatteryLevel = batteryLevel,
            Status = status,
            Station = station,
            StationId = station?.Id
        };
    }
}
