using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с пассажирами.
/// </summary>
public class PassengerEfCoreRepository(AirlineDbContext context)
    : GenericEfCoreRepository<Passenger, int>(context)
{
    /// <summary>
    /// Получает пассажира по идентификатору, включая все связанные с ним билеты.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого пассажира</param>
    /// <returns>Найденный пассажир с его билетами или null, если не найден</returns>
    public override async Task<Passenger?> Read(int entityId) =>
        await _dbSet
            .Include(p => p.Tickets)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех пассажиров, включая для каждого связанные билеты.
    /// </summary>
    /// <returns>Список всех пассажиров с их билетами</returns>
    public override async Task<IList<Passenger>> ReadAll() =>
        await _dbSet
            .Include(p => p.Tickets)
            .ToListAsync();
}