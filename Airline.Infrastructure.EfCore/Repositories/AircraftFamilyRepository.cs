using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с семействами воздушных судов (AirlineFamily) на основе Entity Framework Core.
/// Реализует интерфейс IRepository для операций создания, чтения, обновления и удаления.
/// </summary>
public class AircraftFamilyEfCoreRepository(AirlineDbContext context) : IRepository<AirlineFamily, int>
{
    /// <summary>
    /// Создаёт новое семейство воздушных судов в базе данных.
    /// </summary>
    /// <param name="entity">Сущность семейства для сохранения.</param>
    /// <returns>Созданная сущность с присвоенным идентификатором.</returns>
    public async Task<AirlineFamily> Create(AirlineFamily entity)
    {
        var result = await context.Families.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Удаляет семейство воздушных судов по его идентификатору.
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемого семейства.</param>
    /// <returns>True, если сущность была найдена и удалена; иначе false.</returns>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Families.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Families.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Получает семейство воздушных судов по идентификатору, включая связанные модели самолётов.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого семейства.</param>
    /// <returns>Найденная сущность или null, если не найдена.</returns>
    public async Task<AirlineFamily?> Read(int entityId) =>
        await context.Families
            .Include(af => af.Models)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех семейств воздушных судов, включая связанные модели самолётов.
    /// </summary>
    /// <returns>Список всех семейств.</returns>
    public async Task<IList<AirlineFamily>> ReadAll() =>
        await context.Families
            .Include(af => af.Models)
            .ToListAsync();

    /// <summary>
    /// Обновляет существующее семейство воздушных судов в базе данных.
    /// </summary>
    /// <param name="entity">Обновлённая сущность семейства.</param>
    /// <returns>Обновлённая сущность с актуальными данными из базы (включая связанные модели).</returns>
    public async Task<AirlineFamily> Update(AirlineFamily entity)
    {
        context.Families.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}