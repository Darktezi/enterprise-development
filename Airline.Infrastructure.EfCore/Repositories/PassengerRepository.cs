using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class PassengerEfCoreRepository(AirlineDbContext context) : IRepository<Passenger, int>
{
    public async Task<Passenger> Create(Passenger entity)
    {
        var result = await context.Passengers.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Passengers.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Passengers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Passenger?> Read(int entityId) =>
        await context.Passengers
            .Include(p => p.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Passenger>> ReadAll() =>
        await context.Passengers
            .Include(p => p.Tickets)
            .ToListAsync();

    public async Task<Passenger> Update(Passenger entity)
    {
        context.Passengers.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}