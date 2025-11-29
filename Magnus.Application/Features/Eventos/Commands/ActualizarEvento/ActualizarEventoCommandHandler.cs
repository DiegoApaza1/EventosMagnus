using Magnus.Domain.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;
using MediatR;

namespace Magnus.Application.Features.Eventos.Commands.ActualizarEvento
{
    public class ActualizarEventoCommandHandler : IRequestHandler<ActualizarEventoCommand, EventoResponseDto>
    {
        private readonly IUnitOfWork _uow;

        public ActualizarEventoCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventoResponseDto> Handle(ActualizarEventoCommand command, CancellationToken ct = default)
        {
            var evento = await _uow.Eventos.GetByIdAsync(command.EventoId);
            if (evento == null)
                throw new InvalidOperationException($"Evento con ID {command.EventoId} no encontrado.");

            evento.Update(
                command.Titulo,
                command.FechaInicio,
                command.FechaFin,
                command.Lugar,
                command.Capacidad,
                command.Descripcion
            );

            await _uow.CommitAsync();

            return evento.ToResponseDto();
        }
    }
}
