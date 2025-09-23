namespace Airline.Domain.Entities;

/// <summary>
/// Class representing a flight ticket.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Unique identifier of the ticket.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Flight for which the ticket is issued.
    /// </summary>
    public required Flight Flight { get; set; }

    /// <summary>
    /// Passenger for whom the ticket is issued.
    /// </summary>
    public required Passenger Passenger { get; set; }

    /// <summary>
    /// Seat number on the aircraft.
    /// </summary>
    public required string SeatNumber { get; set; }

    /// <summary>
    /// Indicates whether the passenger has hand luggage.
    /// </summary>
    public bool HasHandLuggage { get; set; }

    /// <summary>
    /// Weight of checked baggage in kilograms (if any).
    /// </summary>
    public double? BaggageWeight { get; set; }
}
