using Airline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для работы с семействами воздушных судов.
/// </summary>
public class AircraftFamilyEfCoreRepository(AirlineDbContext context)
    : GenericEfCoreRepository<AirlineFamily, int>(context)
{
    /// <summary>
    /// Получает семейство воздушных судов по идентификатору, включая все связанные модели самолетов.
    /// </summary>
    /// <param name="entityId">Идентификатор запрашиваемого семейства</param>
    /// <returns>Найденное семейство с коллекцией моделей или null, если не найдено</returns>
    public override async Task<AirlineFamily?> Read(int entityId) =>
        await _dbSet
            .Include(af => af.Models)
            .FirstOrDefaultAsync(e => e.Id == entityId);

    /// <summary>
    /// Получает список всех семейств воздушных судов, включая для каждого все связанные модели.
    /// </summary>
    /// <returns>Список всех семейств с их моделями</returns>
    public override async Task<IList<AirlineFamily>> ReadAll() =>
        await _dbSet
            .Include(af => af.Models)
            .ToListAsync();
}