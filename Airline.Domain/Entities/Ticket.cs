namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий билет на рейс.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор билета.
    /// </summary>
    public required int Id { get; set; }

    public required int FlightId { get; set; }

    /// <summary>
    /// Рейс, на который оформлен билет.
    /// </summary>
    public virtual Flight? Flight { get; set; }

    public required int PassengerId { get; set; }

    /// <summary>
    /// Пассажир, на которого оформлен билет.
    /// </summary>
    public virtual Passenger? Passenger { get; set; }

    /// <summary>
    /// Номер места в самолете.
    /// </summary>
    public required string SeatNumber { get; set; }

    /// <summary>
    /// Наличие ручной клади.
    /// </summary>
    public required bool HasHandLuggage { get; set; }

    /// <summary>
    /// Вес зарегистрированного багажа в килограммах (если есть).
    /// </summary>
    public double? BaggageWeight { get; set; }
}
