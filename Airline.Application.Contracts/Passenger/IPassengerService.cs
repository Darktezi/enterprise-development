namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// Предоставляет операции для CRUD-операций и запроса данных о пассажирах, связанных с их билетами и багажом.
/// </summary>
public interface IPassengerService : IApplicationService<PassengerDto, PassengerCreateUpdateDto, int>
{
    /// <summary>
    /// Возвращает список пассажиров указанного авиарейса, у которых отсутствует зарегистрированный багаж
    /// </summary>
    /// <param name="flightId">Уникальный идентификатор авиарейса.</param>
    /// <returns>Список DTO пассажиров, отсортированный по фамилии и имени.</returns>
    public Task<List<PassengerDto>> GetPassengersWithoutBaggageAsync(int flightId);
}