using Airline.Domain.Data;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore;
public class AirlineDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AircraftFamily> Families { get; set; } = null!;
    public DbSet<AircraftModel> Models { get; set; } = null!;
    public DbSet<Flight> Flights { get; set; } = null!;
    public DbSet<Passenger> Passengers { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Семейство самолётов
        modelBuilder.Entity<AircraftFamily>(builder =>
        {
            builder.HasKey(f => f.Id);
            builder.HasMany(f => f.Models)
                    .WithOne(m => m.Family)
                    .IsRequired();

            builder.HasData(AirlineSeed.Families);
        });

        // Модель самолёта
        modelBuilder.Entity<AircraftModel>(builder =>
        {
            builder.HasKey(m => m.Id);
            builder.HasMany(m => m.Flights)
                    .WithOne(f => f.AircraftModel)
                    .IsRequired();

            builder.HasData(AirlineSeed.Models);
        });

        // Рейс
        modelBuilder.Entity<Flight>(builder =>
        {
            builder.HasKey(f => f.Id);
            builder.HasMany(f => f.Tickets)
                    .WithOne(t => t.Flight)
                    .IsRequired();

            builder.HasData(AirlineSeed.Flights);
        });

        // Пассажир
        modelBuilder.Entity<Passenger>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.Tickets)
                    .WithOne(t => t.Passenger)
                    .IsRequired();

            builder.HasData(AirlineSeed.Passengers);
        });

        // Билет
        modelBuilder.Entity<Ticket>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Flight)
                    .WithMany(f => f.Tickets)
                    .IsRequired();
            builder.HasOne(t => t.Passenger)
                    .WithMany(p => p.Tickets)
                    .IsRequired();

            builder.HasData(AirlineSeed.Tickets);
        });
    }
};