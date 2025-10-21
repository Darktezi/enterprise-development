using Airline.Domain.Entities;

namespace Airline.Domain.Data;

/// <summary>
/// Класс, представляющий набор исходных данных для авиакомпании.
/// Содержит коллекции всех сущностей: семейства самолетов, модели, рейсы, пассажиры и билеты.
/// </summary>
public class AirlineSeed
{
    public static readonly List<AircraftFamily> Families = [
        new AircraftFamily { Id = 1, Name = "Boeing 737", Manufacturer = "Boeing" },
        new AircraftFamily { Id = 2, Name = "Airbus A320", Manufacturer = "Airbus" },
        new AircraftFamily { Id = 3, Name = "Boeing 777", Manufacturer = "Boeing" },
        new AircraftFamily { Id = 4, Name = "Airbus A330", Manufacturer = "Airbus" },
        new AircraftFamily { Id = 5, Name = "Embraer E190", Manufacturer = "Embraer" },
        new AircraftFamily { Id = 6, Name = "Bombardier CS300", Manufacturer = "Bombardier" },
        new AircraftFamily { Id = 7, Name = "Boeing 787", Manufacturer = "Boeing" },
        new AircraftFamily { Id = 8, Name = "Airbus A350", Manufacturer = "Airbus" },
        new AircraftFamily { Id = 9, Name = "Sukhoi Superjet 100", Manufacturer = "Sukhoi" },
        new AircraftFamily { Id = 10, Name = "Comac C919", Manufacturer = "Comac" }];

    public static readonly List<AircraftModel> Models = [
        new AircraftModel { Id = 1, ModelName = "737-800", FlightRange = 5000, PassengerCapacity = 160, CargoCapacity = 8000, FamilyId = 1 },
        new AircraftModel { Id = 2, ModelName = "A320-200", FlightRange = 6100, PassengerCapacity = 150, CargoCapacity = 7500, FamilyId = 2 },
        new AircraftModel { Id = 3, ModelName = "777-300ER", FlightRange = 13600, PassengerCapacity = 300, CargoCapacity = 18000, FamilyId = 3 },
        new AircraftModel { Id = 4, ModelName = "A330-300", FlightRange = 11700, PassengerCapacity = 280, CargoCapacity = 15000, FamilyId = 4 },
        new AircraftModel { Id = 5, ModelName = "E190", FlightRange = 4000, PassengerCapacity = 100, CargoCapacity = 4000, FamilyId = 5 },
        new AircraftModel { Id = 6, ModelName = "CS300", FlightRange = 3600, PassengerCapacity = 130, CargoCapacity = 5000, FamilyId = 6 },
        new AircraftModel { Id = 7, ModelName = "787-9", FlightRange = 14100, PassengerCapacity = 280, CargoCapacity = 16000, FamilyId = 7 },
        new AircraftModel { Id = 8, ModelName = "A350-900", FlightRange = 15000, PassengerCapacity = 300, CargoCapacity = 17000, FamilyId = 8 },
        new AircraftModel { Id = 9, ModelName = "SSJ100", FlightRange = 3000, PassengerCapacity = 98, CargoCapacity = 3500, FamilyId = 9 },
        new AircraftModel { Id = 10, ModelName = "C919", FlightRange = 4000, PassengerCapacity = 156, CargoCapacity = 7000, FamilyId = 10 }];

