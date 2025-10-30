using Airline.Application.Contracts;
using Airline.Application.Contracts.Model;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над моделями самолетов
/// </summary>
/// <param name="modelRepository">Репозиторий моделей самолетов</param>
/// <param name="mapper">Профиль маппинга</param>
public class ModelService(IRepository<AirlineModel, int> modelRepository, IMapper mapper)
    : IApplicationService<ModelDto, ModelCreateUpdateDto, int>
{
    /// <summary>
    /// Создание новой модели самолета
    /// </summary>
    /// <param name="dto">DTO с данными для создания модели</param>
    /// <returns>Созданная модель самолета</returns>
    public async Task<ModelDto> Create(ModelCreateUpdateDto dto)
    {
        var newModel = mapper.Map<AirlineModel>(dto);
        var res = await modelRepository.Create(newModel);
        return mapper.Map<ModelDto>(res);
    }

    /// <summary>
    /// Удаление модели самолета по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор модели</param>
    /// <returns>True если удаление успешно, иначе False</returns>
    public async Task<bool> Delete(int dtoId) =>
        await modelRepository.Delete(dtoId);

    /// <summary>
    /// Получение модели самолета по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор модели</param>
    /// <returns>DTO модели самолета или null если не найдено</returns>
    public async Task<ModelDto?> Get(int dtoId) =>
        mapper.Map<ModelDto>(await modelRepository.Read(dtoId));

    /// <summary>
    /// Получение всех моделей самолетов
    /// </summary>
    /// <returns>Список всех моделей самолетов</returns>
    public async Task<IList<ModelDto>> GetAll() =>
        mapper.Map<List<ModelDto>>(await modelRepository.ReadAll());

    /// <summary>
    /// Обновление данных модели самолета
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор обновляемой модели</param>
    /// <returns>Обновленная модель самолета</returns>
    public async Task<ModelDto> Update(ModelCreateUpdateDto dto, int dtoId)
    {
        var existingModel = await modelRepository.Read(dtoId);
        if (existingModel == null)
            throw new KeyNotFoundException($"Model with id {dtoId} not found");

        mapper.Map(dto, existingModel);
        var res = await modelRepository.Update(existingModel);
        return mapper.Map<ModelDto>(res);
    }
}