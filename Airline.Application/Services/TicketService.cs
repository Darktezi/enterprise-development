using Airline.Application.Contracts;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над билетами
/// </summary>
/// <param name="ticketRepository">Репозиторий билетов</param>
/// <param name="mapper">Профиль маппинга</param>
public class TicketService(IRepository<Ticket, int> ticketRepository, IMapper mapper)
    : ITicketService, IApplicationService<TicketDto, TicketCreateUpdateDto, int>
{
    /// <summary>
    /// Создание нового билета
    /// </summary>
    /// <param name="dto">DTO с данными для создания билета</param>
    /// <returns>Созданный билет</returns>
    public async Task<TicketDto> Create(TicketCreateUpdateDto dto)
    {
        var newTicket = mapper.Map<Ticket>(dto);
        var res = await ticketRepository.Create(newTicket);
        return mapper.Map<TicketDto>(res);
    }

    /// <summary>
    /// Удаление билета по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор билета</param>
    /// <returns>True если удаление успешно, иначе False</returns>
    public async Task<bool> Delete(int dtoId) =>
        await ticketRepository.Delete(dtoId);

    /// <summary>
    /// Получение билета по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор билета</param>
    /// <returns>DTO билета или null если не найдено</returns>
    public async Task<TicketDto?> Get(int dtoId) =>
        mapper.Map<TicketDto>(await ticketRepository.Read(dtoId));

    /// <summary>
    /// Получение всех билетов
    /// </summary>
    /// <returns>Список всех билетов</returns>
    public async Task<IList<TicketDto>> GetAll() =>
        mapper.Map<List<TicketDto>>(await ticketRepository.ReadAll());

    /// <summary>
    /// Обновление данных билета
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор обновляемого билета</param>
    /// <returns>Обновленный билет</returns>
    public async Task<TicketDto> Update(TicketCreateUpdateDto dto, int dtoId)
    {
        var existingTicket = await ticketRepository.Read(dtoId);
        if (existingTicket == null)
            throw new KeyNotFoundException($"Ticket with id {dtoId} not found");

        mapper.Map(dto, existingTicket);
        var res = await ticketRepository.Update(existingTicket);
        return mapper.Map<TicketDto>(res);
    }

    /// <summary>
    /// Получает все билеты для указанного рейса
    /// </summary>
    /// <param name="flightId">Идентификатор рейса</param>
    /// <returns>Список билетов на рейс</returns>
    public async Task<List<TicketDto>> GetTicketsByFlightAsync(int flightId)
    {
        var tickets = await ticketRepository.ReadAll();
        var flightTickets = tickets
            .Where(t => t.FlightId == flightId)
            .ToList();
            
        return mapper.Map<List<TicketDto>>(flightTickets);
    }

    /// <summary>
    /// Получает все билеты для указанного пассажира
    /// </summary>
    /// <param name="passengerId">Идентификатор пассажира</param>
    /// <returns>Список билетов пассажира</returns>
    public async Task<List<TicketDto>> GetTicketsByPassengerAsync(int passengerId)
    {
        var tickets = await ticketRepository.ReadAll();
        var passengerTickets = tickets
            .Where(t => t.PassengerId == passengerId)
            .ToList();
            
        return mapper.Map<List<TicketDto>>(passengerTickets);
    }
}