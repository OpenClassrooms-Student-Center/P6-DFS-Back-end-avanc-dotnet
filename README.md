# Norlys Mobilité — Gestion de flotte

Application interne de Norlys Mobilité pour la gestion des stations, des véhicules partagés (vélos et trottinettes) et des comptes des agents de terrain. Elle expose une API REST consommée par les outils de terrain et le back-office.

Version courante : `1.4.2`

## Prérequis

- SDK .NET 10.0 (10.0.12 ou supérieur dans la ligne 10.0)
- PostgreSQL 16 (uniquement pour le fournisseur `Postgres` ; le développement local utilise SQLite, sans installation)

## Installation

```bash
git clone <url-du-depot> norlys-mobilite
cd norlys-mobilite
dotnet restore
dotnet build
```

## Lancement

### Développement (SQLite)

```bash
dotnet run --project src/NorlysMobilite
```

L'application démarre sur `http://localhost:8080`. Au premier lancement, le schéma est créé dans le fichier `src/NorlysMobilite/norlys.db` et un jeu de données de démarrage (agents, stations, véhicules) est inséré si la base est vide. Pour repartir de zéro, supprimez ce fichier et relancez l'application.

- Swagger UI : `http://localhost:8080/swagger`
- Spécification OpenAPI : `http://localhost:8080/swagger/v1/swagger.json`

### Fournisseur PostgreSQL

Le fournisseur de base et la chaîne de connexion se surchargent par variables d'environnement (voir `.env.example`). Copier `.env.example` vers `.env`, renseigner les valeurs, exporter les variables dans l'environnement, puis :

```bash
dotnet run --project src/NorlysMobilite --environment Production
```

Variables reconnues :

| Variable                     | Rôle                                          | Exemple                                                                    |
|------------------------------|-----------------------------------------------|----------------------------------------------------------------------------|
| `ASPNETCORE_ENVIRONMENT`     | Environnement d'exécution                     | `Development`                                                              |
| `ASPNETCORE_URLS`            | Adresse d'écoute HTTP                         | `http://localhost:8080`                                                    |
| `Database__Provider`         | Fournisseur de base : `Sqlite` ou `Postgres`  | `Sqlite`                                                                   |
| `ConnectionStrings__Default` | Chaîne de connexion du fournisseur choisi     | `Host=localhost;Port=5432;Database=norlys;Username=norlys;Password=…`      |

## Stack technique

| Composant                        | Version |
|----------------------------------|---------|
| .NET / ASP.NET Core              | 10.0.12 |
| Entity Framework Core            | 10.0.12 |
| EF Core SQLite (développement)   | 10.0.12 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.0 |
| Swashbuckle.AspNetCore           | 9.0.6   |

## Structure du projet

```
NorlysMobilite.sln
src/NorlysMobilite
├── Program.cs                     hôte ASP.NET Core : injection, Swagger, base de données
├── NorlysMobilite.csproj
├── appsettings.json               configuration par défaut (SQLite)
├── appsettings.Development.json   journalisation en développement
├── Properties/launchSettings.json profil de lancement (port 8080)
├── Controllers/                   API REST (api/**)
│   ├── AgentsController.cs
│   ├── StationsController.cs
│   └── VehiclesController.cs
├── Services/                      règles de gestion
│   ├── IAgentService.cs, AgentService.cs
│   ├── IStationService.cs, StationService.cs
│   ├── IVehicleService.cs, VehicleService.cs
│   └── StationOccupancy.cs
├── Repositories/                  accès aux données (EF Core)
│   ├── IAgentRepository.cs, AgentRepository.cs
│   ├── IStationRepository.cs, StationRepository.cs
│   └── IVehicleRepository.cs, VehicleRepository.cs
├── Data/
│   ├── NorlysDbContext.cs         contexte EF Core
│   └── DataSeeder.cs              jeu de données de démarrage
├── Models/                        entités et énumérations
│   ├── Agent.cs, AgentRole.cs
│   ├── Station.cs
│   ├── Vehicle.cs, VehicleType.cs, VehicleStatus.cs
│   └── PasswordChangeRequest.cs
└── Exceptions/                    exceptions métier
    ├── BusinessRuleException.cs
    └── ResourceNotFoundException.cs
```

