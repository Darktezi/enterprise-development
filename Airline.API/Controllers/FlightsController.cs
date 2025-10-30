using Airline.Application.Contracts;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers;

/// <summary>
/// Контроллер для работы с авиарейсами.
/// Предоставляет эндпоинты для CRUD-операций и аналитики рейсов:
/// топ по количеству пассажиров, кратчайшая длительность, маршрут, модель самолёта и период.
/// </summary>
/// <param name="crudService">CRUD-сервис рейсов</param>
/// <param name="flightService">Cервис рейсов</param>
/// <param name="passengerService">Cервис рейсов</param>
/// <param name="ticketService">Cервис рейсов</param>
/// <param name="logger">Логгер</param>
public class FlightsController(
    IApplicationService<FlightDto, FlightCreateUpdateDto, int> crudService,
    IFlightService flightService,
    IPassengerService passengerService,
    ITicketService ticketService,
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
            var flights = await flightService.GetTopFlightsByPassengerCountAsync(count);
            return Ok(flights);
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
            var flights = await flightService.GetFlightsWithShortestDurationAsync();
            return Ok(flights);
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
    [HttpGet("route")]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByRoute(
    [FromQuery] string departure,
    [FromQuery] string arrival)
    {
        logger.LogInformation("Getting flights from {Departure} to {Arrival}", departure, arrival);

        try
        {
            var flights = await flightService.GetFlightsByRouteAsync(departure, arrival);
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flights by route");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Получает список пассажиров указанного рейса, у которых отсутствует зарегистрированный багаж
    /// </summary>
    /// <param name="flightId">Уникальный идентификатор авиарейса</param>
    /// <returns>Список пассажиров без багажа, отсортированный по фамилии и имени</returns>
    /// <response code="200">Список пассажиров успешно получен</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{flightId}/passengers/without-baggage")]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengersWithoutBaggage(int flightId)
    {
        logger.LogInformation("Getting passengers without baggage for flight {FlightId}", flightId);
        try
        {
            var passengers = await passengerService.GetPassengersWithoutBaggageAsync(flightId);
            return Ok(passengers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting passengers without baggage");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Получает все билеты для указанного рейса
    /// </summary>
    /// <param name="flightId">Идентификатор рейса</param>
    /// <returns>Список билетов на рейс</returns>
    /// <response code="200">Билеты успешно получены</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{flightId}/tickets")]
    public async Task<ActionResult<List<TicketDto>>> GetFlightTickets(int flightId)
    {
        logger.LogInformation("Getting tickets for flight {FlightId}", flightId);
        try
        {
            var tickets = await ticketService.GetTicketsByFlightAsync(flightId);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flight tickets");
            return StatusCode(500, "Internal server error");
        }
    }
}