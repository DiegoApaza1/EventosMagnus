using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Mappers
{
    public static class ProveedorMapper
    {
        public static ProveedorResponseDto ToResponseDto(this Proveedor proveedor)
        {
            return new ProveedorResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Servicio = proveedor.Servicio
            };
        }

        public static IEnumerable<ProveedorResponseDto> ToResponseDtoList(this IEnumerable<Proveedor> proveedores)
        {
            return proveedores.Select(p => p.ToResponseDto());
        }
    }
}