L'application suit une architecture en couches : `Controllers` → `Services` → `Repositories` → `Data` → `Models`. Les règles de gestion vivent exclusivement dans les services, qui reçoivent leurs dépendances par constructeur via l'injection native d'ASP.NET Core (durée de vie `Scoped`, alignée sur celle du `DbContext`).

## Règles de gestion principales

- Une station a une capacité strictement positive ; elle ne peut pas être réduite sous le nombre de véhicules présents, ni désactivée tant qu'elle contient des véhicules.
- Seul un agent actif ayant le rôle `Superviseur` peut être désigné responsable d'une station.
- Un véhicule est louable s'il est `Available`, rattaché à une station et que sa batterie est au moins à 20 %. En dessous de ce seuil il passe en `LowBattery`.
- Un véhicule ne peut être affecté qu'à une station active et non pleine ; un véhicule `InUse` ou `OutOfService` ne peut pas être affecté.
- Le retour de location fixe le niveau de batterie et recalcule le statut (`Available` ou `LowBattery`).

## Points d'entrée de l'API

Les énumérations sont échangées sous forme de chaînes (`Bike`, `Scooter`, `Available`, `Superviseur`…).

### Agents — `api/agents`

| Méthode | Chemin              | Description                        |
|---------|---------------------|------------------------------------|
| GET     | `/`                 | Liste des agents                   |
| GET     | `/{id}`             | Détail d'un agent                  |
| POST    | `/`                 | Création d'un agent                |
| PUT     | `/{id}/password`    | Changement de mot de passe         |
| DELETE  | `/{id}`             | Désactivation d'un agent           |

### Stations — `api/stations`

| Méthode | Chemin                         | Description                                       |
|---------|--------------------------------|---------------------------------------------------|
| GET     | `/`                            | Liste des stations                                |
| GET     | `/{id}`                        | Détail d'une station                              |
| POST    | `/`                            | Création d'une station                            |
| PUT     | `/{id}`                        | Mise à jour (nom, adresse, capacité, coordonnées) |
| PUT     | `/{id}/manager/{agentId}`      | Désignation du responsable                        |
| GET     | `/{id}/vehicles`               | Véhicules présents dans la station                |
| GET     | `/{id}/occupancy`              | Occupation (places libres, taux)                  |
| DELETE  | `/{id}`                        | Désactivation d'une station                       |

### Véhicules — `api/vehicles`

| Méthode | Chemin                                              | Description                        |
|---------|-----------------------------------------------------|------------------------------------|
| GET     | `/`                                                 | Liste des véhicules                |
| GET     | `/{id}`                                             | Détail d'un véhicule               |
| POST    | `/`                                                 | Ajout d'un véhicule à la flotte    |
| PUT     | `/{id}/station/{stationId}`                         | Affectation à une station          |
| POST    | `/{id}/rental/start`                                | Début de location                  |
| POST    | `/{id}/rental/end?stationId=&batteryLevel=`         | Fin de location                    |
| PUT     | `/{id}/battery?level=`                              | Mise à jour du niveau de batterie  |
| POST    | `/{id}/maintenance`                                 | Envoi en maintenance               |
| POST    | `/{id}/maintenance/return`                          | Retour de maintenance              |
| DELETE  | `/{id}`                                             | Retrait définitif de la flotte     |

La description complète des corps de requête et de réponse est disponible dans Swagger UI.

## Commandes

| Commande                                                        | Effet                                                  |
|-----------------------------------------------------------------|--------------------------------------------------------|
| `dotnet restore`                                                | Restaure les packages NuGet                            |
| `dotnet build`                                                  | Compile la solution                                    |
| `dotnet run --project src/NorlysMobilite`                       | Lance l'application avec SQLite                        |
| `dotnet run --project src/NorlysMobilite --environment Production` | Lance l'application avec la configuration de l'environnement |
| `dotnet test`                                                   | Exécute les projets de tests de la solution            |
| `dotnet publish src/NorlysMobilite -c Release -o publish`       | Produit le livrable dans `publish/`                    |

## Licence

Logiciel propriétaire — usage interne Norlys Mobilité. Tous droits réservés.
