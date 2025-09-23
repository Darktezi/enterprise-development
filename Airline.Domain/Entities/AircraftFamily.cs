namespace Airline.Domain.Entities;

/// <summary>
/// Class representing an aircraft family.
/// </summary>
public class AircraftFamily
{
    /// <summary>
    /// Unique identifier of the aircraft family.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Name of the aircraft family (e.g., "Boeing 737").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Manufacturer of the aircraft in this family (e.g., "Boeing").
    /// </summary>
    public required string Manufacturer { get; set; }

    /// <summary>
    /// List of aircraft models belonging to this family.
    /// </summary>
    public List<AircraftModel>? Models { get; set; } = new List<AircraftModel>();
}
