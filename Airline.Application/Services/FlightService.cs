using Airline.Application.Contracts.Flights;
using Airline.Infrastructure.EfCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Airline.Application.Services;

public class FlightService : IFlightService
{
    private readonly AirlineDbContext _context;
    private readonly IMapper _mapper;

    public FlightService(AirlineDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5)
    {
        var flights = await _context.Flights
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .OrderByDescending(f => f.Tickets.Count)
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<FlightDto>>(flights);
    }
}