    public static readonly List<Flight> Flights = [
        new Flight
        {
            Id = 1,
            Code = "SU1001",
            DepartureAirport = "SVO",
            ArrivalAirport = "JFK",
            DepartureDate = new DateTime(2025,9,1,10,0,0),
            ArrivalDate = new DateTime(2025,9,1,14,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 1
        },
        new Flight
        {
            Id = 2,
            Code = "SU1002",
            DepartureAirport = "LED",
            ArrivalAirport = "LHR",
            DepartureDate = new DateTime(2025,9,2,12,0,0),
            ArrivalDate = new DateTime(2025,9,2,16,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 2
        },
        new Flight
        {
            Id = 3,
            Code = "SU1003",
            DepartureAirport = "SVO",
            ArrivalAirport = "CDG",
            DepartureDate = new DateTime(2025,9,3,9,0,0),
            ArrivalDate = new DateTime(2025,9,3,13,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 3
        },
        new Flight
        {
            Id = 4,
            Code = "SU1004",
            DepartureAirport = "LED",
            ArrivalAirport = "FRA",
            DepartureDate = new DateTime(2025,9,4,15,0,0),
            ArrivalDate = new DateTime(2025,9,4,19,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 4
        },
        new Flight
        {
            Id = 5,
            Code = "SU1005",
            DepartureAirport = "SVO",
            ArrivalAirport = "AMS",
            DepartureDate = new DateTime(2025,9,5,8,0,0),
            ArrivalDate = new DateTime(2025,9,5,12,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 5
        },
        new Flight
        {
            Id = 6,
            Code = "SU1006",
            DepartureAirport = "LED",
            ArrivalAirport = "JFK",
            DepartureDate = new DateTime(2025,9,6,11,0,0),
            ArrivalDate = new DateTime(2025,9,6,15,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 6
        },
        new Flight
        {
            Id = 7,
            Code = "SU1007",
            DepartureAirport = "SVO",
            ArrivalAirport = "LHR",
            DepartureDate = new DateTime(2025,9,7,13,0,0),
            ArrivalDate = new DateTime(2025,9,7,17,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 7
        },
        new Flight
        {
            Id = 8,
            Code = "SU1008",
            DepartureAirport = "LED",
            ArrivalAirport = "CDG",
            DepartureDate = new DateTime(2025,9,8,7,0,0),
            ArrivalDate = new DateTime(2025,9,8,11,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 8
        },
        new Flight
        {
            Id = 9,
            Code = "SU1009",
            DepartureAirport = "SVO",
            ArrivalAirport = "FRA",
            DepartureDate = new DateTime(2025,9,9,14,0,0),
            ArrivalDate = new DateTime(2025,9,9,18,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 9
        },
        new Flight
        {
            Id = 10,
            Code = "SU1010",
            DepartureAirport = "LED",
            ArrivalAirport = "AMS",
            DepartureDate = new DateTime(2025,9,10,10,0,0),
            ArrivalDate = new DateTime(2025,9,10,14,0,0),
            TravelTime = TimeSpan.FromHours(4),
            AircraftModelId = 10
        }];

    public static readonly List<Passenger> Passengers = [
        new Passenger
        {
            Id = 1,
            PassportNumber = "P000001",
            LastName = "Иванов",
            FirstName = "Иван",
            MiddleName = "Иванович",
            BirthDate = new DateOnly(1985,5,12)
        },
        new Passenger
        {
            Id = 2,
            PassportNumber = "P000002",
            LastName = "Петрова",
            FirstName = "Анна",
            MiddleName = "Сергеевна",
            BirthDate = new DateOnly(1990,7,22)
        },
        new Passenger
        {
            Id = 3,
            PassportNumber = "P000003",
            LastName = "Сидоров",
            FirstName = "Павел",
            MiddleName = null,
            BirthDate = new DateOnly(1982,11,2)
        },
        new Passenger
        {
            Id = 4,
            PassportNumber = "P000004",
            LastName = "Кузнецова",
            FirstName = "Мария",
            MiddleName = null,
            BirthDate = new DateOnly(1995,3,15)
        },
        new Passenger
        {
            Id = 5,
            PassportNumber = "P000005",
            LastName = "Смирнов",
            FirstName = "Алексей",
            MiddleName = "Игоревич",
            BirthDate = new DateOnly(1988,12,5)
        },
        new Passenger
        {
            Id = 6,
            PassportNumber = "P000006",
            LastName = "Васильева",
            FirstName = "Елена",
            MiddleName = null,
            BirthDate = new DateOnly(1992,1,20)
        },
        new Passenger
        {
            Id = 7,
            PassportNumber = "P000007",
            LastName = "Морозов",
            FirstName = "Дмитрий",
            MiddleName = "Александрович",
            BirthDate = new DateOnly(1983,6,30)
        },
        new Passenger
        {
            Id = 8,
            PassportNumber = "P000008",
            LastName = "Федорова",
            FirstName = "Ольга",
            MiddleName = null,
            BirthDate = new DateOnly(1991,9,9)
        },
        new Passenger
        {
            Id = 9,
            PassportNumber = "P000009",
            LastName = "Попов",
            FirstName = "Никита",
            MiddleName = "Сергеевич",
            BirthDate = new DateOnly(1987,4,18)
        },
        new Passenger
        {
            Id = 10,
            PassportNumber = "P000010",
            LastName = "Михайлова",
            FirstName = "Анна",
            MiddleName = null,
            BirthDate = new DateOnly(1994,8,25)
        }];

    public static readonly List<Ticket> Tickets = [
        new Ticket { Id = 1, FlightId = 1, PassengerId = 1, SeatNumber = "12A", HasHandLuggage = true, BaggageWeight = 0 },
        new Ticket { Id = 2, FlightId = 2, PassengerId = 2, SeatNumber = "13A", HasHandLuggage = false, BaggageWeight = 1 },
        new Ticket { Id = 3, FlightId = 3, PassengerId = 3, SeatNumber = "14A", HasHandLuggage = true, BaggageWeight = 0 },
        new Ticket { Id = 4, FlightId = 4, PassengerId = 4, SeatNumber = "15A", HasHandLuggage = false, BaggageWeight = 1 },
        new Ticket { Id = 5, FlightId = 5, PassengerId = 5, SeatNumber = "16A", HasHandLuggage = true, BaggageWeight = 0 },
        new Ticket { Id = 6, FlightId = 6, PassengerId = 6, SeatNumber = "17A", HasHandLuggage = false, BaggageWeight = 1 },
        new Ticket { Id = 7, FlightId = 7, PassengerId = 7, SeatNumber = "18A", HasHandLuggage = true, BaggageWeight = 0 },
        new Ticket { Id = 8, FlightId = 8, PassengerId = 8, SeatNumber = "19A", HasHandLuggage = false, BaggageWeight = 1 },
        new Ticket { Id = 9, FlightId = 9, PassengerId = 9, SeatNumber = "20A", HasHandLuggage = true, BaggageWeight = 0 },
        new Ticket { Id = 10, FlightId = 10, PassengerId = 10, SeatNumber = "21A", HasHandLuggage = false, BaggageWeight = 1 }];
}
