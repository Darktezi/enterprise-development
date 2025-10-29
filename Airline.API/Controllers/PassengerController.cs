using Airline.Application.Contracts;
using Airline.Application.Contracts.Passenger;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над пассажирами
/// </summary>
/// <param name="crudService">Сервис пассажиров</param>
/// <param name="passengerService">Аналитический сервис пассажиров</param>
/// <param name="logger">Логгер</param>
public class PassengersController(
    IApplicationService<PassengerDto, PassengerCreateUpdateDto, int> crudService,
    IPassengerService passengerService,
    ILogger<PassengersController> logger)
    : CrudControllerBase<PassengerDto, PassengerCreateUpdateDto, int>(crudService, logger)
{
    /// <summary>
    /// Получение пассажиров рейса без багажа
    /// </summary>
    [HttpGet("flight/{flightId}/without-baggage")]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengersWithoutBaggage(int flightId)
    {
        logger.LogInformation("Getting passengers without baggage for flight {FlightId}", flightId);
        try
        {
            var passengers = await passengerService.GetPassengersWithoutBaggageAsync(flightId);
            return passengers.Count > 0 ? Ok(passengers) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting passengers without baggage");
            return StatusCode(500, "Internal server error");
        }
    }
}