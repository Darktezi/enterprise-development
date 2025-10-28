using Airline.Domain.Data;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore;

/// <summary>
/// Контекст базы данных для авиакомпании, реализующий Entity Framework Core.
/// Настраивает связи между сущностями и загружает начальные данные (seed data) из <see cref="AirlineSeed"/>.
/// </summary>
public class AirlineDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Набор данных семейств воздушных судов (например, Boeing 737, Airbus A320).
    /// </summary>
    public DbSet<AirlineFamily> Families { get; set; } = null!;

    /// <summary>
    /// Набор данных конкретных моделей воздушных судов (например, 737-800, A320-200).
    /// </summary>
    public DbSet<AirlineModel> Models { get; set; } = null!;

    /// <summary>
    /// Набор данных авиарейсов с маршрутами, временем вылета/прилёта и привязкой к модели самолёта.
    /// </summary>
    public DbSet<Flight> Flights { get; set; } = null!;

    /// <summary>
    /// Набор данных пассажиров с персональной информацией и паспортными данными.
    /// </summary>
    public DbSet<Passenger> Passengers { get; set; } = null!;

    /// <summary>
    /// Набор данных билетов, связывающих пассажиров с рейсами, местами и багажом.
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; } = null!;

    /// <summary>
    /// Настраивает модель данных Entity Framework Core: определяет первичные ключи, связи между сущностями
    /// (один ко многим, многие к одному) и загружает начальные данные (seed data) из класса <see cref="AirlineSeed"/>.
    /// </summary>
    /// <param name="modelBuilder">Используется для конфигурации сущностей и их отношений.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Семейство самолётов
        modelBuilder.Entity<AirlineFamily>(builder =>
        {
            builder.HasKey(f => f.Id);
            builder.HasMany(f => f.Models)
                    .WithOne(m => m.Family)
                    .HasForeignKey(m => m.FamilyId)
                    .IsRequired();

            builder.HasData(AirlineSeed.Families);
        });

        // Модель самолёта
        modelBuilder.Entity<AirlineModel>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Family)
                   .WithMany(f => f.Models)
                   .HasForeignKey(m => m.FamilyId)
                   .IsRequired();

            builder.HasMany(m => m.Flights)
                   .WithOne(f => f.AircraftModel)
                   .IsRequired();

            builder.HasData(AirlineSeed.Models);
        });

        // Рейс
        modelBuilder.Entity<Flight>(builder =>
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.AircraftModel)
                    .WithMany(m => m.Flights)
                    .HasForeignKey(f => f.AircraftModelId)
                    .IsRequired();

            builder.HasMany(f => f.Tickets)
                    .WithOne(t => t.Flight)
                    .HasForeignKey(t => t.FlightId)
                    .IsRequired();

            builder.HasData(AirlineSeed.Flights);
        });

        // Пассажир
        modelBuilder.Entity<Passenger>(builder =>
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Tickets)
                    .WithOne(t => t.Passenger)
                    .HasForeignKey(t => t.PassengerId)
                    .IsRequired();

            builder.HasData(AirlineSeed.Passengers);
        });

        // Билет
        modelBuilder.Entity<Ticket>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Flight)
                    .WithMany(f => f.Tickets)
                    .HasForeignKey(t => t.FlightId)
                    .IsRequired();

            builder.HasOne(t => t.Passenger)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(t => t.PassengerId)
                    .IsRequired();

            builder.HasData(AirlineSeed.Tickets);
        });
    }
};