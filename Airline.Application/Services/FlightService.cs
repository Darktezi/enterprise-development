using Airline.Application.Contracts;
using Airline.Application.Contracts.Flight;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для выполнения бизнес-логики, связанной с авиарейсами.
/// Предоставляет методы для CRUD-операций и анализа рейсов на основе пассажиропотока, маршрутов, моделей самолётов и времени полёта.
/// </summary>
public class FlightService(
    IRepository<Flight, int> flightRepository,
    IMapper mapper) : IFlightService, IApplicationService<FlightDto, FlightCreateUpdateDto, int>
{
    /// <summary>
    /// Создание нового рейса
    /// </summary>
    /// <param name="dto">DTO с данными для создания рейса</param>
    /// <returns>Созданный рейс</returns>
    public async Task<FlightDto> Create(FlightCreateUpdateDto dto)
    {
        var newFlight = mapper.Map<Flight>(dto);
        var res = await flightRepository.Create(newFlight);
        return mapper.Map<FlightDto>(res);
    }

    /// <summary>
    /// Удаление рейса по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор рейса</param>
    /// <returns>True если удаление успешно, иначе False</returns>
    public async Task<bool> Delete(int dtoId) =>
        await flightRepository.Delete(dtoId);

    /// <summary>
    /// Получение рейса по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор рейса</param>
    /// <returns>DTO рейса или null если не найдено</returns>
    public async Task<FlightDto?> Get(int dtoId) =>
        mapper.Map<FlightDto>(await flightRepository.Read(dtoId));

    /// <summary>
    /// Получение всех рейсов
    /// </summary>
    /// <returns>Список всех рейсов</returns>
    public async Task<IList<FlightDto>> GetAll() =>
        mapper.Map<List<FlightDto>>(await flightRepository.ReadAll());

    /// <summary>
    /// Обновление данных рейса
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор обновляемого рейса</param>
    /// <returns>Обновленный рейс</returns>
    public async Task<FlightDto> Update(FlightCreateUpdateDto dto, int dtoId)
    {
        var existingFlight = await flightRepository.Read(dtoId);
        if (existingFlight == null)
            throw new ArgumentException($"Flight with id {dtoId} not found");

        mapper.Map(dto, existingFlight);
        var res = await flightRepository.Update(existingFlight);
        return mapper.Map<FlightDto>(res);
    }

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
    /// Возвращает все авиарейсов, имеющие минимальную продолжительность полёта среди всех доступных рейсов.
    /// </summary>
    /// <returns>Список DTO рейсов с наименьшим значением <see cref="Flight.TravelTime"/>.</returns>
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