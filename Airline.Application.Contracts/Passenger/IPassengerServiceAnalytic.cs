namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// Предоставляет операции для запроса данных о пассажирах, связанных с их билетами и багажом.
/// </summary>
public interface IPassengerServiceAnalytic
{

    /// <summary>
    /// Возвращает список пассажиров указанного авиарейса, у которых отсутствует зарегистрированный багаж
    /// (т.е. вес багажа равен 0 кг).
    /// </summary>
    /// <param name="flightId">Уникальный идентификатор авиарейса.</param>
    /// <returns>Список DTO пассажиров, отсортированный по фамилии и имени.</returns>
    public Task<List<PassengerDto>> GetPassengersWithoutBaggageAsync(int flightId);
}