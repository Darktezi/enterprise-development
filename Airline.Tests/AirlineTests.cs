using Airline.Domain.Entities;
using Airline.Domain.Fixtures;

namespace Airline.Tests;

/// <summary>
/// ёнит-тесты дл€ проверки выборок и агрегатов
/// на данных <see cref="AirCompanyFixture10"/>.
/// </summary>
public class AirlineTests(AirlineFixture fixture) : IClassFixture<AirlineFixture>
{
    /// <summary>
    /// ѕроверка: топ-5 рейсов с наибольшим числом пассажиров.
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
                $"Flight {topFlights[i].Code} должно иметь >= пассажиров, чем {topFlights[i + 1].Code}");
        }
    }

    /// <summary>
    /// ѕроверка: рейсы с минимальным временем полета.
    /// </summary>
    [Fact]
    public void FlightsWithShortestDuration_ShouldMatchMinimalTime()
    {
        var shortestTime = fixture.Flights.Min(f => f.TravelTime);
        var flights = fixture.Flights
            .Where(f => f.TravelTime == shortestTime)
            .ToList();

        Assert.All(flights, f => Assert.Equal(shortestTime, f.TravelTime));
        Assert.NotEmpty(flights);
    }

    /// <summary>
    /// ѕроверка: пассажиры рейса без багажа, упор€доченные по имени.
    /// </summary>
    [Fact]
    public void PassengersWithoutBaggage_ShouldBeSortedByName()
    {
        var flight = fixture.Flights.First();
        var passengers = flight.Tickets?
            .Where(t => t.BaggageWeight == 0)
            .Select(t => t.Passenger)
            .OrderBy(p => p.FullName)
            .ToList() ?? new List<Passenger>();

        Assert.All(passengers, p =>
            Assert.Contains(p.Tickets!, t => t.Flight == flight && t.BaggageWeight == 0));
    }

    /// <summary>
    /// ѕроверка: полеты самолета конкретной модели в заданный период.
    /// </summary>
    [Fact]
    public void FlightsOfModelWithinPeriod_ShouldReturnCorrectFlights()
    {
        var model = fixture.Models.Last();
        var from = new DateTime(2025, 9, 1);
        var to = new DateTime(2025, 9, 30);

        var flights = fixture.Flights
            .Where(f => f.AircraftModel == model &&
                        f.DepartureDate >= from &&
                        f.ArrivalDate <= to)
            .ToList();

        Assert.All(flights, f =>
        {
            Assert.Equal(model, f.AircraftModel);
            Assert.NotNull(f.DepartureDate);
            Assert.InRange(f.DepartureDate.Value, from, to);
        });
    }

    /// <summary>
    /// ѕроверка: рейсы между конкретными аэропортами.
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
