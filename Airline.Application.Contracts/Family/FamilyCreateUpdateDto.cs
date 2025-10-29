using Airline.Application.Contracts.Model;

namespace Airline.Application.Contracts.Family;

/// <summary>
/// DTO для PUT/POST запросов к семействам самолётов.
/// </summary>
/// <param name="Name">Название семейства самолётов (например, "Boeing 737")</param>
/// <param name="Manufacturer">Производитель самолётов данного семейства (например, "Boeing")</param>
public record FamilyCreateUpdateDto(
    string? Name,
    string? Manufacturer
);
