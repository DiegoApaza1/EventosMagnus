using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.EventoInvitados.Commands
{
    public record AprobarAutopostulacionCommand(Guid InvitacionId) : IRequest<Unit>;
    
    public class AprobarAutopostulacionCommandHandler : IRequestHandler<AprobarAutopostulacionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;

        public AprobarAutopostulacionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(AprobarAutopostulacionCommand request, CancellationToken cancellationToken)
        {
            var invitacion = await _uow.EventoInvitados.GetByIdAsync(request.InvitacionId);
            if (invitacion == null)
                throw new InvalidOperationException("Invitación no encontrada");

            invitacion.AprobarPorOrganizador();
            
            _uow.EventoInvitados.Update(invitacion);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
