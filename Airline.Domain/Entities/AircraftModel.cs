namespace Airline.Domain.Entities;

/// <summary>
/// Class representing an aircraft model.
/// </summary>
public class AircraftModel
{
    /// <summary>
    /// Unique identifier of the aircraft model.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Name of the aircraft model (e.g., "Boeing 737").
    /// </summary>
    public required string ModelName { get; set; }

    /// <summary>
    /// Aircraft family (e.g., narrow-body, wide-body).
    /// </summary>
    public required AircraftFamily Family { get; set; }

    /// <summary>
    /// Flight range in kilometers.
    /// </summary>
    public required double FlightRange { get; set; }

    /// <summary>
    /// Passenger capacity.
    /// </summary>
    public required int PassengerCapacity { get; set; }

    /// <summary>
    /// Cargo capacity in tons.
    /// </summary>
    public required double CargoCapacity { get; set; }

    /// <summary>
    /// List of flights operated with this aircraft model.
    /// </summary>
    public List<Flight>? Flights { get; set; } = new List<Flight>();
}
