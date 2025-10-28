using Airline.Application.Contracts.Family;
using Airline.Application.Contracts.Flight;

namespace Airline.Application.Contracts.Model;

/// <summary>
/// DTO для передачи данных о модели самолёта
/// </summary>
/// <param name="Id">Уникальный идентификатор модели самолёта</param>
/// <param name="ModelName">Название модели самолёта (например, "Boeing 737")</param>
/// <param name="FamilyId">Идентификатор семейства воздушных судов, к которому принадлежит модель</param>
/// <param name="Family">Семейство самолёта</param>
/// <param name="FlightRange">Дальность полёта в километрах</param>
/// <param name="PassengerCapacity">Вместимость пассажиров</param>
/// <param name="CargoCapacity">Вместимость груза в тоннах</param>
/// <param name="Flights">Список рейсов, выполняемых с использованием этой модели самолёта</param>
public record ModelDto(
    int Id,
    string? ModelName,
    int? FamilyId,
    FamilyDto? Family,
    double? FlightRange,
    int? PassengerCapacity,
    double? CargoCapacity,
    List<FlightDto>? Flights
);
