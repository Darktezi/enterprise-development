namespace Airline.Application.Contracts.Flight;

public interface IFlightService
{
    public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5);
    public Task<List<FlightDto>> GetFlightsWithShortestDurationAsync();
    public Task<List<FlightDto>> GetFlightsByRouteAsync(string departureAirport, string arrivalAirport);
    public Task<List<FlightDto>> GetFlightsByModelAndPeriodAsync(int modelId, DateTime from, DateTime to);
}