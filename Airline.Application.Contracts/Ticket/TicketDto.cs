using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// DTO для передачи данных о билете на рейс
/// </summary>
/// <param name="Id">Уникальный идентификатор билета</param>
/// <param name="FlightId">Идентификатор рейса, на который оформлен билет</param>
/// <param name="Flight">Данные о рейсе, на который оформлен билет</param>
/// <param name="PassengerId">Идентификатор пассажира, на которого оформлен билет</param>
/// <param name="Passenger">Данные о пассажире, на которого оформлен билет</param>
/// <param name="SeatNumber">Номер места в самолёте</param>
/// <param name="HasHandLuggage">Наличие ручной клади</param>
/// <param name="BaggageWeight">Вес зарегистрированного багажа в килограммах (если есть)</param>
public record TicketDto(
    int Id,
    int? FlightId,
    FlightDto? Flight,
    int? PassengerId,
    PassengerDto? Passenger,
    string? SeatNumber,
    bool? HasHandLuggage,
    double? BaggageWeight
);
