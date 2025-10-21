namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий модель самолета.
/// </summary>
public class AircraftModel
{
    /// <summary>
    /// Уникальный идентификатор модели самолета.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название модели самолета (например, "Boeing 737").
    /// </summary>
    public required string ModelName { get; set; }

    public int FamilyId { get; set; }
    /// <summary>
    /// Семейство самолета (например, узкофюзеляжный, широкофюзеляжный).
    /// </summary>
    public AircraftFamily? Family { get; set; }

    /// <summary>
    /// Дальность полета в километрах.
    /// </summary>
    public required double FlightRange { get; set; }

    /// <summary>
    /// Вместимость пассажиров.
    /// </summary>
    public required int PassengerCapacity { get; set; }

    /// <summary>
    /// Вместимость груза в тоннах.
    /// </summary>
    public required double CargoCapacity { get; set; }

    /// <summary>
    /// Список рейсов, выполняемых с использованием этой модели самолета.
    /// </summary>
    public List<Flight>? Flights { get; set; } = new List<Flight>();
}
