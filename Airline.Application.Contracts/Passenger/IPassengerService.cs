namespace Airline.Application.Contracts.Passenger;

public interface IPassengerService
{
    public Task<List<PassengerDto>> GetPassengersWithoutBaggageAsync(int flightId);
}