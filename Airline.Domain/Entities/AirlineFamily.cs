namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий семейство самолетов.
/// </summary>
public class AirlineFamily
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
    public virtual List<AirlineModel>? Models { get; set; } = new List<AirlineModel>();
}
