using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class TicketEfCoreRepository(AirlineDbContext context) : IRepository<Ticket, int>
{
    public async Task<Ticket> Create(Ticket entity)
    {
        var result = await context.Tickets.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Tickets.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Tickets.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Ticket?> Read(int entityId) =>
        await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Ticket>> ReadAll() =>
        await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .ToListAsync();

    public async Task<Ticket> Update(Ticket entity)
    {
        context.Tickets.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}