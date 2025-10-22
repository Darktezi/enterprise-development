namespace Airline.Application.Contracts.Flights;

public record FlightDto(
    int Id,
    string Code,
    string DepartureAirport,
    string ArrivalAirport,
    DateTime? DepartureDate,
    DateTime? ArrivalDate,
    TimeSpan? TravelTime,
    string AircraftModel,
    int TicketsCount
);