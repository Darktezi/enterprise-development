namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий авиарейс.
/// </summary>
public class Flight
{
    /// <summary>
    /// Уникальный идентификатор рейса.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Код рейса (например, "QF025").
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Аэропорт отправления.
    /// </summary>
    public required string DepartureAirport { get; set; }

    /// <summary>
    /// Аэропорт прибытия.
    /// </summary>
    public required string ArrivalAirport { get; set; }

    /// <summary>
    /// Дата и время отправления рейса (если известны).
    /// </summary>
    public DateTime? DepartureDate { get; set; }

    /// <summary>
    /// Дата и время прибытия рейса (если известны).
    /// </summary>
    public DateTime? ArrivalDate { get; set; }

    /// <summary>
    /// Продолжительность рейса (если известна).
    /// </summary>
    public TimeSpan? TravelTime { get; set; }

    /// <summary>
    /// Модель самолета, используемого для рейса.
    /// </summary>
    public required AircraftModel AircraftModel { get; set; }

    /// <summary>
    /// Список билетов, оформленных на этот рейс.
    /// </summary>
    public List<Ticket>? Tickets { get; set; } = new List<Ticket>();
}
