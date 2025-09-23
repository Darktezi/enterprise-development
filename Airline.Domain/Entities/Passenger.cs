namespace Airline.Domain.Entities;

/// <summary>
/// Class representing a passenger.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Unique identifier of the passenger.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Passport number of the passenger.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the passenger.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Birth date of the passenger (if known).
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// List of tickets purchased by the passenger.
    /// </summary>
    public List<Ticket>? Tickets { get; set; } = new List<Ticket>();
}
