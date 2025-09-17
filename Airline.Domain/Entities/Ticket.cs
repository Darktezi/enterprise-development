namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий билет на рейс.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор билета.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Рейс, на который оформлен билет.
    /// </summary>
    public required Flight Flight { get; set; }

    /// <summary>
    /// Пассажир, на которого оформлен билет.
    /// </summary>
    public required Passenger Passenger { get; set; }

    /// <summary>
    /// Номер места в самолете.
    /// </summary>
    public required string SeatNumber { get; set; }

    /// <summary>
    /// Наличие ручной клади.
    /// </summary>
    public bool HasHandLuggage { get; set; }

    /// <summary>
    /// Вес зарегистрированного багажа в килограммах (если есть).
    /// </summary>
    public decimal? BaggageWeight { get; set; }
}
