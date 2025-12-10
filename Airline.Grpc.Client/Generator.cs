using Bogus;
using Airline.Api.Grpc;

namespace Airline.Grpc.Client;

/// <summary>
/// Утилитарный класс для генерации случайных запросов на билеты.
/// Использует библиотеку Bogus для создания реалистичных тестовых данных.
/// </summary>
public class Generator
{
    /// <summary>
    /// Внутренний экземпляр Faker для генерации реалистичных случайных данных
    /// таких как номера мест, идентификаторы рейсов и пассажиров, вес багажа.
    /// </summary>
    private readonly Faker _faker = new("ru");

    /// <summary>
    /// Генерирует случайный запрос на создание билета.
    /// </summary>
    /// <param name="maxFlightId">Максимальный ID рейса для случайного выбора.</param>
    /// <param name="maxPassengerId">Максимальный ID пассажира для случайного выбора.</param>
    public TicketRequest GenerateRandomTicket(int maxFlightId = 10, int maxPassengerId = 10)
    {
        return new TicketRequest
        {
            FlightId = _faker.Random.Int(1, maxFlightId),
            PassengerId = _faker.Random.Int(1, maxPassengerId),
            SeatNumber = $"{_faker.Random.Int(1, 30)}{_faker.PickRandom("A", "B", "C", "D", "E", "F")}",
            HasHandLuggage = _faker.Random.Bool(0.7f),
            BaggageWeight = _faker.Random.Bool(0.6f)
                ? Math.Round(_faker.Random.Double(5, 30), 2) : 0
        };
    }
}
