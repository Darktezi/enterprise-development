using Airline.Application.Contracts;
using Airline.Application.Contracts.Passenger;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для выполнения бизнес-логики, связанной с пассажирами.
/// Предоставляет методы для CRUD-операций и анализа пассажиров.
/// </summary>
public class PassengerService(
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper) : IPassengerService
{

    /// <summary>
    /// Создание нового пассажира
    /// </summary>
    /// <param name="dto">DTO с данными для создания пассажира</param>
    /// <returns>Созданный пассажир</returns>
    public async Task<PassengerDto> Create(PassengerCreateUpdateDto dto)
    {
        var newPassenger = mapper.Map<Passenger>(dto);
        var res = await passengerRepository.Create(newPassenger);
        return mapper.Map<PassengerDto>(res);
    }

    /// <summary>
    /// Удаление пассажира по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор пассажира</param>
    /// <returns>True если удаление успешно, иначе False</returns>
    public async Task<bool> Delete(int dtoId) =>
        await passengerRepository.Delete(dtoId);

    /// <summary>
    /// Получение пассажира по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор пассажира</param>
    /// <returns>DTO пассажира или null если не найдено</returns>
    public async Task<PassengerDto?> Get(int dtoId) =>
        mapper.Map<PassengerDto>(await passengerRepository.Read(dtoId));

    /// <summary>
    /// Получение всех пассажиров
    /// </summary>
    /// <returns>Список всех пассажиров</returns>
    public async Task<IList<PassengerDto>> GetAll() =>
        mapper.Map<List<PassengerDto>>(await passengerRepository.ReadAll());

    /// <summary>
    /// Обновление данных пассажира
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор обновляемого пассажира</param>
    /// <returns>Обновленный пассажир</returns>
    public async Task<PassengerDto> Update(PassengerCreateUpdateDto dto, int dtoId)
    {
        var existingPassenger = await passengerRepository.Read(dtoId);
        if (existingPassenger == null)
            throw new KeyNotFoundException($"Passenger with id {dtoId} not found");

        mapper.Map(dto, existingPassenger);
        var res = await passengerRepository.Update(existingPassenger);
        return mapper.Map<PassengerDto>(res);
    }

    /// <summary>
    /// Возвращает список пассажиров указанного авиарейса, у которых отсутствует зарегистрированный багаж
    /// (т.е. вес багажа равен 0 кг).
    /// </summary>
    /// <param name="flightId">Уникальный идентификатор авиарейса.</param>
    /// <returns>Список DTO пассажиров, отсортированный по фамилии и имени.</returns>
    public async Task<List<PassengerDto>> GetPassengersWithoutBaggageAsync(int flightId)
    {
        var passengers = await passengerRepository.ReadAll();
        var passengersWithoutBaggage = passengers
            .Where(p => p.Tickets?.Any(t => t.FlightId == flightId && t.BaggageWeight == 0) == true)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToList();

        return mapper.Map<List<PassengerDto>>(passengersWithoutBaggage);
    }
}