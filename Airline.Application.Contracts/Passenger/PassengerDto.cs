namespace Airline.Application.Contracts.Passenger;

public record PassengerDto(
    int Id,
    string PassportNumber,
    string LastName,
    string FirstName,
    string? MiddleName,
    DateOnly? BirthDate,
    int TicketsCount
);