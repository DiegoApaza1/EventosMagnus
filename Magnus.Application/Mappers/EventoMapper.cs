using Magnus.Application.DTOs;
using Magnus.Domain.Entities;

namespace Magnus.Application.Mappers
{
    public static class EventoMapper
    {
        public static EventoResponseDto ToResponseDto(this Evento evento)
        {
            return new EventoResponseDto
            {
                Id = evento.Id,
                Titulo = evento.Titulo,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                Lugar = evento.Lugar,
                Capacidad = evento.Capacidad,
                OrganizadorId = evento.OrganizadorId,
                CreatedAt = evento.CreatedAt,
                Organizador = evento.Organizador?.ToOrganizadorDto()
            };
        }

        public static OrganizadorDto? ToOrganizadorDto(this Usuario usuario)
        {
            if (usuario == null) return null;
            
            return new OrganizadorDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email
            };
        }

        public static IEnumerable<EventoResponseDto> ToResponseDtoList(this IEnumerable<Evento> eventos)
        {
            return eventos.Select(e => e.ToResponseDto());
        }
    }
}
