using Airline.Application.Contracts.Flight;
using Airline.Domain;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для выполнения бизнес-логики, связанной с авиарейсами.
/// Предоставляет методы для фильтрации и анализа рейсов на основе пассажиропотока, маршрутов, моделей самолётов и времени полёта.
/// </summary>
public class FlightService(
    IRepository<Domain.Entities.Flight, int> flightRepository,
    IMapper mapper) : IFlightService
{

    /// <summary>
    /// Возвращает топ-N авиарейсов с наибольшим количеством пассажиров (по количеству билетов).
    /// </summary>
    /// <param name="count">Максимальное количество рейсов для возврата. По умолчанию — 5.</param>
    /// <returns>Список DTO рейсов, отсортированных по убыванию числа пассажиров.</returns>
    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5)
    {
        var flights = await flightRepository.ReadAll();
        var topFlights = flights
            .OrderByDescending(f => f.Tickets?.Count ?? 0)
            .Take(count)
            .ToList();

        return mapper.Map<List<FlightDto>>(topFlights);
    }

    /// <summary>
    /// Возвращает все авиарейсы, имеющие минимальную продолжительность полёта среди всех доступных рейсов.
    /// </summary>
    /// <returns>Список DTO рейсов с наименьшим значением <see cref="Domain.Entities.Flight.TravelTime"/>.</returns>
    public async Task<List<FlightDto>> GetFlightsWithShortestDurationAsync()
    {
        var flights = await flightRepository.ReadAll();
        var shortestTime = flights
            .Where(f => f.TravelTime.HasValue)
            .Min(f => f.TravelTime);

        var shortestFlights = flights
            .Where(f => f.TravelTime == shortestTime)
            .ToList();

        return mapper.Map<List<FlightDto>>(shortestFlights);
    }

    /// <summary>
    /// Возвращает все авиарейсы, следующие по заданному маршруту (аэропорт вылета → аэропорт прилёта).
    /// </summary>
    /// <param name="departureAirport">Код аэропорта вылета (например, "SVO").</param>
    /// <param name="arrivalAirport">Код аэропорта прилёта (например, "JFK").</param>
    /// <returns>Список DTO рейсов, соответствующих указанному маршруту.</returns>
    public async Task<List<FlightDto>> GetFlightsByRouteAsync(string departureAirport, string arrivalAirport)
    {
        var flights = await flightRepository.ReadAll();
        var routeFlights = flights
            .Where(f => f.DepartureAirport == departureAirport && f.ArrivalAirport == arrivalAirport)
            .ToList();

        return mapper.Map<List<FlightDto>>(routeFlights);
    }

    /// <summary>
    /// Возвращает все авиарейсы, выполняемые на указанной модели воздушного судна в заданный период времени.
    /// </summary>
    /// <param name="modelId">Идентификатор модели самолёта.</param>
    /// <param name="from">Начало периода (включительно, по дате вылета).</param>
    /// <param name="to">Конец периода (включительно, по дате прилёта).</param>
    /// <returns>Список DTO рейсов, соответствующих критериям фильтрации.</returns>
    public async Task<List<FlightDto>> GetFlightsByModelAndPeriodAsync(int modelId, DateTime from, DateTime to)
    {
        var flights = await flightRepository.ReadAll();
        var modelFlights = flights
            .Where(f => f.AircraftModelId == modelId &&
                       f.DepartureDate >= from &&
                       f.ArrivalDate <= to)
            .ToList();

        return mapper.Map<List<FlightDto>>(modelFlights);
    }
}