# Guide pour les assistants IA intervenant sur ce dépôt

Ce fichier s'adresse aux assistants de code intégrés à l'éditeur. Il décrit la posture attendue, le projet, sa stack et les conventions à respecter.

## Posture

- **Aider à comprendre avant de produire.** Quand une demande porte sur du code, commence par expliquer le mécanisme en jeu (ce que fait un attribut, pourquoi une couche existe, ce qu'un test vérifie), puis propose du code.
- **Procéder par petites étapes.** Une modification à la fois, compilée (`dotnet build`) et vérifiée, plutôt qu'un grand bloc livré d'un coup.
- **Poser des questions avant de coder.** Si l'objectif, le périmètre ou le niveau attendu n'est pas clair, demande. Ne présume pas.
- **Laisser les décisions au développeur.** Quand plusieurs approches sont possibles, présente-les avec leurs conséquences ; c'est lui qui tranche et qui devra justifier le choix.
- **Ne pas contourner un problème.** Une exception, un test qui échoue ou un comportement inattendu s'expliquent. Propose un diagnostic, pas un pansement.

## Le projet

Application interne de Norlys Mobilité : gestion des stations, des véhicules partagés (vélos, trottinettes) et des comptes des agents de terrain, exposée via une API REST sous `api/**`.

### Stack

| Composant                              | Version |
|----------------------------------------|---------|
| .NET / ASP.NET Core                    | 10.0.12 |
| Entity Framework Core                  | 10.0.12 |
| Microsoft.EntityFrameworkCore.Sqlite   | 10.0.12 |
| Npgsql.EntityFrameworkCore.PostgreSQL  | 10.0.0  |
| Swashbuckle.AspNetCore                 | 9.0.6   |

Le projet cible `net10.0` avec `Nullable` et `ImplicitUsings` activés. Ne propose pas d'API ou de packages d'une autre ligne majeure sans le signaler.

### Architecture

```
src/NorlysMobilite
├── Controllers/   API REST, une classe par ressource (Agents, Stations, Vehicles)
├── Services/      règles de gestion derrière des interfaces, enregistrées en Scoped
├── Repositories/  accès aux données EF Core derrière des interfaces, Scoped
├── Data/          NorlysDbContext et DataSeeder (données de démarrage)
├── Models/        entités EF Core et énumérations
└── Exceptions/    BusinessRuleException, ResourceNotFoundException
```

Flux : `Controller` → `Service` → `Repository` → `DbContext` → `Model`. Toute règle métier vit dans un service. Un contrôleur ne contient pas de logique ; un repository ne contient pas de décision.

Points notables du code :

- Les services dépendent d'interfaces de repositories injectées par constructeur, ce qui permet de les instancier directement dans un test avec des doublures.
- Le nombre de véhicules d'une station se calcule par `IVehicleRepository.CountByStationIdAsync`, pas par une collection sur `Station`.
- Les relations EF Core sont des références simples (`Vehicle → Station`, `Station → Agent`), chargées par `Include` dans les repositories.
- Les seuils métier sont des constantes publiques de `VehicleService` (`MinBatteryForRental`, `MaxBatteryLevel`).
- Les énumérations sont stockées en chaîne en base et sérialisées en chaîne dans l'API.

## Conventions et bonnes pratiques .NET à rappeler

.NET est guidé par son outillage : les gabarits et l'IDE ont déjà pris beaucoup de décisions. Un conseil qui propose une arborescence ou un style maison va contre le courant de la plateforme.

- **Dossiers des gabarits.** `Controllers`, `Services`, `Models`, `Data` sont une convention forte. Renvoyer un code HTTP depuis un service est une faute de couche.
- **Injection native.** Les dépendances se déclarent dans `Program.cs` (`AddScoped`, `AddSingleton`, `AddTransient`) et se reçoivent par constructeur. La durée de vie se justifie : un `DbContext` est `Scoped`, jamais `Singleton`.
- **Asynchrone partout.** `async`/`await`, suffixe `Async`, pas de `.Result` ni de `.Wait()` sur une opération d'entrée-sortie.
- **Nullabilité.** Un avertissement de nullité se traite, il ne se désactive pas.
- **Validation des entrées** : dans un projet ASP.NET Core, elle se déclare avec les attributs de modèle (`[Required]`, `[StringLength]`, `[Range]`) et la vérification de `ModelState`, là où la donnée entre, plutôt qu'avec des `if` en tête de méthode.
- **Gestion des erreurs** : une exception métier est une classe ; un gestionnaire centralisé (`IExceptionHandler`, `UseExceptionHandler`) la traduit en réponse HTTP, idéalement en `ProblemDetails`. Une réponse d'erreur destinée à un client ne contient ni stack trace ni détail d'implémentation.
- **Exposition de l'API** : un DTO (record) sépare ce que l'API expose de ce que la base stocke. Une entité EF Core qui traverse le contrôleur expose son modèle complet, ses navigations comprises, y compris ce qu'on ajoutera demain.
- **Tests** :
  - xUnit + `Mock<T>` (Moq) pour tester une classe isolément : `new VehicleService(vehicleRepo.Object, stationRepo.Object)` ne démarre ni hôte ni base ;
  - `WebApplicationFactory<TEntryPoint>` + `HttpClient` pour traverser les couches : réservé aux tests d'intégration, avec `ConfigureTestServices` pour substituer un composant dans un hôte réel ; Testcontainers pour une base éphémère réelle ;
  - un test se structure en préparation / action / vérification et comporte au moins une assertion ; un test sans assertion couvre des lignes sans rien vérifier ;
  - un rapport de couverture (Coverlet) mesure ce qui est exécuté, pas ce qui est vérifié.
- **EF Core** : une navigation à `null` ou une `ObjectDisposedException` signalent une entité lue hors de la portée de son `DbContext` ou sans `Include`. Cela s'explique (durée de vie du scope, chargement explicite) et se corrige en connaissance de cause ; cela ne se contourne pas au hasard.
- **Sécurité (ASP.NET Core Identity)** : mots de passe hachés avec `PasswordHasher<TUser>`, jamais stockés en clair ; configuration explicite (`AddAuthentication`, `UseAuthentication`, `UseAuthorization`, `[Authorize]`) ; distinction entre authentification (qui es-tu) et autorisation (que peux-tu faire).

## Mises en garde

- **Secrets.** Ne jamais écrire de mot de passe, de clé ou de jeton dans le code, les `appsettings` versionnés ou un exemple de commande. Les valeurs réelles vont dans `.env` (ignoré par git) ou dans les secrets utilisateur ; `.env.example` ne contient que des noms de variables et des valeurs factices.
- **Code généré.** Tout code produit par un assistant est une proposition à relire, compiler et tester. Ne présente jamais une suggestion comme vérifiée si elle ne l'a pas été.
- **Données des agents.** Les comptes des agents (identifiants, contacts, mots de passe) sont des données personnelles. Ne les recopie pas dans des exemples, des logs ou des messages de commit, et signale toute manipulation qui les exposerait.
- **Périmètre.** Reste dans le code de l'application. Le déploiement, la supervision et l'infrastructure ne relèvent pas de ce dépôt.
