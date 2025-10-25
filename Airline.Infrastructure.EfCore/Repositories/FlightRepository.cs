using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class FlightEfCoreRepository(AirlineDbContext context) : IRepository<Flight, int>
{
    public async Task<Flight> Create(Flight entity)
    {
        var result = await context.Flights.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Flights.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Flights.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Flight?> Read(int entityId) =>
        await context.Flights
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Flight>> ReadAll() =>
        await context.Flights
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .ToListAsync();

    public async Task<Flight> Update(Flight entity)
    {
        context.Flights.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}