using Magnus.Domain.Interfaces;
using MediatR;

namespace Magnus.Application.Features.Eventos.Commands.EliminarEvento
{
    public class EliminarEventoCommandHandler : IRequestHandler<EliminarEventoCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public EliminarEventoCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(EliminarEventoCommand command, CancellationToken ct = default)
        {
            var evento = await _uow.Eventos.GetByIdAsync(command.EventoId);
            if (evento == null)
                throw new InvalidOperationException($"Evento con ID {command.EventoId} no encontrado.");

            _uow.Eventos.Delete(evento);
            await _uow.CommitAsync();

            return true;
        }
    }
}
