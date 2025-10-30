using Airline.Application.Contracts;
using Airline.Application.Contracts.Family;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над семействами самолетов
/// </summary>
/// <param name="familyRepository">Репозиторий семейств самолетов</param>
/// <param name="mapper">Профиль маппинга</param>
public class FamilyService(IRepository<AirlineFamily, int> familyRepository, IMapper mapper) : IApplicationService<FamilyDto, FamilyCreateUpdateDto, int>
{
    /// <summary>
    /// Создание нового семейства самолетов
    /// </summary>
    /// <param name="dto">DTO с данными для создания семейства</param>
    /// <returns>Созданное семейство самолетов</returns>
    public async Task<FamilyDto> Create(FamilyCreateUpdateDto dto)
    {
        var newFamily = mapper.Map<AirlineFamily>(dto);
        var res = await familyRepository.Create(newFamily);
        return mapper.Map<FamilyDto>(res);
    }

    /// <summary>
    /// Удаление семейства самолетов по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор семейства</param>
    /// <returns>True если удаление успешно, иначе False</returns>
    public async Task<bool> Delete(int dtoId) =>
        await familyRepository.Delete(dtoId);

    /// <summary>
    /// Получение семейства самолетов по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор семейства</param>
    /// <returns>DTO семейства самолетов или null если не найдено</returns>
    public async Task<FamilyDto?> Get(int dtoId) =>
        mapper.Map<FamilyDto>(await familyRepository.Read(dtoId));

    /// <summary>
    /// Получение всех семейств самолетов
    /// </summary>
    /// <returns>Список всех семейств самолетов</returns>
    public async Task<IList<FamilyDto>> GetAll() =>
        mapper.Map<List<FamilyDto>>(await familyRepository.ReadAll());

    /// <summary>
    /// Обновление данных семейства самолетов
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор обновляемого семейства</param>
    /// <returns>Обновленное семейство самолетов</returns>
    public async Task<FamilyDto> Update(FamilyCreateUpdateDto dto, int dtoId)
    {
        var existingFamily = await familyRepository.Read(dtoId);
        if (existingFamily == null)
            throw new KeyNotFoundException($"Family with id {dtoId} not found");

        mapper.Map(dto, existingFamily);

        var res = await familyRepository.Update(existingFamily);
        return mapper.Map<FamilyDto>(res);
    }
}