using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NorlysMobilite.Data;
using NorlysMobilite.Repositories;
using NorlysMobilite.Services;

var builder = WebApplication.CreateBuilder(args);

// API
builder.Services
    .AddControllers(options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Documentation d'API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Norlys Mobilité — Gestion de flotte",
        Version = "1.4.2",
        Description = "API interne de gestion des stations, des véhicules partagés et des comptes des agents de terrain."
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{builder.Environment.ApplicationName}.xml");
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Persistance : le fournisseur est choisi par la configuration (Database:Provider = Sqlite | Postgres)
var provider = builder.Configuration["Database:Provider"] ?? "Sqlite";
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=norlys.db";

builder.Services.AddDbContext<NorlysDbContext>(options =>
{
    if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

// Accès aux données et règles de gestion : une instance par requête HTTP, comme le DbContext
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Création du schéma et insertion des données de démarrage si la base est vide
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NorlysDbContext>();
    await context.Database.EnsureCreatedAsync();
    await DataSeeder.SeedAsync(context);
}

app.Run();

/// <summary>
/// Point d'entrée de l'application, exposé publiquement pour les tests d'intégration
/// qui démarrent l'hôte via <c>WebApplicationFactory&lt;Program&gt;</c>.
/// </summary>
public partial class Program
{
}
