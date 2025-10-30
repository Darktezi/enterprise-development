using Airline.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Универсальный репозиторий для работы с сущностями через Entity Framework Core
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public class GenericEfCoreRepository<TEntity, TKey>(AirlineDbContext context) : IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Набор данных для типа TEntity, предоставляющий методы для запросов к конкретной таблице базы данных.
    /// </summary>
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>
    /// Создает новую сущность в базе данных
    /// </summary>
    /// <param name="entity">Сущность для сохранения</param>
    /// <returns>Созданная сущность с присвоенным идентификатором</returns>
    public virtual async Task<TEntity> Create(TEntity entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Удаляет сущность по её идентификатору
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемой сущности</param>
    /// <returns>True, если сущность была найдена и удалена; иначе false</returns>
    public virtual async Task<bool> Delete(TKey entityId)
    {
        var entity = await _dbSet.FindAsync(entityId);
        if (entity == null)
            return false;
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Получает сущность по идентификатору
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемой сущности</param>
    /// <returns>Найденная сущность или null, если не найдена</returns>
    public virtual async Task<TEntity?> Read(TKey entityId) =>
        await _dbSet.FindAsync(entityId);

    /// <summary>
    /// Получает список всех сущностей
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    public virtual async Task<IList<TEntity>> ReadAll() =>
        await _dbSet.ToListAsync();

    /// <summary>
    /// Обновляет существующую сущность в базе данных
    /// </summary>
    /// <param name="entity">Обновлённая сущность</param>
    /// <returns>Обновлённая сущность с актуальными данными из базы</returns>
    public virtual async Task<TEntity> Update(TEntity entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}