using Airline.Domain.Entities;
using Airline.Domain.Data;

namespace Airline.Tests;

/// <summary>
/// Юнит-тесты для проверки выборок и агрегатов
/// на данных <see cref="AirlineSeed"/>.
/// </summary>
public class AirlineTests(AirlineSeed fixture) : IClassFixture<AirlineSeed>
{
    /// <summary>
    /// Проверка: топ-5 рейсов с наибольшим числом пассажиров.
    /// </summary>
    [Fact]
    public void TopFlightsByPassengerCount_ShouldBeCorrectlyOrdered()
    {
        var topFlights = fixture.Flights
            .OrderByDescending(f => f.Tickets?.Count ?? 0)
            .Take(5)
            .ToList();

        Assert.NotEmpty(topFlights);
        Assert.True(topFlights.Count <= 5);

        for (var i = 0; i < topFlights.Count - 1; i++)
        {
            var currentCount = topFlights[i].Tickets?.Count ?? 0;
            var nextCount = topFlights[i + 1].Tickets?.Count ?? 0;

            Assert.True(currentCount >= nextCount,
                $"Рейс {topFlights[i].Code} должно иметь >= пассажиров, чем {topFlights[i + 1].Code}");
        }
    }

    /// <summary>
    /// Проверка: рейсы с минимальным временем полета.
    /// </summary>
    [Fact]
    public void FlightsWithShortestDuration_ShouldMatchMinimalTime()
    {
        var shortestTime = fixture.Flights.Min(f => f.TravelTime);
        var flights = fixture.Flights
            .Where(f => f.TravelTime == shortestTime)
            .ToList();

        Assert.NotEmpty(flights);
        Assert.All(flights, f => Assert.Equal(shortestTime, f.TravelTime));
    }

    /// <summary>
    /// Проверка: пассажиры рейса без багажа, упорядоченные по фамилии и имени.
    /// </summary>
    [Fact]
    public void PassengersWithoutBaggage_ShouldBeSortedByName()
    {
        var flight = fixture.Flights.First();
        var passengers = flight.Tickets?
            .Where(t => t.BaggageWeight == 0)
            .Select(t => t.Passenger)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToList() ?? new List<Passenger>();

        Assert.All(passengers, p =>
            Assert.Contains(p.Tickets ?? new List<Ticket>(), t => t.Flight == flight && t.BaggageWeight == 0));
    }

    /// <summary>
    /// Проверка: полеты самолета конкретной модели в заданный период.
    /// </summary>
    [Fact]
    public void FlightsOfModelWithinPeriod_ShouldReturnCorrectFlights()
    {
        var model = fixture.Models.Last();
        var from = new DateTime(2025, 10, 1);
        var to = new DateTime(2025, 10, 31);

        var flights = fixture.Flights
            .Where(f => f.AircraftModel == model &&
                        f.DepartureDate >= from &&
                        f.ArrivalDate <= to)
            .ToList();

        Assert.All(flights, f =>
        {
            Assert.Equal(model, f.AircraftModel);
            Assert.NotNull(f.DepartureDate);
            Assert.InRange(f.DepartureDate!.Value, from, to);
        });
    }

    /// <summary>
    /// Проверка: рейсы между конкретными аэропортами.
    /// </summary>
    [Fact]
    public void FlightsBetweenAirports_ShouldMatchRoute()
    {
        var departure = "LED";
        var arrival = "JFK";

        var flights = fixture.Flights
            .Where(f => f.DepartureAirport == departure && f.ArrivalAirport == arrival)
            .ToList();

        Assert.All(flights, f =>
        {
            Assert.Equal(departure, f.DepartureAirport);
            Assert.Equal(arrival, f.ArrivalAirport);
        });
    }
}
