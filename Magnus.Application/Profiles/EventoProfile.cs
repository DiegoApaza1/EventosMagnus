using AutoMapper;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Profiles;

public class EventoProfile : Profile
{
    public EventoProfile()
    {
        CreateMap<Evento, EventoResponseDto>()
            .ForMember(dest => dest.Organizador, opt => opt.MapFrom(src => src.Organizador));

        CreateMap<Usuario, OrganizadorDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}
