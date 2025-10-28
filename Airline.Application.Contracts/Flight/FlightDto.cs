namespace Airline.Application.Contracts.Flight;

/// <summary>
/// DTO для GET запросов к рейсам
/// </summary>
/// <param name="Id">Идентификатор рейса</param>
/// <param name="Code">Код рейса</param>
/// <param name="DepartureAirport">Аэропорт вылета</param>
/// <param name="ArrivalAirport">Аэропорт прилёта</param>
/// <param name="DepartureDate">Дата и время вылета</param>
/// <param name="ArrivalDate">Дата и время прилёта</param>
/// <param name="TravelTime">Время в пути</param>
/// <param name="AircraftModel">Модель самолёта</param>
/// <param name="TicketsCount">Количество билетов</param>
public record FlightDto(
    int Id,
    string? Code,
    string? DepartureAirport,
    string? ArrivalAirport,
    DateTime? DepartureDate,
    DateTime? ArrivalDate,
    TimeSpan? TravelTime,
    string? AircraftModel,
    int? TicketsCount
);
