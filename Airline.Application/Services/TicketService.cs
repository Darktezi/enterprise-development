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
    : IApplicationService<TicketDto, TicketCreateUpdateDto, int>
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
            throw new ArgumentException($"Ticket with id {dtoId} not found");

        mapper.Map(dto, existingTicket);
        var res = await ticketRepository.Update(existingTicket);
        return mapper.Map<TicketDto>(res);
    }
}