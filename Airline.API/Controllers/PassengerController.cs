using Airline.Application.Contracts;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над пассажирами
/// </summary>
/// <param name="crudService">Сервис пассажиров</param>
/// <param name="ticketService">Сервис билетов</param>
/// <param name="logger">Логгер</param>
public class PassengersController(
    IApplicationService<PassengerDto, PassengerCreateUpdateDto, int> crudService,
    ITicketService ticketService,
    ILogger<PassengersController> logger)
    : CrudControllerBase<PassengerDto, PassengerCreateUpdateDto, int>(crudService, logger)
{
    /// <summary>
    /// Получает все билеты для указанного пассажира
    /// </summary>
    /// <param name="passengerId">Идентификатор пассажира</param>
    /// <returns>Список билетов пассажира</returns>
    /// <response code="200">Билеты успешно получены</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{passengerId}/tickets")]
    public async Task<ActionResult<List<TicketDto>>> GetPassengerTickets(int passengerId)
    {
        logger.LogInformation("Getting tickets for passenger {PassengerId}", passengerId);
        try
        {
            var tickets = await ticketService.GetTicketsByPassengerAsync(passengerId);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting passenger tickets");
            return StatusCode(500, "Internal server error");
        }
    }
}