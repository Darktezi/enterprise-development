using Airline.Application.Contracts.Flights;
using Airline.Domain.Entities;
using AutoMapper;

namespace Airline.Application;

public class AirlineProfile : Profile
{
    public AirlineProfile()
    {

        // Flight маппинги
        CreateMap<Flight, FlightDto>()
            .ForMember(dest => dest.AircraftModel,
                       opt => opt.MapFrom(src => src.AircraftModel.ModelName))
            .ForMember(dest => dest.TicketsCount,
                       opt => opt.MapFrom(src => src.Tickets.Count));
    }
}