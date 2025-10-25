using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class AircraftFamilyEfCoreRepository(AirlineDbContext context) : IRepository<AircraftFamily, int>
{
    public async Task<AircraftFamily> Create(AircraftFamily entity)
    {
        var result = await context.Families.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Families.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Families.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<AircraftFamily?> Read(int entityId) =>
        await context.Families
            .Include(af => af.Models)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<AircraftFamily>> ReadAll() =>
        await context.Families
            .Include(af => af.Models)
            .ToListAsync();

    public async Task<AircraftFamily> Update(AircraftFamily entity)
    {
        context.Families.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}