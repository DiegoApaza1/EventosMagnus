using Magnus.Application.DTOs;
using Magnus.Domain.Entities;
using Magnus.Domain.Interfaces;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Commands.CrearOrganizador
{
    public class CrearOrganizadorCommandHandler : IRequestHandler<CrearOrganizadorCommand, OrganizadorResponseDto>
    {
        private readonly IUnitOfWork _uow;

        public CrearOrganizadorCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrganizadorResponseDto> Handle(CrearOrganizadorCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _uow.Usuarios.GetByIdAsync(request.UsuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var organizador = new Organizador(request.Nombre, request.Telefono, request.UsuarioId);

            await _uow.Organizadores.AddAsync(organizador);
            await _uow.CommitAsync();

            return new OrganizadorResponseDto
            {
                Id = organizador.Id,
                Nombre = organizador.Nombre,
                Telefono = organizador.Telefono,
                UsuarioId = organizador.UsuarioId
            };
        }
    }
}
