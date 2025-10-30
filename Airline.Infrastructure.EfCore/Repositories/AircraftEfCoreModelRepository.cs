using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с моделями воздушных судов.
/// </summary>
public class AircraftModelEfCoreRepository(AirlineDbContext context)
    : GenericEfCoreRepository<AirlineModel, int>(context)
{
    /// <summary>
    /// Получает модель воздушного судна по идентификатору, включая связанное семейство и все рейсы, использующие эту модель.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемой модели</param>
    /// <returns>Найденная модель с семейством и рейсами или null, если не найдена</returns>
    public override async Task<AirlineModel?> Read(int entityId) =>
        await _dbSet
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех моделей воздушных судов, включая для каждой связанное семейство и рейсы.
    /// </summary>
    /// <returns>Список всех моделей с их семействами и рейсами</returns>
    public override async Task<IList<AirlineModel>> ReadAll() =>
        await _dbSet
            .Include(am => am.Family)
            .Include(am => am.Flights)
            .ToListAsync();
}