using Airline.Application.Contracts;
using Airline.Application.Contracts.Ticket;

namespace Airline.Api.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над билетами
/// </summary>
/// <param name="crudService">Сервис билетов</param>
/// <param name="logger">Логгер</param>
public class TicketsController(
    IApplicationService<TicketDto, TicketCreateUpdateDto, int> crudService,
    ILogger<TicketsController> logger)
    : CrudControllerBase<TicketDto, TicketCreateUpdateDto, int>(crudService, logger);