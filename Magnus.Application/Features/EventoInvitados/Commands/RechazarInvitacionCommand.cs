using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    public record RechazarInvitacionCommand(Guid InvitacionId) : IRequest<Unit>;
    
    public class RechazarInvitacionCommandHandler : IRequestHandler<RechazarInvitacionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;

        public RechazarInvitacionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(RechazarInvitacionCommand request, CancellationToken cancellationToken)
        {
            var invitacion = await _uow.EventoInvitados.GetByIdAsync(request.InvitacionId);
            if (invitacion == null)
                throw new InvalidOperationException("Invitación no encontrada");

            invitacion.RechazarPorInvitado();
            
            _uow.EventoInvitados.Update(invitacion);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
