using AutoMapper;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Profiles;

public class OrganizadorProfile : Profile
{
    public OrganizadorProfile()
    {
        CreateMap<Organizador, OrganizadorResponseDto>();
    }
}
