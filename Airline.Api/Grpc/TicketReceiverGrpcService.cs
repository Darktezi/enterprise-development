using Airline.Domain.Entities;
using Airline.Domain;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Airline.Api.Grpc;

/// <summary>
/// gRPC сервис для приема билетов через streaming.
/// Обрабатывает входящий поток в батчах и отправляет ответ для каждого элемента.
/// </summary>
/// <param name="ticketRepo">Репозиторий для доступа и сохранения <see cref="Ticket"/> сущностей.</param>
/// <param name="flightRepo">Репозиторий для доступа к <see cref="Flight"/> сущностям.</param>
/// <param name="passengerRepo">Репозиторий для доступа к <see cref="Passenger"/> сущностям.</param>
/// <param name="logger">Logger для записи активности сервиса.</param>
/// <param name="optionsAccessor">Accessor для runtime опций контроля размера батча и лимитов payload.</param>
public class TicketReceiverGrpcService(
    IRepository<Ticket, int> ticketRepo,
    IRepository<Flight, int> flightRepo,
    IRepository<Passenger, int> passengerRepo,
    ILogger<TicketReceiverGrpcService> logger,
    IOptions<TicketReceiverOptions> optionsAccessor)
    : TicketReceiver.TicketReceiverBase
{
    /// <summary>
    /// Runtime опции конфигурации для контроля размера батча, лимитов payload,
    /// и лимитов размера gRPC сообщений.
    /// </summary>
    private readonly TicketReceiverOptions _options = optionsAccessor.Value;

    /// <summary>
    /// Проверяет размер входящего gRPC сообщения против настроенного лимита payload.
    /// </summary>
    /// <param name="msg">gRPC сообщение для валидации.</param>
    /// <param name="error">Сообщение об ошибке если валидация провалилась.</param>
    /// <returns>True если payload в пределах лимитов; иначе false.</returns>
    private bool ValidatePayload(IMessage msg, out string? error)
    {
        if (msg.CalculateSize() > _options.PayloadLimitBytes)
        {
            error = "Payload слишком большой";
            return false;
        }
        error = null;
        return true;
    }

    /// <summary>
    /// Сохраняет батч сущностей в репозиторий и логирует операцию.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <param name="batch">Список сущностей для сохранения.</param>
    /// <param name="repo">Репозиторий для сохранения.</param>
    /// <param name="name">Имя типа сущности для логирования.</param>
    private async Task SaveBatchAsync<T>(List<T> batch, IRepository<T, int> repo, string name)
        where T : class
    {
        foreach (var item in batch)
            await repo.Create(item);

        logger.LogInformation("Сохранен батч из {Count} {Type}", batch.Count, name);
        batch.Clear();
    }

    /// <summary>
    /// Принимает поток запросов на билеты, валидирует их, сохраняет батчами,
    /// и отправляет ответ для каждого запроса.
    /// </summary>
    /// <param name="requestStream">Входящий поток билетов.</param>
    /// <param name="responseStream">Исходящий поток ответов.</param>
    /// <param name="context">gRPC контекст вызова.</param>
    public override async Task StreamTickets(
        IAsyncStreamReader<TicketRequest> requestStream,
        IServerStreamWriter<TicketResponse> responseStream,
        ServerCallContext context)
    {
        var batch = new List<Ticket>();

        await foreach (var req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (!ValidatePayload(req, out var sizeError))
            {
                await responseStream.WriteAsync(new TicketResponse { Success = false, Error = sizeError });
                continue;
            }

            logger.LogInformation("Получен билет: {@Req}", req);

            if (req.FlightId <= 0 || req.PassengerId <= 0)
            {
                await responseStream.WriteAsync(new TicketResponse
                {
                    Success = false,
                    Error = "FlightId и PassengerId должны быть положительными."
                });
                continue;
            }

            var flight = await flightRepo.Read(req.FlightId);
            var passenger = await passengerRepo.Read(req.PassengerId);

            if (flight == null || passenger == null)
            {
                await responseStream.WriteAsync(new TicketResponse
                {
                    Success = false,
                    Error = "Рейс или пассажир не найдены."
                });
                continue;
            }

            batch.Add(new Ticket
            {
                Id = 0,
                FlightId = req.FlightId,
                PassengerId = req.PassengerId,
                SeatNumber = req.SeatNumber,
                HasHandLuggage = req.HasHandLuggage,
                BaggageWeight = req.BaggageWeight > 0 ? req.BaggageWeight : null
            });

            await responseStream.WriteAsync(new TicketResponse { Success = true });

            if (batch.Count >= _options.BatchSize)
                await SaveBatchAsync(batch, ticketRepo, "билетов");
        }

        if (batch.Count > 0)
            await SaveBatchAsync(batch, ticketRepo, "билетов");
    }
}
