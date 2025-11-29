using Magnus.Application.DTOs;
using MediatR;

namespace Magnus.Application.Features.Reportes.GenerarReporteEventos
{
    public class GenerarReporteEventosQuery : IRequest<(byte[] FileBytes, string FileName)>
    {
        public Guid? OrganizadorId { get; }

        public GenerarReporteEventosQuery(Guid? organizadorId = null)
        {
            OrganizadorId = organizadorId;
        }
    }
}
