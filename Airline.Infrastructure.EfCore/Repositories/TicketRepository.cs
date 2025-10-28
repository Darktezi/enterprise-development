using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с билетами (Ticket) на основе Entity Framework Core.
/// Реализует интерфейс IRepository для операций создания, чтения, обновления и удаления.
/// При чтении автоматически подгружает связанный рейс и пассажира.
/// </summary>
public class TicketEfCoreRepository(AirlineDbContext context) : IRepository<Ticket, int>
{
    /// <summary>
    /// Создаёт новый билет в базе данных.
    /// </summary>
    /// <param name="entity">Сущность билета для сохранения.</param>
    /// <returns>Созданный билет с присвоенным идентификатором.</returns>
    public async Task<Ticket> Create(Ticket entity)
    {
        var result = await context.Tickets.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Удаляет билет по его идентификатору.
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемого билета.</param>
    /// <returns>True, если билет был найден и удалён; иначе false.</returns>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Tickets.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Tickets.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Получает билет по идентификатору, включая связанный рейс и пассажира.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого билета.</param>
    /// <returns>Найденный билет или null, если не найден.</returns>
    public async Task<Ticket?> Read(int entityId) =>
        await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех билетов, включая для каждого связанный рейс и пассажира.
    /// </summary>
    /// <returns>Список всех билетов.</returns>
    public async Task<IList<Ticket>> ReadAll() =>
        await context.Tickets
            .Include(t => t.Flight)
            .Include(t => t.Passenger)
            .ToListAsync();

    /// <summary>
    /// Обновляет данные существующего билета в базе данных.
    /// </summary>
    /// <param name="entity">Обновлённая сущность билета.</param>
    /// <returns>Обновлённый билет с актуальными данными из базы, включая связанный рейс и пассажира.</returns>
    public async Task<Ticket> Update(Ticket entity)
    {
        context.Tickets.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}