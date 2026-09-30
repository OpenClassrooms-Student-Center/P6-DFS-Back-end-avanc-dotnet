using Microsoft.EntityFrameworkCore;
using NorlysMobilite.Models;

namespace NorlysMobilite.Data;

/// <summary>
/// Contexte EF Core de l'application : agents, stations et véhicules.
/// </summary>
public class NorlysDbContext : DbContext
{
    public NorlysDbContext(DbContextOptions<NorlysDbContext> options) : base(options)
    {
    }

    public DbSet<Agent> Agents => Set<Agent>();

    public DbSet<Station> Stations => Set<Station>();

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.ToTable("agents");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Username).IsRequired();
            entity.HasIndex(a => a.Username).IsUnique();
            entity.Property(a => a.Password).IsRequired();
            entity.HasIndex(a => a.Email)
                .IsUnique()
                .HasFilter("\"Email\" IS NOT NULL");
            entity.Property(a => a.Role)
                .IsRequired()
                .HasConversion<string>();
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.ToTable("stations");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired();
            entity.HasOne(s => s.Manager)
                .WithMany()
                .HasForeignKey(s => s.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("vehicles");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.SerialNumber).IsRequired();
            entity.HasIndex(v => v.SerialNumber).IsUnique();
            entity.Property(v => v.Type)
                .IsRequired()
                .HasConversion<string>();
            entity.Property(v => v.Status)
                .IsRequired()
                .HasConversion<string>();
            entity.HasOne(v => v.Station)
                .WithMany()
                .HasForeignKey(v => v.StationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
