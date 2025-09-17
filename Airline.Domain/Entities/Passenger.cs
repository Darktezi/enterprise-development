namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий пассажира.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Уникальный идентификатор пассажира.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер паспорта пассажира.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Полное имя пассажира.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Дата рождения пассажира (если известна).
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// Список билетов, приобретённых пассажиром.
    /// </summary>
    public List<Ticket> Tickets { get; set; } = [];
}
