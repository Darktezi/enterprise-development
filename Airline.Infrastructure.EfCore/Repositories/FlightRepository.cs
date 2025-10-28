using Airline.Domain;
using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с авиарейсами (Flight) на основе Entity Framework Core.
/// Реализует интерфейс IRepository для операций создания, чтения, обновления и удаления.
/// При чтении автоматически подгружает связанную модель самолёта и список билетов на рейс.
/// </summary>
public class FlightEfCoreRepository(AirlineDbContext context) : IRepository<Flight, int>
{
    /// <summary>
    /// Создаёт новый авиарейс в базе данных.
    /// </summary>
    /// <param name="entity">Сущность рейса для сохранения.</param>
    /// <returns>Созданный рейс с присвоенным идентификатором.</returns>
    public async Task<Flight> Create(Flight entity)
    {
        var result = await context.Flights.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Удаляет авиарейс по его идентификатору.
    /// </summary>
    /// <param name="entityId">Идентификатор удаляемого рейса.</param>
    /// <returns>True, если рейс был найден и удалён; иначе false.</returns>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Flights.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;
        context.Flights.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Получает авиарейс по идентификатору, включая связанную модель воздушного судна и все билеты на этот рейс.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого рейса.</param>
    /// <returns>Найденный рейс или null, если не найден.</returns>
    public async Task<Flight?> Read(int entityId) =>
        await context.Flights
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех авиарейсов, включая для каждого связанную модель самолёта и билеты.
    /// </summary>
    /// <returns>Список всех рейсов.</returns>
    public async Task<IList<Flight>> ReadAll() =>
        await context.Flights
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .ToListAsync();

    /// <summary>
    /// Обновляет существующий авиарейс в базе данных.
    /// </summary>
    /// <param name="entity">Обновлённая сущность рейса.</param>
    /// <returns>Обновлённый рейс с актуальными данными из базы, включая связанную модель и билеты.</returns>
    public async Task<Flight> Update(Flight entity)
    {
        context.Flights.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }
}