using AutoMapper;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioResponseDto>();
    }
}
