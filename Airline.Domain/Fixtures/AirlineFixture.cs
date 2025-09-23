using Airline.Domain.Entities;

namespace Airline.Domain.Fixtures;
public class AirlineFixture
{
    public List<AircraftFamily> Families { get; } = [];
    public List<AircraftModel> Models { get; } = [];
    public List<Flight> Flights { get; } = [];
    public List<Passenger> Passengers { get; } = [];
    public List<Ticket> Tickets { get; } = [];

    private int _ticketId = 1;

    /// <summary>
    /// Конструктор инициализирует тестовый набор данных.
    /// Создает ровно по 10 сущностей для классов:
    /// <see cref="AircraftFamily"/>, <see cref="AircraftModel"/>, 
    /// <see cref="Flight"/>, <see cref="Passenger"/> и <see cref="Ticket"/>.
    /// Используется для юнит-тестов с предопределенными данными.
    /// </summary>
    public AirlineFixture()
    {
        Families.AddRange(Enumerable.Range(1, 10).Select(i => new AircraftFamily
        {
            Id = i,
            Name = $"Family-{i}",
            Manufacturer = i % 2 == 0 ? "Airbus" : "Boeing"
        }));

        Models.AddRange(Enumerable.Range(1, 10).Select(i => new AircraftModel
        {
            Id = i,
            ModelName = $"Model-{i}",
            Family = Families[(i - 1) % Families.Count],
            FlightRange = 3000 + i * 400,
            PassengerCapacity = 120 + i * 10,
            CargoCapacity = 9000 + i * 800
        }));

        foreach (var model in Models)
        {
            model.Family.Models ??= new List<AircraftModel>();
            model.Family.Models.Add(model);
        }

        Flights.AddRange(Enumerable.Range(1, 10).Select(i => new Flight
        {
            Id = i,
            Code = $"FL{i:000}",
            DepartureAirport = i % 2 == 0 ? "SVO" : "LED",
            ArrivalAirport = i % 2 == 0 ? "JFK" : "LHR",
            DepartureDate = new DateTime(2025, 9, i, 10, 0, 0),
            ArrivalDate = new DateTime(2025, 9, i, 13, 0, 0),
            TravelTime = TimeSpan.FromHours(3),
            AircraftModel = Models[(i - 1) % Models.Count]
        }));

        foreach (var flight in Flights)
        {
            flight.AircraftModel.Flights ??= new List<Flight>();
            flight.AircraftModel.Flights.Add(flight);
        }

        Passengers.AddRange(Enumerable.Range(1, 10).Select(i => new Passenger
        {
            Id = i,
            PassportNumber = $"P{i:000000}",
            FullName = $"Passenger-{i} Testovich",
            BirthDate = new DateOnly(1980 + (i % 20), (i % 12) + 1, (i % 28) + 1)
        }));

        for (var i = 0; i < 10; i++)
        {
            var flight = Flights[i];
            var passenger = Passengers[i];
            AddTicket(flight, passenger, $"{12 + i}A", i % 2 == 0, i); // багажа от 0 до 9 кг
        }
    }

    /// <summary>
    /// Добавляет билет и связывает его с рейсом и пассажиром.
    /// </summary>
    private void AddTicket(Flight flight, Passenger passenger, string seat, bool hasHandLuggage, double baggageWeight)
    {
        var ticket = new Ticket
        {
            Id = _ticketId++,
            Flight = flight ?? throw new ArgumentNullException(nameof(flight)),
            Passenger = passenger ?? throw new ArgumentNullException(nameof(passenger)),
            SeatNumber = seat ?? throw new ArgumentNullException(nameof(seat)),
            HasHandLuggage = hasHandLuggage,
            BaggageWeight = baggageWeight
        };

        Tickets.Add(ticket);

        flight.Tickets ??= new List<Ticket>();
        passenger.Tickets ??= new List<Ticket>();

        flight.Tickets.Add(ticket);
        passenger.Tickets.Add(ticket);
    }
}
