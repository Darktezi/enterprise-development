using Airline.Application.Contracts;
using Airline.Application.Contracts.Flight;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для работы с авиарейсами.
/// Предоставляет эндпоинты для CRUD-операций и аналитики рейсов:
/// топ по количеству пассажиров, кратчайшая длительность, маршрут, модель самолёта и период.
/// </summary>
/// <param name="crudService">CRUD-сервис рейсов</param>
/// <param name="analyticService">Аналитический сервис рейсов</param>
/// <param name="logger">Логгер</param>
public class FlightsController(
    IApplicationService<FlightDto, FlightCreateUpdateDto, int> crudService,
    IFlightService analyticService,
    ILogger<FlightsController> logger)
    : CrudControllerBase<FlightDto, FlightCreateUpdateDto, int>(crudService, logger)
{
    /// <summary>
    /// Получает список топ-N рейсов с наибольшим количеством пассажиров.
    /// </summary>
    /// <param name="count">Количество рейсов для возврата (по умолчанию — 5).</param>
    /// <returns>Список рейсов в формате DTO или NoContent, если рейсы не найдены.</returns>
    /// <response code="200">Список рейсов успешно получен.</response>
    /// <response code="204">Рейсы не найдены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("top-by-passengers")]
    public async Task<ActionResult<List<FlightDto>>> GetTopFlightsByPassengerCount(int count = 5)
    {
        logger.LogInformation("Getting top {Count} flights by passenger count", count);

        try
        {
            var flights = await analyticService.GetTopFlightsByPassengerCountAsync(count);
            return flights.Count > 0 ? Ok(flights) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top flights by passenger count");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Получает все рейсы, имеющие минимальное время полёта среди всех рейсов.
    /// </summary>
    /// <returns>Список рейсов с кратчайшей продолжительностью или NoContent, если данные отсутствуют.</returns>
    /// <response code="200">Рейсы с минимальной длительностью найдены.</response>
    /// <response code="204">Рейсы не найдены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("shortest-duration")]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsWithShortestDuration()
    {
        logger.LogInformation("Getting flights with shortest duration");

        try
        {
            var flights = await analyticService.GetFlightsWithShortestDurationAsync();
            return flights.Count > 0 ? Ok(flights) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flights with shortest duration");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Получает рейсы по заданному маршруту (аэропорт вылета → аэропорт прилёта).
    /// </summary>
    /// <param name="departure">Код аэропорта вылета (например, "SVO").</param>
    /// <param name="arrival">Код аэропорта прилёта (например, "JFK").</param>
    /// <returns>Список рейсов по маршруту или NoContent, если рейсы не найдены.</returns>
    /// <response code="200">Рейсы по маршруту найдены.</response>
    /// <response code="204">Рейсы по указанному маршруту отсутствуют.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("route/{departure}/{arrival}")]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByRoute(string departure, string arrival)
    {
        logger.LogInformation("Getting flights from {Departure} to {Arrival}", departure, arrival);

        try
        {
            var flights = await analyticService.GetFlightsByRouteAsync(departure, arrival);
            return flights.Count > 0 ? Ok(flights) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flights by route");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Получает рейсы, выполняемые на указанной модели самолёта в заданный период.
    /// </summary>
    /// <param name="modelId">Идентификатор модели воздушного судна.(например, 1)</param>
    /// <param name="from">Начало периода (включительно).(например, "2025-08-12")</param>
    /// <param name="to">Конец периода (включительно).(например, "2025-11-12")</param>
    /// <returns>Список рейсов по модели и периоду или NoContent, если рейсы не найдены.</returns>
    /// <response code="200">Рейсы найдены.</response>
    /// <response code="204">Рейсы по заданным критериям отсутствуют.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("model/{modelId}/period")]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByModelAndPeriod(
        int modelId,
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        logger.LogInformation("Getting flights for model {ModelId} from {From} to {To}", modelId, from, to);

        try
        {
            var fromDateTime = from.ToDateTime(TimeOnly.MinValue);
            var toDateTime = to.ToDateTime(TimeOnly.MaxValue);

            var flights = await analyticService.GetFlightsByModelAndPeriodAsync(modelId, fromDateTime, toDateTime);

            return flights.Count > 0 ? Ok(flights) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flights by model and period");
            return StatusCode(500, "Internal server error");
        }
    }
}