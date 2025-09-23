using Airline.Domain.Entities;
using Airline.Domain.Fixtures;

namespace Airline.Tests;

/// <summary>
/// Unit tests for validating queries and aggregates
/// using data from <see cref="AirlineFixture"/>.
/// </summary>
public class AirlineTests(AirlineFixture fixture) : IClassFixture<AirlineFixture>
{
    /// <summary>
    /// Checks top-5 flights with the highest number of passengers.
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
                $"Flight {topFlights[i].Code} should have >= passengers than {topFlights[i + 1].Code}");
        }
    }

    /// <summary>
    /// Checks flights with the shortest duration.
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
    /// Checks passengers of a flight without baggage, ordered by full name.
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
            Assert.Contains(p.Tickets ?? new List<Ticket>(), t => t.Flight == flight && t.BaggageWeight == 0));
    }

    /// <summary>
    /// Checks flights of a specific aircraft model within a date range.
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
            Assert.InRange(f.DepartureDate!.Value, from, to);
        });
    }

    /// <summary>
    /// Checks flights between specific departure and arrival airports.
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
