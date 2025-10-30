using Airline.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Controllers;
/// <summary>
/// Базовый контроллер для CRUD-операций над сущностями
/// </summary>
/// <typeparam name="TDto">DTO для Get-запросов</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для Post/Put-запросов</typeparam>
/// <typeparam name="TKey">Тип идентификатора DTO</typeparam>
/// <param name="appService">Служба для манипуляции DTO</param>
/// <param name="logger">Логгер</param>
[Route("api/[controller]")]
[ApiController]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(IApplicationService<TDto, TCreateUpdateDto, TKey> appService,
    ILogger<CrudControllerBase<TDto, TCreateUpdateDto, TKey>> logger) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Добавление новой записи
    /// </summary>
    /// <param name="newDto">Новые данные</param>
    /// <returns>Добавленные данные</returns> 
    /// <response code="201">Сущность создана успешно.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpPost]
    public async Task<ActionResult<TDto>> Create(TCreateUpdateDto newDto)
    {
        return await ExecuteWithLogging(async () =>
        {
            var result = await appService.Create(newDto);
            return CreatedAtAction(nameof(Create), result);
        }, nameof(Create), newDto);
    }

    /// <summary>
    /// Изменение имеющихся данных
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="newDto">Измененные данные</param>
    /// <returns>Обновленные данные</returns>
    /// <response code="200">Сущность успешно отредактированна.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<TDto>> Edit(TKey id, TCreateUpdateDto newDto)
    {
        return await ExecuteWithLogging(async () =>
        {
            var result = await appService.Update(newDto, id);
            return Ok(result);
        }, nameof(Edit), id, newDto);
    }

    /// <summary>
    /// Удаление данных
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <response code="200">Сущность удалена успешно.</response>
    /// <response code="204">Сущность не найдена.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(TKey id)
    {
        return await ExecuteWithLogging(async () =>
        {
            var result = await appService.Delete(id);
            return result ? Ok() : NoContent();
        }, nameof(Delete), id);
    }

    /// <summary>
    /// Получение списка всех данных
    /// </summary>
    /// <returns>Список всех данных</returns>
    /// <response code="200">Сущности успешно получены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet]
    public async Task<ActionResult<IList<TDto>>> GetAll()
    {
        return await ExecuteWithLogging(async () =>
        {
            var result = await appService.GetAll();
            return Ok(result);
        }, nameof(GetAll));
    }

    /// <summary>
    /// Получение данных по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Данные</returns>
    /// <response code="200">Сущность получена успешно.</response>
    /// <response code="204">Сущность не найдена.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TDto>> Get(TKey id)
    {
        return await ExecuteWithLogging(async () =>
        {
            var result = await appService.Get(id);
            return result != null ? Ok(result) : NoContent();
        }, nameof(Get), id);
    }

    /// <summary>
    /// Выполняет операцию с логированием начала, успешного завершения и ошибок.
    /// </summary>
    /// <param name="operation">Асинхронная операция для выполнения</param>
    /// <param name="methodName">Название метода для логирования</param>
    /// <param name="parameters">Параметры метода для логирования</param>
    /// <returns>Результат выполнения операции или ошибку 500 в случае исключения</returns>
    private async Task<ActionResult> ExecuteWithLogging(Func<Task<ActionResult>> operation, string methodName, params object[] parameters)
    {
        logger.LogInformation("Метод {Method} вызван в {Controller} с параметрами: {@Params}",
            methodName, GetType().Name, parameters);

        try
        {
            var result = await operation();
            logger.LogInformation("Метод {Method} завершился успешно", methodName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Исключение в методе {Method}: {Exception}", methodName, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}