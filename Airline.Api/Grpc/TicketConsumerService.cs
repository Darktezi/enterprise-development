using Airline.Grpc.Contracts;
using Airline.Domain;
using Airline.Domain.Entities;
using Grpc.Core;
using Grpc.Net.Client;

namespace Airline.Api.Grpc;

/// <summary>
/// Фоновый сервис-потребитель билетов, работающий как gRPC клиент.
/// Подключается к удалённому генератору билетов, получает поток данных
/// и сохраняет их в базу данных с отправкой статусов обработки.
/// </summary>
public class TicketConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TicketConsumerService> _logger;
    private readonly IConfiguration _config;
    private readonly string _generatorUrl;
    private readonly int _batchSize;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса-потребителя билетов.
    /// </summary>
    /// <param name="scopeFactory">Фабрика для создания scope зависимостей с ограниченным временем жизни.</param>
    /// <param name="logger">Логгер для записи событий работы сервиса.</param>
    /// <param name="config">Конфигурация приложения для получения URL генератора.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если GeneratorGrpcUrl не настроен.</exception>
    public TicketConsumerService(
        IServiceScopeFactory scopeFactory,
        ILogger<TicketConsumerService> logger,
        IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _config = config;
        _generatorUrl = config["GeneratorGrpcUrl"] ?? throw new InvalidOperationException("GeneratorGrpcUrl not configured");
        _batchSize = config.GetValue<int?>("TicketBatchSize") ?? 10;
    }

    /// <summary>
    /// Основной метод выполнения фонового сервиса.
    /// Устанавливает соединение с генератором, получает поток билетов
    /// и обрабатывает каждый билет с сохранением в БД.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для корректного завершения работы сервиса.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Подключение к генератору билетов: {Url}, размер батча: {BatchSize}", _generatorUrl, _batchSize);

        var channel = GrpcChannel.ForAddress(_generatorUrl);
        var client = new TicketGenerator.TicketGeneratorClient(channel);

        try
        {
            using var call = client.StreamTickets(cancellationToken: stoppingToken);

            var ticketBatch = new List<(TicketResponse Data, bool WaitingForResponse)>();

            var receiveTask = Task.Run(async () =>
            {
                await foreach (var ticket in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    _logger.LogInformation("Получен билет: Рейс={FlightId}, Пассажир={PassengerId}, Место={Seat}",
                        ticket.FlightId, ticket.PassengerId, ticket.SeatNumber);

                    ticketBatch.Add((ticket, true));

                    if (ticketBatch.Count >= _batchSize)
                    {
                        await ProcessBatch(ticketBatch, call.RequestStream);
                        ticketBatch.Clear();
                    }
                }

                if (ticketBatch.Count > 0)
                {
                    await ProcessBatch(ticketBatch, call.RequestStream);
                }

            }, stoppingToken);

            await receiveTask;
            await call.RequestStream.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при работе с генератором билетов");
        }
        finally
        {
            await channel.ShutdownAsync();
        }
    }

    /// <summary>
    /// Обрабатывает батч билетов - валидирует и сохраняет в БД,
    /// отправляя статусы обратно генератору.
    /// </summary>
    /// <param name="batch">Список билетов для обработки.</param>
    /// <param name="responseWriter">Поток для отправки статусов генератору.</param>
    private async Task ProcessBatch(
        List<(TicketResponse Data, bool WaitingForResponse)> batch,
        IClientStreamWriter<TicketCallback> responseWriter)
    {
        using var scope = _scopeFactory.CreateScope();
        var ticketRepo = scope.ServiceProvider.GetRequiredService<IRepository<Ticket, int>>();
        var flightRepo = scope.ServiceProvider.GetRequiredService<IRepository<Flight, int>>();
        var passengerRepo = scope.ServiceProvider.GetRequiredService<IRepository<Passenger, int>>();

        var successCount = 0;
        var errorCount = 0;

        foreach (var (ticketData, _) in batch)
        {
            var callback = await ValidateAndSaveTicket(
                ticketData,
                ticketRepo,
                flightRepo,
                passengerRepo);

            if (callback.Success)
                successCount++;
            else
                errorCount++;

            await responseWriter.WriteAsync(callback);
        }

        _logger.LogInformation("→ Обработан батч: успешно={Success}, ошибок={Errors}", successCount, errorCount);
    }

    /// <summary>
    /// Валидирует и сохраняет один билет.
    /// </summary>
    /// <param name="ticketData">Данные билета от генератора.</param>
    /// <param name="ticketRepo">Репозиторий билетов.</param>
    /// <param name="flightRepo">Репозиторий рейсов.</param>
    /// <param name="passengerRepo">Репозиторий пассажиров.</param>
    /// <returns>Статус обработки билета.</returns>
    private async Task<TicketCallback> ValidateAndSaveTicket(
        TicketResponse ticketData,
        IRepository<Ticket, int> ticketRepo,
        IRepository<Flight, int> flightRepo,
        IRepository<Passenger, int> passengerRepo)
    {
        try
        {
            var flight = await flightRepo.Read(ticketData.FlightId);
            if (flight == null)
            {
                _logger.LogWarning("Рейс {FlightId} не найден", ticketData.FlightId);
                return new TicketCallback { Success = false, Error = "Рейс не найден" };
            }

            var passenger = await passengerRepo.Read(ticketData.PassengerId);
            if (passenger == null)
            {
                _logger.LogWarning("Пассажир {PassengerId} не найден", ticketData.PassengerId);
                return new TicketCallback { Success = false, Error = "Пассажир не найден" };
            }

            var ticket = new Ticket
            {
                Id = 0,
                FlightId = ticketData.FlightId,
                PassengerId = ticketData.PassengerId,
                SeatNumber = ticketData.SeatNumber,
                HasHandLuggage = ticketData.HasHandLuggage,
                BaggageWeight = ticketData.BaggageWeight
            };

            await ticketRepo.Create(ticket);
            return new TicketCallback { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сохранения билета");
            return new TicketCallback { Success = false, Error = ex.Message };
        }
    }
}
