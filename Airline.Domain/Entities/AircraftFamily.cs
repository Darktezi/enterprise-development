namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий семейство самолетов.
/// </summary>
public class AircraftFamily
{
    /// <summary>
    /// Уникальный идентификатор семейства самолетов.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название семейства самолетов (например, "Boeing 737").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Производитель самолетов данного семейства (например, "Boeing").
    /// </summary>
    public required string Manufacturer { get; set; }

    /// <summary>
    /// Список моделей самолетов, относящихся к этому семейству.
    /// </summary>
    public virtual List<AircraftModel>? Models { get; set; } = new List<AircraftModel>();
}
