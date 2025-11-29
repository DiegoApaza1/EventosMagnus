using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorId
{
    public class ObtenerOrganizadorPorIdQueryHandler : IRequestHandler<ObtenerOrganizadorPorIdQuery, OrganizadorResponseDto?>
    {
        private readonly IUnitOfWork _uow;

        public ObtenerOrganizadorPorIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrganizadorResponseDto?> Handle(ObtenerOrganizadorPorIdQuery request, CancellationToken cancellationToken)
        {
            var organizador = await _uow.Organizadores.GetByIdAsync(request.Id);

            if (organizador == null)
                return null;

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
