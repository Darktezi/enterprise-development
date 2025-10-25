using Airline.Application.Contracts.Flights;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

public class FlightService : IFlightService
{
    private readonly IRepository<Flight, int> _flightRepository;
    private readonly IMapper _mapper;

    public FlightService(IRepository<Flight, int> flightRepository, IMapper mapper)
    {
        _flightRepository = flightRepository;
        _mapper = mapper;
    }

    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5)
    {
        var allFlights = await _flightRepository.ReadAll();

        var topFlights = allFlights
            .OrderByDescending(f => f.Tickets?.Count ?? 0)
            .Take(count)
            .ToList();

        return _mapper.Map<List<FlightDto>>(topFlights);
    }
}