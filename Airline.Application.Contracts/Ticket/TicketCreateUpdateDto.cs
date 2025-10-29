using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// DTO для PUT/POST запросов к билетам.
/// </summary>
/// <param name="FlightId">Идентификатор рейса, на который оформлен билет</param>
/// <param name="PassengerId">Идентификатор пассажира, на которого оформлен билет</param>
/// <param name="SeatNumber">Номер места в самолёте</param>
/// <param name="HasHandLuggage">Наличие ручной клади</param>
/// <param name="BaggageWeight">Вес зарегистрированного багажа в килограммах (если есть)</param>
public record TicketCreateUpdateDto(
    int? FlightId,
    int? PassengerId,
    string? SeatNumber,
    bool? HasHandLuggage,
    double? BaggageWeight
);
