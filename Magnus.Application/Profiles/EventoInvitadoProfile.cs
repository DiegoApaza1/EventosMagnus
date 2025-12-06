using AutoMapper;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Profiles;

public class EventoInvitadoProfile : Profile
{
    public EventoInvitadoProfile()
    {
        CreateMap<EventoInvitado, EventoInvitadoResponseDto>();
    }
}
