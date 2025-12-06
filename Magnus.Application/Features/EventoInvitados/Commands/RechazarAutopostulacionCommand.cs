using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    public record RechazarAutopostulacionCommand(Guid InvitacionId) : IRequest<Unit>;
    
    public class RechazarAutopostulacionCommandHandler : IRequestHandler<RechazarAutopostulacionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;

        public RechazarAutopostulacionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(RechazarAutopostulacionCommand request, CancellationToken cancellationToken)
        {
            var invitacion = await _uow.EventoInvitados.GetByIdAsync(request.InvitacionId);
            if (invitacion == null)
                throw new InvalidOperationException("Invitación no encontrada");

            invitacion.RechazarPorOrganizador();
            
            _uow.EventoInvitados.Update(invitacion);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
