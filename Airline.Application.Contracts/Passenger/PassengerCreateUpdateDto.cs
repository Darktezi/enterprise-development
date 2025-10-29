namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// DTO для PUT/POST запросов к пассажирам
/// </summary>
/// <param name="PassportNumber">Номер паспорта пассажира</param>
/// <param name="LastName">Фамилия пассажира</param>
/// <param name="FirstName">Имя пассажира</param>
/// <param name="MiddleName">Отчество пассажира</param>
/// <param name="BirthDate">Дата рождения пассажира</param>
public record PassengerCreateUpdateDto(
    string? PassportNumber,
    string? LastName,
    string? FirstName,
    string? MiddleName,
    DateOnly? BirthDate
);
