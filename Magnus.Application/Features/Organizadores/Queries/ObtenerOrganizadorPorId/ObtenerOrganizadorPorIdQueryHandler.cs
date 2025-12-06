using Magnus.Application.DTOs;
using Magnus.Domain.Interfaces.Repositories;
using Magnus.Domain.Interfaces.Services;
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
                NombreEmpresa = organizador.NombreEmpresa,
                Descripcion = organizador.Descripcion,
                Telefono = organizador.Telefono,
                Direccion = organizador.Direccion,
                PrecioPorEvento = organizador.PrecioPorEvento,
                AñosExperiencia = organizador.AñosExperiencia,
                Especialidad = organizador.Especialidad,
                Verificado = organizador.Verificado,
                Rating = organizador.Rating,
                UsuarioId = organizador.UsuarioId
            };
        }
    }
}
