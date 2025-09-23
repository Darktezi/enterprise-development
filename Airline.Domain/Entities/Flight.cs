namespace Airline.Domain.Entities;

/// <summary>
/// Class representing a flight.
/// </summary>
public class Flight
{
    /// <summary>
    /// Unique identifier of the flight.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Flight code (e.g., "QF025").
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Departure airport.
    /// </summary>
    public required string DepartureAirport { get; set; }

    /// <summary>
    /// Arrival airport.
    /// </summary>
    public required string ArrivalAirport { get; set; }

    /// <summary>
    /// Date and time of departure (if known).
    /// </summary>
    public DateTime? DepartureDate { get; set; }

    /// <summary>
    /// Date and time of arrival (if known).
    /// </summary>
    public DateTime? ArrivalDate { get; set; }

    /// <summary>
    /// Flight duration (if known).
    /// </summary>
    public TimeSpan? TravelTime { get; set; }

    /// <summary>
    /// Aircraft model used for this flight.
    /// </summary>
    public required AircraftModel AircraftModel { get; set; }

    /// <summary>
    /// List of tickets booked for this flight.
    /// </summary>
    public List<Ticket>? Tickets { get; set; } = new List<Ticket>();
}
