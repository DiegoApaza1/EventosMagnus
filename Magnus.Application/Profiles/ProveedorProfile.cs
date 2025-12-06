using AutoMapper;
using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Profiles;

public class ProveedorProfile : Profile
{
    public ProveedorProfile()
    {
        CreateMap<Proveedor, ProveedorResponseDto>();
    }
}
