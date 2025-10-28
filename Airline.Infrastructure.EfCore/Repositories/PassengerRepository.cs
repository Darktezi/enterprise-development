    using Airline.Domain;
    using Airline.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с пассажирами (Passenger) на основе Entity Framework Core.
/// Реализует интерфейс IRepository для операций создания, чтения, обновления и удаления.
/// При чтении автоматически подгружает связанные билеты пассажира.
/// </summary>
public class PassengerEfCoreRepository(AirlineDbContext context) : IRepository<Passenger, int>
{
    /// <summary>
    /// Создаёт нового пассажира в базе данных.
    /// </summary>
    /// <param name="entity">Сущность пассажира для сохранения.</param>
    /// <returns>Созданный пассажир с присвоенным идентификатором.</returns>
    public async Task<Passenger> Create(Passenger entity)
        {
            var result = await context.Passengers.AddAsync(entity);
            await context.SaveChangesAsync();
            return result.Entity;
        }

    /// <summary>
    /// Удаляет пассажира по его идентификатору.
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемого пассажира.</param>
    /// <returns>True, если пассажир был найден и удалён; иначе false.</returns>
    public async Task<bool> Delete(int entityId)
        {
            var entity = await context.Passengers.FirstOrDefaultAsync(e => e.Id == entityId);
            if (entity == null)
                return false;
            context.Passengers.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

    /// <summary>
    /// Получает пассажира по идентификатору, включая все связанные с ним билеты.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого пассажира.</param>
    /// <returns>Найденный пассажир или null, если не найден.</returns>
    public async Task<Passenger?> Read(int entityId) =>
        await context.Passengers
            .Include(p => p.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех пассажиров, включая для каждого связанные билеты.
    /// </summary>
    /// <returns>Список всех пассажиров.</returns>
    public async Task<IList<Passenger>> ReadAll() =>
        await context.Passengers
            .Include(p => p.Tickets)
            .ToListAsync();

    /// <summary>
    /// Обновляет данные существующего пассажира в базе данных.
    /// </summary>
    /// <param name="entity">Обновлённая сущность пассажира.</param>
    /// <returns>Обновлённый пассажир с актуальными данными из базы, включая связанные билеты.</returns>
    public async Task<Passenger> Update(Passenger entity)
    {
        context.Passengers.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}