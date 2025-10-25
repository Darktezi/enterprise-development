using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class AircraftModelEfCoreRepository(AirlineDbContext context) : IRepository<AircraftModel, int>
{
    public async Task<AircraftModel> Create(AircraftModel entity)
    {
        var result = await context.Models.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Models.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Models.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<AircraftModel?> Read(int entityId) =>
        await context.Models
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<AircraftModel>> ReadAll() =>
        await context.Models
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .ToListAsync();

    public async Task<AircraftModel> Update(AircraftModel entity)
    {
        context.Models.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}