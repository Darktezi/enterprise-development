using Airline.Application.Contracts;

namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// Сервис для работы с билетами
/// </summary>
public interface ITicketService : IApplicationService<TicketDto, TicketCreateUpdateDto, int>
{
    /// <summary>
    /// Получает все билеты для указанного рейса
    /// </summary>
    /// <param name="flightId">Идентификатор рейса</param>
    /// <returns>Список билетов на рейс</returns>
    public Task<List<TicketDto>> GetTicketsByFlightAsync(int flightId);

    /// <summary>
    /// Получает все билеты для указанного пассажира
    /// </summary>
    /// <param name="passengerId">Идентификатор пассажира</param>
    /// <returns>Список билетов пассажира</returns>
    public Task<List<TicketDto>> GetTicketsByPassengerAsync(int passengerId);
}