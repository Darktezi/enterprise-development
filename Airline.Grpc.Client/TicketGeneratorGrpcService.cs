using Airline.Grpc.Contracts;
using Bogus;
using Grpc.Core;

namespace Airline.Grpc.Client;

/// <summary>
/// gRPC сервис-генератор билетов.
/// Генерирует случайные билеты и отправляет их клиентам (API).
/// </summary>
public class TicketGeneratorGrpcService(
    ILogger<TicketGeneratorGrpcService> logger,
    IConfiguration config) : TicketGenerator.TicketGeneratorBase
{
    private readonly Faker _faker = new("ru");
    private readonly int _delaySeconds = config.GetValue<int?>("GeneratorDelaySeconds") ?? 1;

    /// <summary>
    /// Потоковая генерация билетов.
    /// Клиент (API) подключается, генератор отправляет билеты, клиент возвращает статусы.
    /// </summary>
    public override async Task StreamTickets(
        IAsyncStreamReader<TicketCallback> requestStream,
        IServerStreamWriter<TicketResponse> responseStream,
        ServerCallContext context)
    {
        logger.LogInformation("Клиент подключился к генератору билетов");

        // Задача генерации и отправки билетов
        var generatorTask = Task.Run(async () =>
        {
            var generatedCount = 0;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                var ticket = GenerateRandomTicket();
                generatedCount++;

                logger.LogInformation("→ Генерация билета #{Count}: Рейс={FlightId}, Пассажир={PassengerId}, Место={Seat}",
                    generatedCount, ticket.FlightId, ticket.PassengerId, ticket.SeatNumber);

                await responseStream.WriteAsync(ticket, context.CancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(_delaySeconds), context.CancellationToken);
            }
        }, context.CancellationToken);

        // Задача приёма статусов от клиента
        var callbackTask = Task.Run(async () =>
        {
            await foreach (var callback in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (callback.Success)
                {
                    logger.LogInformation("← Клиент подтвердил успешное сохранение билета");
                }
                else
                {
                    logger.LogWarning("← Клиент сообщил об ошибке: {Error}", callback.Error);
                }
            }
        }, context.CancellationToken);

        try
        {
            await Task.WhenAll(generatorTask, callbackTask);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Клиент отключился от генератора");
        }
    }

    /// <summary>
    /// Метод генерации билетов.
    /// </summary>
    private TicketResponse GenerateRandomTicket()
    {
        return new TicketResponse
        {
            FlightId = _faker.Random.Int(1, 10),
            PassengerId = _faker.Random.Int(1, 10),
            SeatNumber = $"{_faker.Random.Int(1, 30)}{_faker.PickRandom("A", "B", "C", "D", "E", "F")}",
            HasHandLuggage = _faker.Random.Bool(0.7f),
            BaggageWeight = _faker.Random.Bool(0.8f) 
                ? Math.Round(_faker.Random.Double(5, 30), 2) : 0
        };
    }
}
