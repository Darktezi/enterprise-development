using Airline.Application.Contracts;
using Airline.Application.Contracts.Family;
using Microsoft.AspNetCore.Mvc;

namespace Airline.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций над семействами самолетов
/// </summary>
/// <param name="crudService">Сервис семейств самолетов</param>
/// <param name="logger">Логгер</param>
public class FamiliesController(
    IApplicationService<FamilyDto, FamilyCreateUpdateDto, int> crudService,
    ILogger<FamiliesController> logger)
    : CrudControllerBase<FamilyDto, FamilyCreateUpdateDto, int>(crudService, logger);