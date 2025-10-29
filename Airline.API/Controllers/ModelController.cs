using Airline.Application.Contracts;
using Airline.Application.Contracts.Model;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над моделями самолетов
/// </summary>
/// <param name="crudService">Сервис моделей самолетов</param>
/// <param name="logger">Логгер</param>
public class ModelsController(
    IApplicationService<ModelDto, ModelCreateUpdateDto, int> crudService,
    ILogger<ModelsController> logger)
    : CrudControllerBase<ModelDto, ModelCreateUpdateDto, int>(crudService, logger);