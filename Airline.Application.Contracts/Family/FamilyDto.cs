using Airline.Application.Contracts.Model;

namespace Airline.Application.Contracts.Family;

/// <summary>
/// DTO для передачи данных о семействе самолётов
/// </summary>
/// <param name="Id">Уникальный идентификатор семейства самолётов</param>
/// <param name="Name">Название семейства самолётов (например, "Boeing 737")</param>
/// <param name="Manufacturer">Производитель самолётов данного семейства (например, "Boeing")</param>
/// <param name="Models">Список моделей самолётов, относящихся к этому семейству</param>
public record FamilyDto(
    int Id,
    string? Name,
    string? Manufacturer,
    List<ModelDto>? Models
);
