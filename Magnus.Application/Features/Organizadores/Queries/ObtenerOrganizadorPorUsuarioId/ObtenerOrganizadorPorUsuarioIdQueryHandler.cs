using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces.Repositories;
using MediatR;

namespace Magnus.Application.Features.Organizadores.Queries.ObtenerOrganizadorPorUsuarioId
{
    public class ObtenerOrganizadorPorUsuarioIdQueryHandler : IRequestHandler<ObtenerOrganizadorPorUsuarioIdQuery, OrganizadorResponseDto?>
    {
        private readonly IUnitOfWork _uow;

        public ObtenerOrganizadorPorUsuarioIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<OrganizadorResponseDto?> Handle(ObtenerOrganizadorPorUsuarioIdQuery request, CancellationToken cancellationToken)
        {
            var organizador = await _uow.Organizadores.GetByUsuarioIdAsync(request.UsuarioId);

            if (organizador == null)
                return null;

            return new OrganizadorResponseDto
            {
                Id = organizador.Id,
                NombreEmpresa = organizador.NombreEmpresa,
                Telefono = organizador.Telefono,
                UsuarioId = organizador.UsuarioId,
                PrecioPorEvento = organizador.PrecioPorEvento,
                AñosExperiencia = organizador.AñosExperiencia,
                Descripcion = organizador.Descripcion,
                Direccion = organizador.Direccion,
                Especialidad = organizador.Especialidad,
                Verificado = organizador.Verificado,
                Rating = organizador.Rating
            };
        }
    }
}
