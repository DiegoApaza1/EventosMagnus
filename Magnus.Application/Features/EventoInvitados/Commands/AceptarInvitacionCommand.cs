using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    public record AceptarInvitacionCommand(Guid InvitacionId) : IRequest<Unit>;
    
    public class AceptarInvitacionCommandHandler : IRequestHandler<AceptarInvitacionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;

        public AceptarInvitacionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(AceptarInvitacionCommand request, CancellationToken cancellationToken)
        {
            var invitacion = await _uow.EventoInvitados.GetByIdAsync(request.InvitacionId);
            if (invitacion == null)
                throw new InvalidOperationException("Invitación no encontrada");

            invitacion.AceptarPorInvitado();
            
            _uow.EventoInvitados.Update(invitacion);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
