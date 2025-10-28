using Airline.Application.Contracts.Passenger;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для работы с пассажирами.
/// Предоставляет эндпоинты для получения информации о пассажирах по дополнительным критериям
/// (например, пассажиры без багажа на конкретном рейсе).
/// </summary>
/// <param name="service">Сервис для выполнения бизнес-логики, связанной с пассажирами.</param>
/// <param name="logger">Логгер для записи диагностических сообщений.</param>
[ApiController]
[Route("api/[controller]")]
public class PassengersController(IPassengerService service, ILogger<PassengersController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список пассажиров указанного рейса, не имеющих зарегистрированного багажа.
    /// </summary>
    /// <param name="flightId">Идентификатор рейса.(например, 1)</param>
    /// <returns>Список пассажиров без багажа или NoContent, если таких нет.</returns>
    /// <response code="200">Пассажиры успешно найдены.</response>
    /// <response code="204">Пассажиры без багажа на указанном рейсе отсутствуют.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("flight/{flightId}/without-baggage")]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengersWithoutBaggage(int flightId)
    {
        logger.LogInformation("Getting passengers without baggage for flight {FlightId}", flightId);

        try
        {
            var passengers = await service.GetPassengersWithoutBaggageAsync(flightId);
            return passengers.Count > 0 ? Ok(passengers) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting passengers without baggage");
            return StatusCode(500, "Internal server error");
        }
    }
}