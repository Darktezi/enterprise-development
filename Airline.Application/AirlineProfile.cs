using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application;

/// <summary>
/// Профиль AutoMapper для преобразования сущностей домена в DTO, используемые в API.
/// Обеспечивает маппинг между внутренними моделями и контрактами, скрывая детали реализации от клиентов.
/// </summary>
public class AirlineProfile : Profile
{
    /// <summary>
    /// Инициализирует правила преобразования между сущностями и их DTO-представлениями.
    /// </summary>
    public AirlineProfile()
    {
        CreateMap<Flight, FlightDto>()
            .ForMember(dest => dest.AircraftModel,
                       opt => opt.MapFrom(src => src.AircraftModel!.ModelName))
            .ForMember(dest => dest.TicketsCount,
                       opt => opt.MapFrom(src => src.Tickets!.Count));

        CreateMap<Passenger, PassengerDto>()
            .ForMember(dest => dest.TicketsCount,
                       opt => opt.MapFrom(src => src.Tickets!.Count));
    }
}