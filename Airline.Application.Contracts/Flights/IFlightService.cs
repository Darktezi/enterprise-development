using Airline.Application.Contracts.Flights;

namespace Airline.Application.Contracts.Flights;

public interface IFlightService
{
    public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5);
}