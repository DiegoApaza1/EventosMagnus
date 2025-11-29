using System.Threading;
using System.Threading.Tasks;
using Magnus.Domain.Interfaces;
using Magnus.Application.Mappers;
using MediatR;

namespace Magnus.Application.Features.Reportes.GenerarReporteEventos
{
    public class GenerarReporteEventosQueryHandler : IRequestHandler<GenerarReporteEventosQuery, (byte[] FileBytes, string FileName)>
    {
        private readonly IUnitOfWork _uow;
        private readonly IReportService _reportService;

        public GenerarReporteEventosQueryHandler(IUnitOfWork uow, IReportService reportService)
        {
            _uow = uow;
            _reportService = reportService;
        }

        public async Task<(byte[] FileBytes, string FileName)> Handle(GenerarReporteEventosQuery query, CancellationToken ct)
        {
            var eventos = query.OrganizadorId.HasValue
                ? await _uow.Eventos.GetByOrganizadorIdAsync(query.OrganizadorId.Value)
                : await _uow.Eventos.GetAllAsync();

            var eventosDto = eventos.ToResponseDtoList();
            return await _reportService.GenerarReporteEventosAsync(eventosDto, "ReporteEventos");
        }
    }
}
