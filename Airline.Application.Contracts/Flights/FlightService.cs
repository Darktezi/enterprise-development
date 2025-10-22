using Airline.Application.Contracts.Flights;
using Airline.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Airline.Application.Contracts.Flights;

public class FlightService : IFlightService
{
    private readonly AirlineDbContext _context;

    public FlightService(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int count = 5)
    {
        var data = await _context.Flights
            .Select(f => new
            {
                f.Id,
                f.Code,
                f.DepartureAirport,
                f.ArrivalAirport,
                f.DepartureDate,
                f.ArrivalDate,
                f.TravelTime,
                AircraftModel = f.AircraftModel.ModelName,
                TicketsCount = f.Tickets.Count
            })
            .OrderByDescending(x => x.TicketsCount)
            .Take(count)
            .ToListAsync();

        return data.Select(d => new FlightDto(
            d.Id,
            d.Code,
            d.DepartureAirport,
            d.ArrivalAirport,
            d.DepartureDate,
            d.ArrivalDate,
            d.TravelTime,
            d.AircraftModel,
            d.TicketsCount
        )).ToList();
    }
}