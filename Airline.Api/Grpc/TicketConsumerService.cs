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
    }

    /// <summary>
    /// Основной метод выполнения фонового сервиса.
    /// Устанавливает соединение с генератором, получает поток билетов
    /// и обрабатывает каждый билет с сохранением в БД.
    /// </summary>
    /// <param name="stoppingToken">Токен отмены для корректного завершения работы сервиса.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Подключение к генератору билетов: {Url}", _generatorUrl);

        // Создаём gRPC канал к генератору
        var channel = GrpcChannel.ForAddress(_generatorUrl);
        var client = new TicketGenerator.TicketGeneratorClient(channel);

        try
        {
            using var call = client.StreamTickets(cancellationToken: stoppingToken);

            // Задача получения билетов от генератора
            var receiveTask = Task.Run(async () =>
            {
                await foreach (var ticket in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    _logger.LogInformation("← Получен билет: Рейс={FlightId}, Пассажир={PassengerId}, Место={Seat}",
                        ticket.FlightId, ticket.PassengerId, ticket.SeatNumber);

                    var callback = await SaveTicketToDatabase(ticket);
                    await call.RequestStream.WriteAsync(callback, stoppingToken);
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
    /// Сохраняет данные билета в базу данных после проверки существования
    /// связанных сущностей (рейса и пассажира).
    /// </summary>
    /// <param name="ticketData">Данные билета, полученные от генератора.</param>
    /// <returns>
    /// Объект <see cref="TicketCallback"/> с результатом обработки:
    /// </returns>
    private async Task<TicketCallback> SaveTicketToDatabase(TicketResponse ticketData)
    {
        using var scope = _scopeFactory.CreateScope();
        var ticketRepo = scope.ServiceProvider.GetRequiredService<IRepository<Ticket, int>>();
        var flightRepo = scope.ServiceProvider.GetRequiredService<IRepository<Flight, int>>();
        var passengerRepo = scope.ServiceProvider.GetRequiredService<IRepository<Passenger, int>>();

        try
        {
            // Проверяем существование рейса и пассажира
            var flight = await flightRepo.Read(ticketData.FlightId);
            var passenger = await passengerRepo.Read(ticketData.PassengerId);

            if (flight == null)
            {
                _logger.LogWarning("Рейс {FlightId} не найден", ticketData.FlightId);
                return new TicketCallback { Success = false, Error = "Рейс не найден" };
            }

            if (passenger == null)
            {
                _logger.LogWarning("Пассажир {PassengerId} не найден", ticketData.PassengerId);
                return new TicketCallback { Success = false, Error = "Пассажир не найден" };
            }

            // Создаём и сохраняем билет
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

            _logger.LogInformation("→ Билет успешно сохранён в БД");
            return new TicketCallback { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка сохранения билета");
            return new TicketCallback { Success = false, Error = ex.Message };
        }
    }
}
