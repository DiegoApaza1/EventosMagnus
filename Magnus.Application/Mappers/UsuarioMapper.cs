using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioResponseDto ToResponseDto(this Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                CreatedAt = usuario.CreatedAt
            };
        }

        public static IEnumerable<UsuarioResponseDto> ToResponseDtoList(this IEnumerable<Usuario> usuarios)
        {
            return usuarios.Select(u => u.ToResponseDto());
        }
    }
}
