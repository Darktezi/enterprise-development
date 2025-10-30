using Airline.Api.Controllers;
using Airline.Application.Contracts;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Model;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над моделями самолетов
/// </summary>
/// <param name="crudService">Сервис моделей самолетов</param>
/// <param name="flightService">Сервис рейсов</param>
/// <param name="logger">Логгер</param>
public class ModelsController(
    IApplicationService<ModelDto, ModelCreateUpdateDto, int> crudService,
    IFlightService flightService,
    ILogger<ModelsController> logger)
    : CrudControllerBase<ModelDto, ModelCreateUpdateDto, int>(crudService, logger)
{
    /// <summary>
    /// Получает рейсы, выполняемые на указанной модели самолёта в заданный период.
    /// </summary>
    /// <param name="modelId">Идентификатор модели воздушного судна</param>
    /// <param name="from">Начало периода (включительно)</param>
    /// <param name="to">Конец периода (включительно)</param>
    /// <returns>Список рейсов по модели и периоду</returns>
    /// <response code="200">Рейсы найдены</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{modelId}/flights/period")]
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

            var flights = await flightService.GetFlightsByModelAndPeriodAsync(modelId, fromDateTime, toDateTime);
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting flights by model and period");
            return StatusCode(500, "Internal server error");
        }
    }
}