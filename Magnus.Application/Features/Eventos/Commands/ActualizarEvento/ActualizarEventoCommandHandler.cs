using Magnus.Application.Interfaces;
using Magnus.Application.DTOs;
using Magnus.Application.Mappers;

namespace Magnus.Application.Features.Eventos.Commands.ActualizarEvento
{
    public class ActualizarEventoCommandHandler
    {
        private readonly IUnitOfWork _uow;

        public ActualizarEventoCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventoResponseDto> Handle(ActualizarEventoCommand command, CancellationToken ct = default)
        {
            // Buscar el evento existente
            var evento = await _uow.Eventos.GetByIdAsync(command.EventoId);
            if (evento == null)
                throw new InvalidOperationException($"Evento con ID {command.EventoId} no encontrado.");

            // Actualizar usando el método del dominio (mantiene las validaciones)
            evento.Update(
                command.Titulo,
                command.FechaInicio,
                command.FechaFin,
                command.Lugar,
                command.Capacidad,
                command.Descripcion
            );

            // Guardar cambios
            await _uow.CommitAsync();

            // Mapear a DTO antes de retornar
            return evento.ToResponseDto();
        }
    }
}
