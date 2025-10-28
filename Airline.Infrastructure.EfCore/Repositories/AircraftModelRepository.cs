using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с моделями воздушных судов (AirlineModel) на основе Entity Framework Core.
/// Реализует интерфейс IRepository для операций создания, чтения, обновления и удаления.
/// При чтении автоматически подгружает связанное семейство самолётов и список рейсов.
/// </summary>
public class AircraftModelEfCoreRepository(AirlineDbContext context) : IRepository<AirlineModel, int>
{
    /// <summary>
    /// Создаёт новую модель воздушного судна в базе данных.
    /// </summary>
    /// <param name="entity">Сущность модели для сохранения.</param>
    /// <returns>Созданная модель с присвоенным идентификатором.</returns>
    public async Task<AirlineModel> Create(AirlineModel entity)
    {
        var result = await context.Models.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Удаляет модель воздушного судна по её идентификатору.
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемой модели.</param>
    /// <returns>True, если модель была найдена и удалена; иначе false.</returns>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Models.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Models.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Получает модель воздушного судна по идентификатору, включая связанное семейство и все рейсы, использующие эту модель.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемой модели.</param>
    /// <returns>Найденная модель или null, если не найдена.</returns>
    public async Task<AirlineModel?> Read(int entityId) =>
        await context.Models
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех моделей воздушных судов, включая для каждой связанное семейство и рейсы.
    /// </summary>
    /// <returns>Список всех моделей.</returns>
    public async Task<IList<AirlineModel>> ReadAll() =>
        await context.Models
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .ToListAsync();

    /// <summary>
    /// Обновляет существующую модель воздушного судна в базе данных.
    /// </summary>
    /// <param name="entity">Обновлённая сущность модели.</param>
    /// <returns>Обновлённая модель с актуальными данными из базы, включая связанные семейство и рейсы.</returns>
    public async Task<AirlineModel> Update(AirlineModel entity)
    {
        context.Models.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}