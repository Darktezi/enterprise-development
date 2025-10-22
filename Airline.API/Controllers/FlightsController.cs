using Airline.Application.Contracts.Flights;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _service;
    private readonly ILogger<FlightsController> _logger;

    public FlightsController(
        IFlightService service,
        ILogger<FlightsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Получение топ-N рейсов по количеству пассажиров
    /// </summary>
    [HttpGet("top-by-passengers")]
    public async Task<ActionResult<List<FlightDto>>> GetTopFlightsByPassengerCount(int count = 5)
    {
        _logger.LogInformation("Getting top {Count} flights by passenger count", count);

        try
        {
            var flights = await _service.GetTopFlightsByPassengerCountAsync(count);
            return flights.Count > 0 ? Ok(flights) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top flights by passenger count");
            return StatusCode(500, "Internal server error");
        }
    }
}