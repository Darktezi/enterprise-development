using Airline.Application.Contracts.Family;
using Airline.Application.Contracts.Flight;

namespace Airline.Application.Contracts.Model;

/// <summary>
/// DTO для PUT/POST запросов к моделям самолётов.
/// </summary>
/// <param name="ModelName">Название модели самолёта (например, "Boeing 737")</param>
/// <param name="FamilyId">Идентификатор семейства воздушных судов, к которому принадлежит модель</param>
/// <param name="FlightRange">Дальность полёта в километрах</param>
/// <param name="PassengerCapacity">Вместимость пассажиров</param>
/// <param name="CargoCapacity">Вместимость груза в тоннах</param>
public record ModelCreateUpdateDto(
    string? ModelName,
    int? FamilyId,
    double? FlightRange,
    int? PassengerCapacity,
    double? CargoCapacity
);
