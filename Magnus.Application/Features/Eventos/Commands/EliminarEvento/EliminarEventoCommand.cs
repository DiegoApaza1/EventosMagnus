using MediatR;

namespace Magnus.Application.Features.Eventos.Commands.EliminarEvento
{
    public class EliminarEventoCommand : IRequest<bool>
    {
        public Guid EventoId { get; }

        public EliminarEventoCommand(Guid eventoId)
        {
            EventoId = eventoId;
        }
    }
}
