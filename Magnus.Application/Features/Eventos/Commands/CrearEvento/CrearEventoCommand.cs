using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Eventos.Commands.CrearEvento
{
    public class CrearEventoCommand : IRequest<EventoResponseDto>
    {
        public EventoCreacionDto Dto { get; }

        public CrearEventoCommand(EventoCreacionDto dto)
        {
            Dto = dto;
        }
    }
}