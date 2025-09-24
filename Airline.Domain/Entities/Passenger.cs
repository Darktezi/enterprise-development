namespace Airline.Domain.Entities;

/// <summary>
/// Класс, представляющий пассажира.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Уникальный идентификатор пассажира.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер паспорта пассажира.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Фамилия пассажира.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Имя пассажира.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Отчество пассажира (может отсутствовать).
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Дата рождения пассажира (если известна).
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// Список билетов, приобретённых пассажиром.
    /// </summary>
    public List<Ticket>? Tickets { get; set; } = [];
}
