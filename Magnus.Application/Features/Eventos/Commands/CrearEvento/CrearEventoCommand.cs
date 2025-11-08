using Magnus.Application.DTOs;

namespace Magnus.Application.Features.Eventos.Commands.CrearEvento
{
    public class CrearEventoCommand
    {
        public EventoCreacionDto Dto { get; }

        public CrearEventoCommand(EventoCreacionDto dto)
        {
            Dto = dto;
        }
    }
}