namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// DTO для GET запросов к пассажирам
/// </summary>
/// <param name="Id">Идентификатор пассажира</param>
/// <param name="PassportNumber">Номер паспорта пассажира</param>
/// <param name="LastName">Фамилия пассажира</param>
/// <param name="FirstName">Имя пассажира</param>
/// <param name="MiddleName">Отчество пассажира</param>
/// <param name="BirthDate">Дата рождения пассажира</param>
/// <param name="TicketsCount">Количество купленных билетов пассажира</param>
public record PassengerDto(
    int Id,
    string? PassportNumber,
    string? LastName,
    string? FirstName,
    string? MiddleName,
    DateOnly? BirthDate,
    int? TicketsCount
);
