namespace Airline.Application.Contracts.Flight;

/// <summary>
/// Предоставляет операции для запроса и анализа данных об авиарейсах.
/// </summary>
public interface IFlightService
{
    /// <summary>
    /// Возвращает топ-N авиарейсов с наибольшим количеством пассажиров.
    /// </summary>
    /// <param name="count">Максимальное количество рейсов для возврата. По умолчанию — 5.</param>
    /// <returns>Список DTO рейсов, отсортированных по убыванию числа пассажиров.</returns>
    public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5);

    /// <summary>
    /// Возвращает все авиарейсы, имеющие минимальную продолжительность полёта среди всех доступных рейсов.
    /// </summary>
    /// <returns>Список DTO рейсов с наименьшим значением продолжительности полёта.</returns>
    public Task<List<FlightDto>> GetFlightsWithShortestDurationAsync();

    /// <summary>
    /// Возвращает все авиарейсы, следующие по указанному маршруту (аэропорт вылета → аэропорт прилёта).
    /// </summary>
    /// <param name="departureAirport">Код аэропорта вылета (например, "SVO").</param>
    /// <param name="arrivalAirport">Код аэропорта прилёта (например, "JFK").</param>
    /// <returns>Список DTO рейсов, соответствующих заданному маршруту.</returns>
    public Task<List<FlightDto>> GetFlightsByRouteAsync(string departureAirport, string arrivalAirport);

    /// <summary>
    /// Возвращает все авиарейсы, выполняемые на указанной модели воздушного судна в заданный временной период.
    /// </summary>
    /// <param name="modelId">Идентификатор модели самолёта.</param>
    /// <param name="from">Начало периода (включительно, по дате вылета).</param>
    /// <param name="to">Конец периода (включительно, по дате прилёта).</param>
    /// <returns>Список DTO рейсов, соответствующих указанным критериям.</returns>
    public Task<List<FlightDto>> GetFlightsByModelAndPeriodAsync(int modelId, DateTime from, DateTime to);
}