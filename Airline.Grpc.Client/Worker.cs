using Airline.Api.Grpc;
using Grpc.Core;

namespace Airline.Grpc.Client;

/// <summary>
/// Фоновый воркер, отвечающий за периодическую генерацию и отправку
/// случайных билетов на gRPC сервер.
/// </summary>
/// <param name="client">gRPC клиент для коммуникации с сервером.</param>
/// <param name="logger">Логгер для структурированного логирования.</param>
/// <param name="config">Конфигурация с настройками воркера.</param>
public class Worker(
    TicketReceiver.TicketReceiverClient client,
    ILogger<Worker> logger,
    IConfiguration config) : BackgroundService
{
    /// <summary>
    /// Утилита для генерации случайных запросов на билеты.
    /// </summary>
    private readonly Generator _generator = new();

    /// <summary>
    /// Задержка между каждым сгенерированным запросом, настраивается через
    /// переменную окружения WorkerDelaySeconds.
    /// </summary>
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(
        config.GetValue<int?>("IntervalSeconds") ?? 1);

    /// <summary>
    /// Непрерывно генерирует и отправляет случайные билеты
    /// на сервер через streaming, и логирует ответы до отмены.
    /// </summary>
    /// <param name="stoppingToken">Токен для сигнализации отмены воркера.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Воркер генерации билетов запущен. Задержка между билетами: {Delay}", _delay);

        using var ticketStream = client.StreamTickets(cancellationToken: stoppingToken);

        var sendTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ticket = _generator.GenerateRandomTicket();
                logger.LogInformation("Отправка билета: Рейс={FlightId}, Пассажир={PassengerId}, Место={SeatNumber}, Багаж={BaggageWeight}кг",
                    ticket.FlightId, ticket.PassengerId, ticket.SeatNumber, ticket.BaggageWeight);

                await ticketStream.RequestStream.WriteAsync(ticket, stoppingToken);
                await Task.Delay(_delay, stoppingToken);
            }

            logger.LogInformation("Завершение отправки билетов");
            await ticketStream.RequestStream.CompleteAsync();

        }, stoppingToken);

        var receiveTask = Task.Run(async () =>
        {
            await foreach (var response in ticketStream.ResponseStream.ReadAllAsync(stoppingToken))
            {
                if (response.Success)
                {
                    logger.LogInformation("Билет успешно создан на сервере");
                }
                else
                {
                    logger.LogWarning("Ошибка создания билета: {Error}", response.Error);
                }
            }

            logger.LogInformation("Завершение получения ответов от сервера");
        }, stoppingToken);

        await Task.WhenAll(sendTask, receiveTask);
        logger.LogInformation("Воркер генерации билетов остановлен");
    }
}
