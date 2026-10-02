using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NorlysMobilite.Data;
using Testcontainers.PostgreSql;

namespace NorlysMobilite.Tests.Integration;

/// <summary>
/// Démarre l'API dans un hôte de test adossé à une base PostgreSQL éphémère fournie par Testcontainers.
/// Le fournisseur et la chaîne de connexion de l'application sont remplacés par ceux du conteneur.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("norlys_tests")
        .WithUsername("norlys")
        .WithPassword("norlys")
        .Build();

    private bool _databaseDisposed;

    /// <summary>Chaîne de connexion de la base éphémère, disponible après <see cref="InitializeAsync"/>.</summary>
    public string ConnectionString => _database.GetConnectionString();

    /// <summary>Démarre le conteneur PostgreSQL avant le premier appel HTTP.</summary>
    public Task InitializeAsync()
    {
        return _database.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Database:Provider", "Postgres");
        builder.UseSetting("ConnectionStrings:Default", ConnectionString);

        // Le fournisseur est fixé à l'enregistrement du DbContext : on remplace l'enregistrement
        // de l'application (SQLite) par PostgreSQL sur la chaîne de connexion du conteneur.
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<NorlysDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<NorlysDbContext>>();
            services.AddDbContext<NorlysDbContext>(options => options.UseNpgsql(ConnectionString));
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await DisposeDatabaseAsync();
        await base.DisposeAsync();
    }

    // IAsyncLifetime (xUnit v2) expose DisposeAsync en Task, alors que WebApplicationFactory
    // l'expose en ValueTask : les deux chemins convergent vers le même nettoyage idempotent.
    async Task IAsyncLifetime.DisposeAsync()
    {
        await DisposeDatabaseAsync();
        await base.DisposeAsync();
    }

    private async Task DisposeDatabaseAsync()
    {
        if (_databaseDisposed)
        {
            return;
        }

        _databaseDisposed = true;
        await _database.DisposeAsync();
    }
}
