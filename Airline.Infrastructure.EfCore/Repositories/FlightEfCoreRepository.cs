using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с авиарейсами.
/// </summary>
public class FlightEfCoreRepository(AirlineDbContext context)
    : GenericEfCoreRepository<Flight, int>(context)
{
    /// <summary>
    /// Получает авиарейс по идентификатору, включая связанную модель воздушного судна и все билеты на этот рейс.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого рейса</param>
    /// <returns>Найденный рейс с моделью самолета и билетами или null, если не найден</returns>
    public override async Task<Flight?> Read(int entityId) =>
        await _dbSet
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех авиарейсов, включая для каждого связанную модель самолёта и билеты.
    /// </summary>
    /// <returns>Список всех рейсов с моделями самолетов и билетами</returns>
    public override async Task<IList<Flight>> ReadAll() =>
        await _dbSet
            .Include(f => f.AircraftModel)
            .Include(f => f.Tickets)
            .ToListAsync();
}