using Airline.Application.Contracts.Passenger;
using Airline.Domain;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для выполнения операций, связанных с пассажирами.
/// Предоставляет методы для фильтрации пассажиров по признакам, связанным с их билетами и багажом.
/// </summary>
public class PassengerService(
    IRepository<Domain.Entities.Passenger, int> passengerRepository,
    IMapper mapper) : IPassengerService
{
    /// <summary>
    /// Возвращает список пассажиров указанного рейса, у которых отсутствует зарегистрированный багаж
    /// </summary>
    /// <param name="flightId">Уникальный идентификатор авиарейса.</param>
    /// <returns>Список DTO пассажиров, отсортированный по фамилии и имени.</returns>
    public async Task<List<PassengerDto>> GetPassengersWithoutBaggageAsync(int flightId)
    {
        var passengers = await passengerRepository.ReadAll();
        var passengersWithoutBaggage = passengers
            .Where(p => p.Tickets?.Any(t => t.FlightId == flightId && t.BaggageWeight == 0) == true)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToList();

        return mapper.Map<List<PassengerDto>>(passengersWithoutBaggage);
    }
}