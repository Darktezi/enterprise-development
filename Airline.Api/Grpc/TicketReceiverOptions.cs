namespace Airline.Api.Grpc;

/// <summary>
/// Параметры конфигурации для gRPC сервиса приема билетов
/// </summary>
public class TicketReceiverOptions
{
    /// <summary>
    /// Размер батча для сохранения билетов в БД
    /// По умолчанию: 50 билетов за раз
    /// </summary>
    public int BatchSize { get; set; } = 50;

    /// <summary>
    /// Максимальный размер payload в байтах
    /// По умолчанию: 5MB
    /// </summary>
    public int PayloadLimitBytes { get; set; } = 5 * 1024 * 1024;

    /// <summary>
    /// Максимальный размер входящего gRPC сообщения в байтах
    /// По умолчанию: 10MB
    /// </summary>
    public int MaxReceiveMessageSizeBytes { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Максимальный размер исходящего gRPC сообщения в байтах
    /// По умолчанию: 10MB
    /// </summary>
    public int MaxSendMessageSizeBytes { get; set; } = 10 * 1024 * 1024;
}
