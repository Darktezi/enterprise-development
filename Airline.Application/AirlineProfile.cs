using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Family;
using Airline.Application.Contracts.Model;
using Airline.Application.Contracts.Ticket;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application;

/// <summary>
/// Профиль AutoMapper для преобразования между доменными сущностями и DTO
/// </summary>
public class AirlineProfile : Profile
{
    /// <summary>
    /// Инициализирует правила маппинга между сущностями домена и DTO
    /// </summary>
    public AirlineProfile()
    {
        // Маппинг для рейсов
        CreateMap<Flight, FlightDto>();
        CreateMap<FlightCreateUpdateDto, Flight>();

        // Маппинг для пассажиров
        CreateMap<Passenger, PassengerDto>();
        CreateMap<PassengerCreateUpdateDto, Passenger>();

        // Маппинг для семейств самолетов
        CreateMap<AirlineFamily, FamilyDto>();
        CreateMap<FamilyCreateUpdateDto, AirlineFamily>();

        // Маппинг для моделей самолетов
        CreateMap<AirlineModel, ModelDto>();
        CreateMap<ModelCreateUpdateDto, AirlineModel>();

        // Маппинг для билетов
        CreateMap<Ticket, TicketDto>();
        CreateMap<TicketCreateUpdateDto, Ticket>();
    }
}